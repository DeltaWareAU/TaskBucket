using System;
using TaskBucket.Pooling.Options;
using TaskBucket.Scheduling.Options;

namespace TaskBucket.Options
{
    internal class TaskBucketOptionsBuilder: ITaskBucketOptionsBuilder
    {
        public int MaxConcurrentThreads { get; set; } = Environment.ProcessorCount;

        public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Local;

        public IBucketOptions BuildBucketOptions()
        {
            BucketOptions options = new BucketOptions();

            return options;
        }

        public ISchedulerOptions BuildSchedulerOptions()
        {
            SchedulerOptions options = new SchedulerOptions
            {
                TimeZone = TimeZone
            };

            return options;
        }

        public ITaskPoolOptions BuildTaskPoolOptions()
        {
            TaskPoolOptions options = new TaskPoolOptions
            {
                MaxConcurrentThreads = MaxConcurrentThreads
            };

            return options;
        }
    }
}
