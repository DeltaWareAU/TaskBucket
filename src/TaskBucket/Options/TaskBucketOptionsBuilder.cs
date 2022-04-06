using System;
using TaskBucket.Pooling.Options;
using TaskBucket.Scheduling.Options;

namespace TaskBucket.Options
{
    internal class TaskBucketOptionsBuilder : ITaskBucketOptionsBuilder
    {
        private TimeSpan _taskQueueTrimInterval = TimeSpan.FromMinutes(2);
        private TimeSpan _taskQueueCheckingInterval = TimeSpan.FromMilliseconds(50);
        private TimeSpan _taskSchedulerCheckInterval = TimeSpan.FromMinutes(1);

        /// <inheritdoc/>
        public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Local;

        /// <inheritdoc/>
        public int WorkerThreadCount { get; set; } = Environment.ProcessorCount;

        /// <summary>
        /// Builds a new instance of <see cref="ITaskSchedulerOptions"/> using the provided configuration.
        /// </summary>
        public ITaskSchedulerOptions BuildSchedulerOptions()
        {
            TaskSchedulerOptions options = new()
            {
                TimeZone = TimeZone,
                TaskSchedulerCheckInterval = _taskSchedulerCheckInterval
            };

            return options;
        }

        /// <summary>
        /// Builds a new instance of <see cref="ITaskPoolOptions"/> using the provided configuration.
        /// </summary>
        public ITaskPoolOptions BuildTaskPoolOptions()
        {
            TaskPoolOptions options = new()
            {
                WorkerThreadCount = WorkerThreadCount,
                TaskQueueCheckingInterval = _taskQueueCheckingInterval,
                TaskQueueTrimInterval = _taskQueueTrimInterval
            };

            return options;
        }

        /// <inheritdoc/>
        public void SetTaskQueueCheckingInterval(int interval)
        {
            if (interval <= 0)
            {
                throw new ArgumentException("The interval must be greater than 0", nameof(interval));
            }

            _taskQueueCheckingInterval = TimeSpan.FromMilliseconds(interval);
        }

        /// <inheritdoc/>
        public void SetTaskQueueCheckingInterval(TimeSpan interval)
        {
            _taskQueueCheckingInterval = interval;
        }

        /// <inheritdoc/>
        public void SetTaskQueueTrimInterval(int interval)
        {
            if (interval <= 0)
            {
                throw new ArgumentException("The interval must be greater than 0", nameof(interval));
            }

            _taskQueueTrimInterval = TimeSpan.FromMilliseconds(interval);
        }

        /// <inheritdoc/>
        public void SetTaskQueueTrimInterval(TimeSpan interval)
        {
            _taskQueueTrimInterval = interval;
        }

        /// <inheritdoc/>
        public void SetTaskSchedulerCheckInterval(int interval)
        {
            if (interval <= 0)
            {
                throw new ArgumentException("The interval must be greater than 0", nameof(interval));
            }

            _taskSchedulerCheckInterval = TimeSpan.FromMilliseconds(interval);
        }

        /// <inheritdoc/>
        public void SetTaskSchedulerCheckInterval(TimeSpan interval)
        {
            _taskSchedulerCheckInterval = interval;
        }
    }
}