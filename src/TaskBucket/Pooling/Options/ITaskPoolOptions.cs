using System;

namespace TaskBucket.Pooling.Options
{
    internal interface ITaskPoolOptions
    {
        int MaxConcurrentThreads { get; }

        TimeSpan CheckPendingTasksFrequency { get; }
    }
}
