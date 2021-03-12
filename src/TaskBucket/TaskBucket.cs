using DeltaWare.SDK.Common.Collections.RecyclingQueue;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskBucket.Options;
using TaskBucket.Tasks;

namespace TaskBucket
{
    internal class TaskBucket: ITaskBucket
    {
        private readonly IServiceProvider _services;

        private readonly IBucketOptions _options;

        private readonly ILogger<ITaskBucket> _logger;

        private readonly ConcurrentQueue<ITaskReference> _taskQueue = new ConcurrentQueue<ITaskReference>();

        private readonly Dictionary<Guid, ITaskReference> _runningTasks = new Dictionary<Guid, ITaskReference>();

        private readonly ConcurrentRecyclingQueue<ITaskReference> _taskHistory;

        private readonly object _taskLock = new object();

        private readonly object _queueLock = new object();

        public IReadOnlyList<ITaskReference> TaskHistory => _taskHistory.ToList();

        public IReadOnlyList<ITaskReference> PendingTasks
        {
            get
            {
                lock(_queueLock)
                {
                    return _taskQueue.ToList();
                }
            }
        }

        public IReadOnlyList<ITaskReference> RunningTasks
        {
            get
            {
                lock(_taskLock)
                {
                    return _runningTasks.Values.ToList();
                }
            }
        }

        public TaskBucket(IBucketOptions options, IServiceProvider services = null, ILogger<ITaskBucket> logger = null)
        {
            _options = options ?? throw new ArgumentNullException(nameof(services));
            _services = services;
            _logger = logger;

            _taskHistory = new ConcurrentRecyclingQueue<ITaskReference>(_options.JobHistoryDepth);
        }

        public ITaskReference AddBackgroundTask(ITaskReference serviceTask)
        {
            _logger?.LogInformation($"Adding new ServiceTask: {serviceTask.Identity} to TaskBucket");

            lock(_queueLock)
            {
                _taskQueue.Enqueue(serviceTask);
            }

            TryStartJob();

            return serviceTask;
        }

        public void ClearTaskHistory()
        {
            _taskHistory.Clear();
        }

        public ITaskReference AddBackgroundTask<TDefinition>(Func<TDefinition, Task> action)
        {
            IServiceTask newTask = new ServiceTask<TDefinition>(action, OnJobComplete);

            return AddBackgroundTask(newTask);
        }

        public ITaskReference AddBackgroundTask<TDefinition>(Func<TDefinition, ITaskReference, Task> action)
        {
            IServiceTask newTask = new ServiceTask<TDefinition>(action, OnJobComplete);

            return AddBackgroundTask(newTask);
        }

        public ITaskReference AddBackgroundTask<TDefinition>(TDefinition instance, Func<TDefinition, Task> action)
        {
            IInstanceTask newTask = new InstanceTask<TDefinition>(instance, action, OnJobComplete);

            return AddBackgroundTask(newTask);
        }

        public ITaskReference AddBackgroundTask<TDefinition>(TDefinition instance, Func<TDefinition, ITaskReference, Task> action)
        {
            IInstanceTask newTask = new InstanceTask<TDefinition>(instance, action, OnJobComplete);

            return AddBackgroundTask(newTask);
        }

        public List<ITaskReference> AddBackgroundTasks<TDefinition, TParameter>(IEnumerable<TParameter> parameters, Func<TDefinition, TParameter, Task> action)
        {
            List<ITaskReference> references = new List<ITaskReference>();

            foreach(TParameter parameter in parameters)
            {
                references.Add(AddBackgroundTask(new ParameterTask<TDefinition, TParameter>(action, parameter, OnJobComplete)));
            }

            return references;
        }

        public List<ITaskReference> AddBackgroundTasks<TDefinition, TParameter>(IEnumerable<TParameter> parameters, Func<TDefinition, TParameter, ITaskReference, Task> action)
        {
            List<ITaskReference> taskReferences = new List<ITaskReference>();

            foreach(TParameter parameter in parameters)
            {
                taskReferences.Add(AddBackgroundTask(new ParameterTask<TDefinition, TParameter>(action, parameter, OnJobComplete)));
            }

            return taskReferences;
        }

        private void TryStartJob(int threadIndex = -1)
        {
            lock(_taskLock)
            {
                if(_runningTasks.Count >= _options.MaxBackgroundThreads)
                {
                    return;
                }

                if(threadIndex == -1)
                {
                    threadIndex = _runningTasks.Count;
                }

                lock(_queueLock)
                {
                    if(!_taskQueue.TryDequeue(out ITaskReference taskReference))
                    {
                        return;
                    }

                    _logger.LogDebug($"Starting ServiceTask: {taskReference.Identity}");

                    StartJobAsync(taskReference, threadIndex);
                }
            }
        }

        private Task StartJobAsync(ITaskReference taskReference, int threadIndex)
        {
            _runningTasks.Add(taskReference.Identity, taskReference);

            return Task.Factory.StartNew(async () =>
            {
                switch(taskReference)
                {
                    case IInstanceTask task:
                    await ExecuteInstanceTask(task, threadIndex);
                    break;
                    case IServiceTask task:
                    await ExecuteServiceTask(task, threadIndex);
                    break;
                }
            });
        }

        private void OnJobComplete(ITaskReference serviceTask)
        {
            _logger?.LogDebug($"ServiceTask: {serviceTask.Identity} Completed");

            lock(_taskLock)
            {
                _runningTasks.Remove(serviceTask.Identity);
            }

            if(_options.JobHistoryEnabled)
            {
                _taskHistory.Add(serviceTask.GetTaskReference());
            }


            TryStartJob(serviceTask.ThreadIndex);
        }

        private async Task ExecuteInstanceTask(IInstanceTask task, int threadIndex)
        {
            await task.ExecuteAsync(threadIndex);
        }

        private async Task ExecuteServiceTask(IServiceTask task, int threadIndex)
        {
            IServiceScope scope = _services.CreateScope();

            await task.ExecuteAsync(scope.ServiceProvider, threadIndex);

            scope.Dispose();
        }
    }
}
