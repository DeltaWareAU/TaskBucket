using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Scheduling.Options;
using TaskBucket.Tasks;
using TaskStatus = TaskBucket.Tasks.Enums.TaskStatus;

namespace TaskBucket.Scheduling.Scheduler
{
    internal class Scheduler: IScheduler
    {
        private readonly ILogger _logger;

        private readonly IServiceProvider _services;

        private readonly ISchedulerOptions _options;

        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();

        private readonly object _taskLock = new object();

        private readonly ConcurrentTaskQueue _taskQueue = new ConcurrentTaskQueue();

        private readonly ITask[] _taskThreads;

        public bool IsRunning => _taskThreads.Any(i => i != null);

        public bool Enabled { get; set; }

        public Scheduler(ISchedulerOptions options, IServiceProvider services, ILogger<IScheduler> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _services = services ?? throw new ArgumentNullException(nameof(services));

            _logger = logger;

            _taskThreads = new ITask[_options.MaxConcurrentThreads];
        }

        public void ScheduleTask(ITask task)
        {
            _logger?.LogInformation($"Adding new Task:{task.Identity}");

            _taskQueue.Enqueue(task);
        }

        public void CancelAllCancellableTasks()
        {
            if(_cancellationSource.IsCancellationRequested)
            {
                return;
            }

            _cancellationSource.Cancel();
        }

        public void RunScheduler()
        {
            if(!Enabled)
            {
                return;
            }

            lock(_taskLock)
            {
                // Check each task thread for new thread space.
                for(int i = 0; i < _taskThreads.Length; i++)
                {
                    if(_taskThreads[i].Status == TaskStatus.Pending || _taskThreads[i].Status == TaskStatus.Running)
                    {
                        // This thread is currently in use so we skip it.
                        continue;
                    }

                    // This thread is empty so we can try to queue up a new task.
                    if(!_taskQueue.TryDequeue(out ITask task))
                    {
                        // As there are no pending tasks, we exit the loop.
                        return;
                    }

                    _taskThreads[i] = task;

                    StartTaskAsync(task, i);
                }
            }
        }

        private async void StartTaskAsync(ITask task, int threadIndex)
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
                catch(Exception e)
                {
                    _logger.LogWarning(e, "An exception occurred whilst trying to start Task:{taskId}", task.Identity);
                }
                finally
                {
                    scope?.Dispose();
                }
            });
        }

        private async Task ExecuteServiceTaskAsync(ITask task, int threadIndex)
        {
            IServiceScope scope = _services.CreateScope();

            await task.StartAsync(scope.ServiceProvider, threadIndex, _cancellationSource.Token);

            scope.Dispose();
        }
    }
}
