using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Pooling.Options;

namespace TaskBucket.Pooling.HostedService
{
    /// <summary>
    /// Hosts the <see cref="ITaskPool"/> as a <see cref="IHostedService"/>.
    /// </summary>
    /// <remarks>This is done to ensure that if the application is shutdown any pending tasks can either complete or be cancelled gracefully before the application can exit.</remarks>
    internal class TaskPoolHost : IHostedService, IAsyncDisposable, IDisposable
    {
        private readonly ILogger _logger;

        private readonly ITaskPoolOptions _options;
        private readonly ITaskPool _taskPool;
        private bool _isRunning;
        private Timer _taskQueueTimer;

        public TaskPoolHost(ILogger<TaskPoolHost> logger, ITaskPool taskPool, ITaskPoolOptions options)
        {
            _logger = logger;

            _taskPool = taskPool ?? throw new ArgumentNullException(nameof(taskPool));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Starts the hosted service.
        /// </summary>
        public void Start()
        {
            InternalStart();
        }

        /// <inheritdoc/>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            InternalStart();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Stops the hosted service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the stop process has been aborted.</param>
        public void Stop(CancellationToken cancellationToken)
        {
            InternalStop();

            while (_taskPool.IsRunning)
            {
                _logger?.LogWarning("There are {taskCount} tasks running - app shutdown will be postponed until these tasks have completed.", _taskPool.ExecutingTaskCount);

                Thread.Sleep(1000);
            }

            _logger?.LogInformation("Stopped.");
        }

        /// <inheritdoc/>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            InternalStop();

            while (_taskPool.IsRunning)
            {
                _logger?.LogWarning("There are {taskCount} tasks running - app shutdown will be postponed until these tasks have completed.", _taskPool.ExecutingTaskCount);

                // We don't want to cancel the Delay.
                // ReSharper disable once MethodSupportsCancellation
                await Task.Delay(1000);
            }

            _logger?.LogInformation("Stopped.");
        }

        /// <summary>
        /// Starts the hosted service.
        /// </summary>
        protected virtual void InternalStart()
        {
            if (_isRunning)
            {
                return;
            }

            _logger?.LogInformation("Starting.");

            _isRunning = true;

            _taskQueueTimer = new Timer(StartPendingTasksAsync, null, TimeSpan.Zero, _options.TaskQueueCheckingInterval);

            _logger?.LogInformation("Started.");
        }

        /// <summary>
        /// Stops the hosted service.
        /// </summary>
        protected virtual void InternalStop()
        {
            if (!_isRunning)
            {
                return;
            }

            _logger?.LogInformation("Stopping.");

            _isRunning = false;

            // Stops the scheduler from being invoked again
            _taskQueueTimer.Change(Timeout.Infinite, Timeout.Infinite);

            _taskPool.CancelAllCancellableTasks();
        }

        private async void StartPendingTasksAsync(object state)
        {
            if (_isRunning)
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

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);

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
                Stop(CancellationToken.None);

                _logger?.LogTrace("Has been disposed.");

                _taskQueueTimer?.Dispose();
            }

            _disposed = true;
        }

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                await StopAsync(CancellationToken.None);

                _logger?.LogTrace("Has been disposed.");

                if (_taskQueueTimer != null)
                {
                    await _taskQueueTimer.DisposeAsync();
                }
            }

            _disposed = true;
        }

        #endregion IDisposable
    }
}