using System;
using TaskBucket.Scheduling.Options;

namespace TaskBucket.Options
{
    internal class TaskBucketOptionsBuilder: ITaskBucketOptionsBuilder
    {
        public int MaxConcurrentThreads { get; set; } = Environment.ProcessorCount;

        public IBucketOptions BuildBucketOptions()
        {
            BucketOptions options = new BucketOptions();

            return options;
        }

        public ISchedulerOptions BuildSchedulerOptions()
        {
            SchedulerOptions options = new SchedulerOptions
            {
                MaxConcurrentThreads = MaxConcurrentThreads
            };

            return options;
        }
    }
}
