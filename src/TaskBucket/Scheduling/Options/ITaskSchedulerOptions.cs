using System;

namespace TaskBucket.Scheduling.Options
{
    internal interface ITaskSchedulerOptions
    {
        TimeZoneInfo TimeZone { get; }
    }
}
