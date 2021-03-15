using System;

namespace TaskBucket.Scheduling.Options
{
    internal interface ISchedulerOptions
    {
        TimeZoneInfo TimeZone { get; }
    }
}
