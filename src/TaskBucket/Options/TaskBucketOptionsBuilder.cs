using System;
using TaskBucket.Pooling.Options;
using TaskBucket.Scheduling.Options;

namespace TaskBucket.Options
{
    internal class TaskBucketOptionsBuilder: ITaskBucketOptionsBuilder
    {
        private TimeSpan _checkPendingTasksFrequency = TimeSpan.FromSeconds(1);

        public int MaxConcurrentThreads { get; set; } = Environment.ProcessorCount;

        public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Local;

        public void SetTaskCheckingFrequency(int milliseconds)
        {
            _checkPendingTasksFrequency = TimeSpan.FromMilliseconds(milliseconds);
        }

        public void SetTaskCheckingFrequency(TimeSpan time)
        {
            _checkPendingTasksFrequency = time;
        }

        public ITaskBucketOptions BuildBucketOptions()
        {
            TaskBucketOptions options = new TaskBucketOptions();

            return options;
        }

        public ITaskSchedulerOptions BuildSchedulerOptions()
        {
            TaskSchedulerOptions options = new TaskSchedulerOptions
            {
                TimeZone = TimeZone
            };

            return options;
        }

        public ITaskPoolOptions BuildTaskPoolOptions()
        {
            TaskPoolOptions options = new TaskPoolOptions
            {
                MaxConcurrentThreads = MaxConcurrentThreads,
                CheckPendingTasksFrequency = _checkPendingTasksFrequency
            };

            return options;
        }
    }
}
