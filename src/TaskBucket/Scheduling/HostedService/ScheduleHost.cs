using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Scheduling.Scheduler;

namespace TaskBucket.Scheduling.HostedService
{
    public class ScheduleHost: IHostedService, IDisposable
    {
        private readonly ILogger _logger;

        private readonly IScheduler _scheduler;

        private Timer _scheduleTimer;

        private bool _enabled;

        public ScheduleHost(ILogger<ScheduleHost> logger, IScheduler scheduler)
        {
            _logger = logger;

            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if(_enabled)
            {
                return Task.CompletedTask;
            }

            _enabled = true;

            _scheduleTimer = new Timer(RunScheduler, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            _logger.LogInformation("TaskBucket Scheduler has Started");

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if(!_enabled)
            {
                return;
            }

            _enabled = false;

            _scheduleTimer.Change(Timeout.Infinite, Timeout.Infinite);

            _scheduler.CancelAllCancellableTasks();

            if(_scheduler.IsRunning)
            {
                _logger?.LogWarning("TaskBucket Scheduler has stopped but there are tasks still running that could not be stopped - app shutdown will be postponed until these tasks have completed.");
            }
            else
            {
                _logger.LogInformation("TaskBucket Scheduler has stopped.");
            }

            while(_scheduler.IsRunning)
            {
                await Task.Delay(50, cancellationToken);
            }
        }

        private void RunScheduler(object state)
        {
            if(_enabled)
            {
                _scheduler.RunScheduler();
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

                _scheduleTimer?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
