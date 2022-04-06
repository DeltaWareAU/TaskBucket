using System;

namespace TaskBucket.Pooling.Options
{
    internal class TaskPoolOptions : ITaskPoolOptions
    {
        /// <inheritdoc/>
        public TimeSpan TaskQueueCheckingInterval { get; set; }

        /// <inheritdoc/>
        public TimeSpan TaskQueueTrimInterval { get; set; }

        /// <inheritdoc/>
        public int WorkerThreadCount { get; set; }
    }
}