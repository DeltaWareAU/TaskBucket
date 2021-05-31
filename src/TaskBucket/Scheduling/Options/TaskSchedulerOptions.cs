using System;

namespace TaskBucket.Scheduling.Options
{
    internal class TaskSchedulerOptions : ITaskSchedulerOptions
    {
        public TimeZoneInfo TimeZone { get; set; }
    }
}
