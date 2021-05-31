using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Pooling.Options;
using TaskBucket.Scheduling.Scheduler;
using TaskBucket.Tasks;
using TaskStatus = TaskBucket.Tasks.Enums.TaskStatus;

namespace TaskBucket.Pooling
{
    internal class TaskPool : ITaskPool
    {
        private readonly ILogger _logger;

        private readonly IServiceProvider _services;

        private readonly ITaskPoolOptions _options;

        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();

        private readonly ConcurrentTaskQueue _taskQueue = new ConcurrentTaskQueue();

        private readonly ITask[] _threadPool;

        public bool IsRunning => _threadPool.Any(i => i != null);

        public TaskPool(ITaskPoolOptions options, IServiceProvider services, ILogger<ITaskScheduler> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _services = services ?? throw new ArgumentNullException(nameof(services));

            _logger = logger;

            _threadPool = new ITask[_options.MaxConcurrentThreads];
        }

        public void EnqueueTask(ITask task)
        {
            _logger?.LogInformation($"Adding new Task:{task.Identity}");

            _taskQueue.Enqueue(task);
        }

        public void CancelAllCancellableTasks()
        {
            if (_cancellationSource.IsCancellationRequested)
            {
                return;
            }

            _cancellationSource.Cancel();
        }

        public Task StartPendingTasksAsync()
        {
            // Check thread space availability.
            List<Task> runningTasks = new List<Task>();

            for (int i = 0; i < _threadPool.Length; i++)
            {
                if (_threadPool[i] != null && (_threadPool[i].Status == TaskStatus.Pending || _threadPool[i].Status == TaskStatus.Running))
                {
                    // This thread is currently in use so we skip it.
                    continue;
                }

                // This thread is empty so we can try to queue up a new task.
                if (!_taskQueue.TryDequeue(out ITask task))
                {
                    // As there are no pending tasks, we exit the loop.
                    return Task.CompletedTask;
                }

                _threadPool[i] = task;

                runningTasks.Add(StartTaskAsync(task, i));
            }

            _logger?.LogDebug("TaskBucket.TaskPool has started {taskCount} background tasks", runningTasks.Count);

            return Task.WhenAll(runningTasks);
        }

        private async Task StartTaskAsync(ITask task, int threadIndex)
        {
            await Task.Factory.StartNew(async () =>
            {
                IServiceScope scope = null;

                try
                {
                    _logger.LogDebug($"Task: {task.Identity} has Started");

                    scope = _services.CreateScope();

                    await task.StartAsync(scope.ServiceProvider, threadIndex, _cancellationSource.Token);

                    _logger.LogDebug($"Task: {task.Identity} has Ended");
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "An exception occurred whilst trying to start Task:{taskId}",
                        task.Identity);
                }
                finally
                {
                    scope?.Dispose();
                }
            });
        }
    }
}
