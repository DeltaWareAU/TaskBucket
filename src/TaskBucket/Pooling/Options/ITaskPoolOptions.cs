using System;

namespace TaskBucket.Pooling.Options
{
    internal interface ITaskPoolOptions
    {
        /// <summary>
        /// Specifies how often the Task Queue is checked for Pending Tasks.
        /// </summary>
        TimeSpan TaskQueueCheckingInterval { get; }

        /// <summary>
        /// Specifies how often the Task Queue is trimmed.
        /// </summary>
        TimeSpan TaskQueueTrimInterval { get; }

        /// <summary>
        /// Specifies how many worker threads are available.
        /// </summary>
        int WorkerThreadCount { get; }
    }
}