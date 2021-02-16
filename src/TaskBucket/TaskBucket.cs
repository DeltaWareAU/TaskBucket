using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskBucket
{
    internal class TaskBucket: ITaskBucket
    {
        private readonly IServiceProvider _services;

        private readonly IBucketOptions _options;

        private readonly ILogger<ITaskBucket> _logger;

        //private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private readonly ConcurrentQueue<IJob> _jobQueue = new ConcurrentQueue<IJob>();

        private readonly Dictionary<Guid, IJob> _runningJobs = new Dictionary<Guid, IJob>();

        private readonly object _jobLock = new object();

        private readonly object _queueLock = new object();

        public List<IJobReference> Tasks
        {
            get
            {
                List<IJobReference> jobs;

                lock(_queueLock)
                {
                    jobs = _jobQueue.ToList<IJobReference>();
                }

                lock(_jobLock)
                {
                    jobs.AddRange(_runningJobs.Values.ToList<IJobReference>());
                }

                return jobs;
            }
        }

        public TaskBucket(IServiceProvider services, IBucketOptions options, ILogger<ITaskBucket> logger)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _options = options ?? throw new ArgumentNullException(nameof(services));
            _logger = logger;
        }

        public IJobReference AddBackgroundJob(IJob job)
        {
            _logger?.LogInformation($"Adding new Job: {job.Identity} to TaskBucket");

            lock(_queueLock)
            {
                _jobQueue.Enqueue(job);
            }

            TryStartJob();

            return job;
        }

        public IJobReference AddBackgroundJob<T>(Func<T, Task> action)
        {
            IJob newJob = new Job<T>(action, OnJobComplete);

            return AddBackgroundJob(newJob);
        }

        public List<IJobReference> AddBackgroundJobs<T, TValue>(IEnumerable<TValue> values, Func<T, TValue, Task> action)
        {
            List<IJobReference> references = new List<IJobReference>();

            foreach(TValue value in values)
            {
                references.Add(AddBackgroundJob(new Job<T, TValue>(action, value, OnJobComplete)));
            }

            return references;
        }

        private void TryStartJob(int threadIndex = -1)
        {
            lock(_jobLock)
            {
                if(_runningJobs.Count >= _options.Instances)
                {
                    return;
                }

                if(threadIndex == -1)
                {
                    threadIndex = _runningJobs.Count;
                }

                lock(_queueLock)
                {
                    if(!_jobQueue.TryDequeue(out IJob job))
                    {
                        return;
                    }

                    _logger.LogInformation($"Starting Job: {job.Identity}");

                    StartJobAsync(job, threadIndex);
                }
            }
        }

        private void StartJobAsync(IJob job, int threadIndex)
        {
            _runningJobs.Add(job.Identity, job);

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    IServiceScope scope = _services.CreateScope();

                    await job.ExecuteAsync(scope.ServiceProvider, threadIndex);

                    scope.Dispose();
                }
                catch(Exception e)
                {
                    throw e;
                }
            });
        }

        private void OnJobComplete(IJobReference job)
        {
            _logger?.LogInformation($"Job: {job.Identity} Completed");

            lock(_jobLock)
            {
                _runningJobs.Remove(job.Identity);
            }

            TryStartJob(job.ThreadIndex);
        }
    }
}
