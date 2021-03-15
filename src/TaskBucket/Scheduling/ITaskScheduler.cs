using System;

namespace TaskBucket.Scheduling
{
    public interface ITaskScheduler
    {
        DateTime? GetNextSchedule(DateTime utcTime, TimeZoneInfo timeZone);
    }
}
