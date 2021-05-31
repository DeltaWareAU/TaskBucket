using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Pooling.Options;

namespace TaskBucket.Pooling.HostedService
{
    internal class TaskPoolHost : IHostedService, IDisposable
    {
        private readonly ILogger _logger;

        private readonly ITaskPool _taskPool;

        private readonly ITaskPoolOptions _options;

        private Timer _taskQueueTimer;

        private bool _enabled;

        public TaskPoolHost(ILogger<TaskPoolHost> logger, ITaskPool taskPool, ITaskPoolOptions options)
        {
            _logger = logger;

            _taskPool = taskPool ?? throw new ArgumentNullException(nameof(taskPool));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_enabled)
            {
                return Task.CompletedTask;
            }

            _enabled = true;

            _taskQueueTimer = new Timer(StartPendingTasksAsync, null, TimeSpan.Zero, _options.CheckPendingTasksFrequency);

            _logger.LogInformation("TaskBucket.TaskPool has Started");

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (!_enabled)
            {
                return;
            }

            _enabled = false;

            // Stops the scheduler from being invoked again
            _taskQueueTimer.Change(Timeout.Infinite, Timeout.Infinite);

            _taskPool.CancelAllCancellableTasks();

            if (_taskPool.IsRunning)
            {
                _logger?.LogWarning(
                    "TaskBucket.TaskPool has stopped but there are tasks still running that could not be stopped - app shutdown will be postponed until these tasks have completed.");
            }
            else
            {
                _logger.LogInformation("TaskBucket.TaskPool has stopped.");
            }

            while (_taskPool.IsRunning)
            {
                await Task.Delay(50, cancellationToken);
            }
        }

        private async void StartPendingTasksAsync(object state)
        {
            if (_enabled)
            {
                await _taskPool.StartPendingTasksAsync();
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
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _logger?.LogTrace("TaskBucket.TaskQueue has been disposed.");

                _taskQueueTimer?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
