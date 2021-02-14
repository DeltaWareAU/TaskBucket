using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBucket
{
    internal class TaskBucket: ITaskBucket, IDisposable
    {
        private readonly IServiceProvider _services;

        private readonly IBucketOptions _options;

        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        private readonly ConcurrentQueue<ITask> _jobQueue = new ConcurrentQueue<ITask>();

        private readonly Dictionary<Guid, ITask> _runningJobs = new Dictionary<Guid, ITask>();

        private readonly object _jobLock = new object();

        public List<ITaskReference> Tasks
        {
            get
            {
                List<ITaskReference> jobs = _jobQueue.ToList<ITaskReference>();

                jobs.AddRange(_runningJobs.Values.ToList<ITaskReference>());

                return jobs;
            }
        }

        public int Instances { get; set; } = 10;

        public TaskBucket(IServiceProvider services, IBucketOptions options)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _options = options ?? throw new ArgumentNullException(nameof(services));
        }

        public ITaskReference AddBackgroundTask(ITask task)
        {
            _jobQueue.Enqueue(task);

            TryStartJob();

            return task;
        }

        public ITaskReference AddBackgroundTask<T>(Func<T, Task> action)
        {
            ITask newTask = new Task<T>(action, OnJobComplete);

            return AddBackgroundTask(newTask);
        }

        public List<ITaskReference> AddBackgroundTasks<T, TValue>(IEnumerable<TValue> values, Func<T, TValue, Task> action)
        {
            List<ITaskReference> references = new List<ITaskReference>();

            foreach (TValue value in values)
            {
                references.Add(new Task<T, TValue>(value, action, OnJobComplete));
            }

            return references;
        }

        private void TryStartJob()
        {
            lock(_jobLock)
            {
                if(_runningJobs.Count < Instances && _jobQueue.TryDequeue(out ITask job))
                {
                    StartJobAsync(job);
                }
            }
        }

        private Task StartJobAsync(ITask task)
        {
            _runningJobs.Add(task.Identity, task);

            return Task.Factory.StartNew(() =>
            {
                using IServiceScope scope = _services.CreateScope();

                task.ExecuteAsync(scope);
            }, _cancellationToken.Token);
        }

        private void OnJobComplete(ITaskReference task)
        {
            _runningJobs.Remove(task.Identity);

            TryStartJob();
        }

        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(_disposed)
            {
                return;
            }

            if(disposing)
            {
                _cancellationToken.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
