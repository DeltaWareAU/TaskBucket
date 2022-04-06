using System;
using TaskBucket.Tasks;

namespace TaskBucket.Scheduling
{
    /// <summary>
    /// Defines the Schedule for a <see cref="ITask{TResult}"/>.
    /// </summary>
    public interface ITaskSchedule
    {
        /// <summary>
        /// Gets the <see cref="DateTime"/> for the next scheduled execution.
        /// </summary>
        /// <param name="utcTime">The current <see cref="DateTime"/>.</param>
        /// <param name="timeZone">The <see cref="TimeZoneInfo"/>.</param>
        /// <returns></returns>
        DateTime? GetNextSchedule(DateTime utcTime, TimeZoneInfo timeZone);
    }
}
