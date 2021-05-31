using System;

namespace TaskBucket.Pooling.Options
{
    internal class TaskPoolOptions : ITaskPoolOptions
    {
        public int MaxConcurrentThreads { get; set; }
        public TimeSpan CheckPendingTasksFrequency { get; set; }
    }
}
