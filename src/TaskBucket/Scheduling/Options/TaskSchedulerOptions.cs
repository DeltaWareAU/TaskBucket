using System;

namespace TaskBucket.Scheduling.Options
{
    internal class TaskSchedulerOptions : ITaskSchedulerOptions
    {
        /// <inheritdoc/>
        public TimeZoneInfo TimeZone { get; set; }

        public TimeSpan TaskSchedulerCheckInterval { get; set; }
    }
}
