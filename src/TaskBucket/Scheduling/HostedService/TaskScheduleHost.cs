using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Scheduling.Scheduler;

namespace TaskBucket.Scheduling.HostedService
{
    internal class TaskScheduleHost: IHostedService, IDisposable
    {
        private readonly TimeSpan _schedulerRunFrequency = TimeSpan.FromSeconds(1);

        private readonly ILogger _logger;

        private readonly ITaskScheduler _taskScheduler;

        private Timer _scheduleTimer;

        private bool _enabled;

        public TaskScheduleHost(ILogger<TaskScheduleHost> logger, Scheduler.ITaskScheduler taskScheduler)
        {
            _logger = logger;

            _taskScheduler = taskScheduler ?? throw new ArgumentNullException(nameof(taskScheduler));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if(_enabled)
            {
                return Task.CompletedTask;
            }

            _enabled = true;

            _scheduleTimer = new Timer(RunScheduler, null, TimeSpan.Zero, _schedulerRunFrequency);

            _logger.LogInformation("TaskBucket.TaskScheduler has Started");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if(!_enabled)
            {
                return Task.CompletedTask;
            }

            _enabled = false;

            // Stops the scheduler from being invoked again
            _scheduleTimer.Change(Timeout.Infinite, Timeout.Infinite);

            _logger.LogInformation("TaskBucket.TaskScheduler has stopped.");

            return Task.CompletedTask;
        }

        private void RunScheduler(object state)
        {
            if(_enabled)
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
            if(_disposed)
            {
                return;
            }

            if(disposing)
            {
                _logger?.LogTrace("TaskBucket.TaskScheduler has been disposed.");

                _scheduleTimer?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
