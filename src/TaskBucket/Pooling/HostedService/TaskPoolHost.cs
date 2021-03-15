using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBucket.Pooling.HostedService
{
    internal class TaskPoolHost: IHostedService, IDisposable
    {
        private readonly TimeSpan _taskQueueFrequency = TimeSpan.FromSeconds(1);

        private readonly ILogger _logger;

        private readonly ITaskPool _taskPool;

        private Timer _taskQueueTimer;

        private bool _enabled;

        public TaskPoolHost(ILogger<TaskPoolHost> logger, ITaskPool taskPool)
        {
            _logger = logger;

            _taskPool = taskPool ?? throw new ArgumentNullException(nameof(taskPool));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if(_enabled)
            {
                return Task.CompletedTask;
            }

            _enabled = true;

            _taskQueueTimer = new Timer(StartPendingTasks, null, TimeSpan.Zero, _taskQueueFrequency);

            _logger.LogInformation("TaskBucket.TaskPool has Started");

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if(!_enabled)
            {
                return;
            }

            _enabled = false;

            // Stops the scheduler from being invoked again
            _taskQueueTimer.Change(Timeout.Infinite, Timeout.Infinite);

            _taskPool.CancelAllCancellableTasks();

            if(_taskPool.IsRunning)
            {
                _logger?.LogWarning(
                    "TaskBucket.TaskPool has stopped but there are tasks still running that could not be stopped - app shutdown will be postponed until these tasks have completed.");
            }
            else
            {
                _logger.LogInformation("TaskBucket.TaskPool has stopped.");
            }

            while(_taskPool.IsRunning)
            {
                await Task.Delay(50, cancellationToken);
            }
        }

        private void StartPendingTasks(object state)
        {
            if(_enabled)
            {
                _taskPool.StartPendingTasks();
            }
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
                _taskQueueTimer?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
