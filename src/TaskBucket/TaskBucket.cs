using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Jobs;

namespace TaskBucket
{
    internal class TaskBucket: ITaskBucket
    {
        private readonly IServiceProvider _services;

        private readonly IBucketOptions _options;

        private readonly ILogger<ITaskBucket> _logger;

        private readonly CancellationTokenSource _cancellationToken;

        private readonly ConcurrentQueue<IJobReference> _jobQueue = new ConcurrentQueue<IJobReference>();

        private readonly Dictionary<Guid, IJobReference> _runningJobs = new Dictionary<Guid, IJobReference>();

        private readonly List<IJobReference> _jobHistory = new List<IJobReference>();

        private readonly object _jobLock = new object();

        private readonly object _queueLock = new object();

        public IReadOnlyList<IJobReference> JobHistory => _jobHistory;

        public IReadOnlyList<IJobReference> Jobs
        {
            get
            {
                List<IJobReference> jobs;

                lock(_queueLock)
                {
                    jobs = _jobQueue.ToList();
                }

                lock(_jobLock)
                {
                    jobs.AddRange(_runningJobs.Values.ToList());
                }

                return jobs;
            }
        }

        public TaskBucket(IBucketOptions options, IServiceProvider services = null, ILogger<ITaskBucket> logger = null, CancellationTokenSource cancellationToken = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(services));
            _services = services;
            _logger = logger;
            _cancellationToken = cancellationToken;
        }

        public IJobReference AddBackgroundJob(IJobReference job)
        {
            _logger?.LogInformation($"Adding new Job: {job.Identity} to TaskBucket");

            lock(_queueLock)
            {
                _jobQueue.Enqueue(job);
            }

            TryStartJob();

            return job;
        }

        public void ClearJobHistory()
        {
            _jobHistory.Clear();
        }

        public IJobReference AddBackgroundJob<TInstance>(Func<TInstance, Task> action)
        {
            IJobReference newJob = new Job<TInstance>(action, OnJobComplete);

            return AddBackgroundJob(newJob);
        }

        public IJobReference AddBackgroundJob<TInstance>(Func<TInstance, IJobReference, Task> action)
        {
            IJobReference newJob = new Job<TInstance>(action, OnJobComplete);

            return AddBackgroundJob(newJob);
        }

        public IJobReference AddBackgroundJob<TInstance>(TInstance instance, Func<TInstance, Task> action)
        {
            IJobReference newJob = new InstanceJob<TInstance>(instance, action, OnJobComplete);

            return AddBackgroundJob(newJob);
        }

        public IJobReference AddBackgroundJob<TInstance>(TInstance instance, Func<TInstance, IJobReference, Task> action)
        {
            IJobReference newJob = new InstanceJob<TInstance>(instance, action, OnJobComplete);

            return AddBackgroundJob(newJob);
        }

        public List<IJobReference> AddBackgroundJobs<TInstance, TValue>(IEnumerable<TValue> values, Func<TInstance, TValue, Task> action)
        {
            List<IJobReference> references = new List<IJobReference>();

            foreach(TValue value in values)
            {
                references.Add(AddBackgroundJob(new ParameterJob<TInstance, TValue>(action, value, OnJobComplete)));
            }

            return references;
        }

        public List<IJobReference> AddBackgroundJobs<TInstance, TValue>(IEnumerable<TValue> values, Func<TInstance, TValue, IJobReference, Task> action)
        {
            List<IJobReference> references = new List<IJobReference>();

            foreach(TValue value in values)
            {
                references.Add(AddBackgroundJob(new ParameterJob<TInstance, TValue>(action, value, OnJobComplete)));
            }

            return references;
        }

        private void TryStartJob(int threadIndex = -1)
        {
            lock(_jobLock)
            {
                if(_runningJobs.Count >= _options.MaxBackgroundThreads)
                {
                    return;
                }

                if(threadIndex == -1)
                {
                    threadIndex = _runningJobs.Count;
                }

                lock(_queueLock)
                {
                    if(!_jobQueue.TryDequeue(out IJobReference job))
                    {
                        return;
                    }

                    _logger.LogDebug($"Starting Job: {job.Identity}");

                    StartJobAsync(job, threadIndex);
                }
            }
        }

        private Task StartJobAsync(IJobReference jobReference, int threadIndex)
        {
            _runningJobs.Add(jobReference.Identity, jobReference);

            return Task.Factory.StartNew(async () =>
            {
                if(jobReference is IJob job)
                {
                    await job.ExecuteAsync(threadIndex);
                }
                else if(jobReference is IServiceJob serviceJob)
                {
                    IServiceScope scope = _services.CreateScope();

                    await serviceJob.ExecuteAsync(scope.ServiceProvider, threadIndex);

                    scope.Dispose();
                }
            }, _cancellationToken.Token);
        }

        private void OnJobComplete(IJobReference jobReference)
        {
            _logger?.LogDebug($"Job: {jobReference.Identity} Completed");

            lock(_jobLock)
            {
                _runningJobs.Remove(jobReference.Identity);
            }

            _jobHistory.Add(jobReference.GetJobReference());

            TryStartJob(jobReference.ThreadIndex);
        }
    }
}
