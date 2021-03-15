using System;

namespace TaskBucket.Scheduling.Options
{
    internal class SchedulerOptions: ISchedulerOptions
    {
        public TimeZoneInfo TimeZone { get; set; }
    }
}
