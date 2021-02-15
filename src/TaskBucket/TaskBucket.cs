using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TaskBucket
{
    public class TaskBucket: ITaskBucket
    {
        private readonly IServiceProvider _services;

        private readonly IBucketOptions _options;

        private ILogger<ITaskBucket> _logger;

        //private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private readonly ConcurrentQueue<IJob> _jobQueue = new ConcurrentQueue<IJob>();

        private readonly Dictionary<Guid, IJob> _runningJobs = new Dictionary<Guid, IJob>();

        private readonly object _jobLock = new object();

        public List<IJobReference> Tasks
        {
            get
            {
                List<IJobReference> jobs = _jobQueue.ToList<IJobReference>();

                jobs.AddRange(_runningJobs.Values.ToList<IJobReference>());

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
            _jobQueue.Enqueue(job);

            TryStartJob();

            return job;
        }

        public IJobReference AddBackgroundJob<T>(Func<T, Task> action)
        {
            IJob newJob = new Job<T>(action, OnJobComplete);

            return AddBackgroundJob(newJob);
        }

        public List<IJobReference> AddBackgroundTasks<T, TValue>(IEnumerable<TValue> values, Func<T, TValue, Task> action)
        {
            List<IJobReference> references = new List<IJobReference>();

            foreach(TValue value in values)
            {
                references.Add(new Job<T, TValue>(value, action, OnJobComplete));
            }

            return references;
        }

        private void TryStartJob()
        {
            lock(_jobLock)
            {
                if(_runningJobs.Count < _options.Instances && _jobQueue.TryDequeue(out IJob job))
                {
                    StartJobAsync(job);
                }
            }
        }

        private void StartJobAsync(IJob job)
        {
            _runningJobs.Add(job.Identity, job);

            Task.Factory.StartNew(async () =>
            {
                using(IServiceScope scope = _services.CreateScope())
                {
                    await job.ExecuteAsync(scope.ServiceProvider);
                }
            });
        }

        private void OnJobComplete(IJobReference job)
        {
            _runningJobs.Remove(job.Identity);

            TryStartJob();
        }

        //#region IDisposable

        //private bool _disposed;

        //public void Dispose()
        //{
        //    Dispose(true);

        //    GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(bool disposing)
        //{
        //    if(_disposed)
        //    {
        //        return;
        //    }

        //    if(disposing)
        //    {
        //        _cancellationToken.Dispose();
        //    }

        //    _disposed = true;
        //}

        //#endregion

        public static void Wait(IJobReference job)
        {
            while (job.Status == TaskStatus.Pending || job.Status == TaskStatus.Running)
            {
                Thread.Sleep(25);
            }
        }

        public static void WaitAll(params IJobReference[] jobs)
        {
            foreach (IJobReference job in jobs)
            {
                Wait(job);
            }
        }
    }
}
