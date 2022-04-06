using Cronos;
using System;

namespace TaskBucket.Scheduling
{
    internal class CronSchedule : ITaskSchedule
    {
        private readonly CronExpression _cronSchedule;

        public CronSchedule(string cron, CronFormat format = CronFormat.Standard)
        {
            _cronSchedule = CronExpression.Parse(cron, format);
        }

        public DateTime? GetNextSchedule(DateTime utcTime, TimeZoneInfo timeZone)
        {
            return _cronSchedule.GetNextOccurrence(utcTime, timeZone);
        }
    }
}
