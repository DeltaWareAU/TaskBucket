using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Scheduling.Options;
using TaskBucket.Scheduling.Scheduler;

namespace TaskBucket.Scheduling.HostedService
{
    internal class TaskScheduleHost : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly ITaskScheduler _taskScheduler;
        private readonly ITaskSchedulerOptions _options;

        private bool _enabled;
        private Timer _scheduleTimer;

        public TaskScheduleHost(ILogger<TaskScheduleHost> logger, ITaskScheduler taskScheduler, ITaskSchedulerOptions options)
        {
            _logger = logger;

            _taskScheduler = taskScheduler ?? throw new ArgumentNullException(nameof(taskScheduler));

            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_enabled)
            {
                return Task.CompletedTask;
            }

            _enabled = true;

            _scheduleTimer = new Timer(RunScheduler, null, TimeSpan.Zero, _options.TaskSchedulerCheckInterval);

            _logger?.LogInformation("Has Started");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (!_enabled)
            {
                return Task.CompletedTask;
            }

            _enabled = false;

            // Stops the scheduler from being invoked again
            _scheduleTimer.Change(Timeout.Infinite, Timeout.Infinite);

            _logger?.LogInformation("Has stopped.");

            return Task.CompletedTask;
        }

        private void RunScheduler(object state)
        {
            if (_enabled)
            {
                _taskScheduler.RunScheduler();
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
                _logger?.LogTrace("Has been disposed.");

                _scheduleTimer?.Dispose();
            }

            _disposed = true;
        }

        #endregion IDisposable
    }
}