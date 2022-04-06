using System;

namespace TaskBucket.Scheduling.Options
{
    internal interface ITaskSchedulerOptions
    {
        /// <summary>
        /// Specifies the timezone.
        /// </summary>
        TimeZoneInfo TimeZone { get; }

        /// <summary>
        /// Specifies the interval to which the Task Scheduler will check for pending tasks.
        /// </summary>
        TimeSpan TaskSchedulerCheckInterval { get; }
    }
}
