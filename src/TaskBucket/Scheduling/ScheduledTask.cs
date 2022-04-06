using Cronos;
using System;
using System.Threading.Tasks;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Scheduling
{
    internal class ScheduledTask<TService> : IScheduledTask
    {
        private readonly Func<TService, Task> _asyncAction;
        private readonly Action<TService> _syncAction;
        private readonly ITaskBucket _taskBucket;
        private Action<ITaskOptionsBuilder> _optionsFactory;

        public ScheduledTask(ITaskBucket taskBucket, Func<TService, Task> asyncAction, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            _taskBucket = taskBucket;
            _asyncAction = asyncAction;
            _optionsFactory = optionsFactory;
        }

        public ScheduledTask(ITaskBucket taskBucket, Action<TService> syncAction, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            _taskBucket = taskBucket;
            _syncAction = syncAction;
            _optionsFactory = optionsFactory;
        }

        public void RunAsCronJob(string cron, CronFormat format = CronFormat.Standard)
        {
            ITaskSchedule schedule = new CronSchedule(cron, format);

            _optionsFactory += builder =>
            {
                if (builder.Schedule != null)
                {
                    throw new ArgumentException("A schedule for this task has already been set.");
                }

                builder.Schedule = schedule;
            };

            if (_asyncAction != null)
            {
                _taskBucket.AddBackgroundTask(_asyncAction, _optionsFactory);
            }
            else
            {
                _taskBucket.AddBackgroundTask(_syncAction, _optionsFactory);
            }
        }
    }
}