using Cronos;
using System;
using System.Threading.Tasks;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Scheduling
{
    internal class TaskScheduling<TDefinition> : ITaskScheduling
    {
        private readonly ITaskBucket _taskBucket;

        private readonly Func<TDefinition, Task> _action;

        private Action<ITaskOptionsBuilder> _optionsFactory;

        public TaskScheduling(ITaskBucket taskBucket, Func<TDefinition, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            _taskBucket = taskBucket;
            _action = action;
            _optionsFactory = optionsFactory;
        }

        public void RunHourly(int every = 1)
        {
            throw new NotImplementedException();
        }

        public void RunDaily(int every = 1)
        {
            throw new NotImplementedException();
        }

        public void RunAsCronJob(string cron, CronFormat format = CronFormat.Standard)
        {
            ITaskSchedule schedule = new CronSchedule(cron, format);

            _optionsFactory += builder => builder.Schedule = schedule;

            _taskBucket.AddBackgroundTask(_action, _optionsFactory);
        }
    }
}
