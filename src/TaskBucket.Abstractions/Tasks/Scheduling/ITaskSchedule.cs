namespace TaskBucket.Abstractions.Tasks.Scheduling
{
    /// <summary>
    /// Defines the Schedule for a <see cref="IBackgroundTaskReference"/>.
    /// </summary>
    public interface ITaskSchedule
    {
        /// <summary>
        /// Gets the <see cref="DateTime"/> for the next scheduled execution.
        /// </summary>
        /// <param name="utcTime">The current <see cref="DateTime"/>.</param>
        /// <param name="timeZone">The <see cref="TimeZoneInfo"/>.</param>
        /// <returns>The next Execution Date</returns>
        DateTime? GetNextExecutionDateUtc(DateTime utcTime, TimeZoneInfo timeZone);
    }
}
