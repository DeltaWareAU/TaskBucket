using Cronos;

namespace TaskBucket.Scheduling
{
    public interface IScheduledTask
    {
        /// <summary>
        /// Sets the schedule using a cron string.
        /// </summary>
        /// <param name="cron">The cron string.</param>
        /// <param name="format">The <see cref="CronFormat"/>.</param>
        void RunAsCronJob(string cron, CronFormat format = CronFormat.Standard);
    }
}
