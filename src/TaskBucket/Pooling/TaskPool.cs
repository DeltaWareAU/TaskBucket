using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Extensions;
using TaskBucket.Pooling.Options;
using TaskBucket.Tasks;
using TaskBucket.Tasks.Asynchronous;
using TaskBucket.Tasks.Enums;
using TaskBucket.Tasks.Synchronous;

namespace TaskBucket.Pooling
{
    internal class TaskPool : ITaskPool
    {
        private readonly CancellationTokenSource _cancellationSource = new();
        private readonly ILogger _logger;

        private readonly ITaskPoolOptions _options;

        // TODO: This should be an IServiceScopeFactory.
        private readonly IServiceProvider _services;
        private readonly PriorityTaskQueue _taskQueue = new();

        private readonly Timer _trimQueueTimer;
        private readonly ITaskDetails[] _workerThreads;

        public bool IsRunning => _workerThreads.Any(i => i is { State: TaskState.Running });

        public int ExecutingTaskCount => _workerThreads.Count(i => i is { State: TaskState.Running });

        public TaskPool(ITaskPoolOptions options, IServiceProvider services, ILogger<ITaskPool> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _services = services ?? throw new ArgumentNullException(nameof(services));

            _logger = logger;

            int threadCount = Environment.ProcessorCount;

            if (_options.WorkerThreadCount > threadCount)
            {
                _logger?.LogWarning("The specified WorkerThread count of {threadCount} is greater than the systems thread count of {threadCount} and may cause a degradation in performance.", _options.WorkerThreadCount, threadCount);
            }

            _workerThreads = new ITaskDetails[_options.WorkerThreadCount];

            _logger?.LogDebug("A new instance of {nameof} has been instantiated with {threadCount} WorkerThreads", nameof(TaskPool), _workerThreads.Length);

            _trimQueueTimer = new Timer(TrimQueue, null, TimeSpan.Zero, _options.TaskQueueTrimInterval);
        }

        public void CancelAllCancellableTasks()
        {
            if (_cancellationSource.IsCancellationRequested)
            {
                return;
            }

            _logger?.LogDebug("Cancelling all tasks that inherit {nameof}", nameof(ICancellableTask));

            _cancellationSource.Cancel();
        }

        public void EnqueueTask(ITaskDetails task)
        {
            _logger?.LogDebug("Task[{taskId}] Has been added to the pool and will be processed when a worker thread becomes available.", task.Identity);

            _taskQueue.Enqueue(task);
        }

        /// <summary>
        /// Checks to see if there is a worker thread open, if there is the task is assigned and started.
        /// </summary>
        public Task StartPendingTasksAsync()
        {
            List<Task> tasksStarted = new List<Task>();

            for (int i = 0; i < _workerThreads.Length; i++)
            {
                if (_workerThreads[i]?.State is TaskState.Pending or TaskState.Running)
                {
                    // This thread is currently in use so we skip it.
                    continue;
                }

                if (!_taskQueue.TryDequeue(out ITaskDetails task))
                {
                    // As there are no pending tasks, we exit the loop.
                    return Task.CompletedTask;
                }

                if (task.Options.InstanceLimit == InstanceLimit.Single && IsTaskInstanceRunning(task))
                {
                    _logger?.LogDebug("Task[{taskId}] has not been started as a previous instance of it is still running.", task.Identity);

                    // An instance of this task is currently running, so we won't start it.
                    continue;
                }

                // Assigned the current task to a worker thread and start it.
                _workerThreads[i] = task;

                tasksStarted.Add(StartTaskAsync(task, i));
            }

            return Task.WhenAll(tasksStarted);
        }

        private bool IsTaskInstanceRunning(ITaskDetails task)
        {
            return _workerThreads.Any(t =>
                t != null &&
                t.Identity == task.PreviouslyRanInstance.Identity &&
                t.State is TaskState.Pending or TaskState.Running);
        }

        private async Task StartTaskAsync(ITaskDetails task, int threadIndex)
        {
            IServiceScope scope = null;

            object serviceInstance;

            try
            {
                _logger?.LogTrace("WorkerThread[{threadIndex}].Task[{taskId}] Creating scope.", threadIndex, task.Identity);

                scope = _services.CreateScope();

                serviceInstance = scope.ServiceProvider.GetService(task.ServiceType);
            }
            catch (Exception e)
            {
                scope?.Dispose();

                _logger?.LogError(e, "WorkerThread[{threadIndex}].Task[{taskId}] Encountered an exception whilst creating its scope.", threadIndex, task.Identity);

                return;
            }

            try
            {
                _logger?.LogDebug("WorkerThread[{threadIndex}].Task[{taskId}] Has Started.", threadIndex, task.Identity);

                if (task is IAsynchronousTask asyncTask)
                {
                    await asyncTask.RunTaskAsync(serviceInstance, threadIndex, _cancellationSource.Token);
                }
                else if (task is ISynchronousTask syncTask)
                {
                    syncTask.RunTask(serviceInstance, threadIndex, _cancellationSource.Token);
                }

                _logger?.LogInformation("WorkerThread[{threadIndex}].Task[{taskId}] Completed successfully in {time}.", threadIndex, task.Identity, task.ExecutionTime.ToHumanTimeString());
            }
            catch (Exception e)
            {
                _logger?.LogWarning(e, "WorkerThread[{threadIndex}].Task[{taskId}] Encountered an exception.", threadIndex, task.Identity);
            }
            finally
            {
                _logger?.LogTrace("WorkerThread[{threadIndex}].Task[{taskId}] Disposed scope.", threadIndex, task.Identity);

                scope.Dispose();
            }
        }

        private void TrimQueue(object state)
        {
            _logger?.LogDebug("Trimming Task Queue");

            _taskQueue.Trim();
        }
    }
}