using System;

namespace TaskBucket.Scheduling
{
    public interface ITaskSchedule
    {
        DateTime? GetNextSchedule(DateTime utcTime, TimeZoneInfo timeZone);
    }
}
