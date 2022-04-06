using System;

namespace TaskBucket.Options
{
    public interface ITaskBucketOptionsBuilder
    {
        /// <summary>
        /// Specifies the timezone.
        /// </summary>
        /// <remarks>Used for Task Scheduling.</remarks>
        TimeZoneInfo TimeZone { get; set; }

        /// <summary>
        /// How many task instances can be executed at any given time.
        /// </summary>
        /// <remarks>
        /// By default this is set to the number of logical cores that are available for use by the
        /// common language runtime (CLR). Exceeding this limit may cause a degradation in performance.
        /// </remarks>
        int WorkerThreadCount { get; set; }

        /// <summary>
        /// Sets the interval in milliseconds for how often the Task Queue is checked for Pending Tasks.
        /// </summary>
        /// <param name="interval">The amount of time in milliseconds.</param>
        /// <remarks><see langword="default"/> - 50 Milliseconds.</remarks>
        void SetTaskQueueCheckingInterval(int interval);

        /// <summary>
        /// Sets the interval in milliseconds for how often the Task Queue is checked for Pending Tasks.
        /// </summary>
        /// <param name="interval">The amount of time.</param>
        /// <remarks><see langword="default"/> - 50 Milliseconds.</remarks>
        void SetTaskQueueCheckingInterval(TimeSpan interval);

        /// <summary>
        /// Sets the interval in milliseconds for how often the Task Queue is trimmed.
        /// </summary>
        /// <param name="interval">The amount of time in milliseconds.</param>
        /// <remarks><see langword="default"/> - 2 Minutes.</remarks>
        void SetTaskQueueTrimInterval(int interval);

        /// <summary>
        /// Sets the interval in milliseconds for how often the Task Queue is trimmed.
        /// </summary>
        /// <param name="interval">The amount of time.</param>
        /// <remarks><see langword="default"/> - 2 Minutes.</remarks>
        void SetTaskQueueTrimInterval(TimeSpan interval);

        /// <summary>
        /// Sets the interval in milliseconds for how often the Task Scheduler check for pending tasks.
        /// </summary>
        /// <param name="interval">The amount of time in milliseconds.</param>
        /// <remarks><see langword="default"/> - 1 Minute.</remarks>
        void SetTaskSchedulerCheckInterval(int interval);

        /// <summary>
        /// Sets the interval in milliseconds for how often the Task Scheduler check for pending tasks.
        /// </summary>
        /// <param name="interval">The amount of time.</param>
        /// <remarks><see langword="default"/> - 1 Minute.</remarks>
        void SetTaskSchedulerCheckInterval(TimeSpan interval);
    }
}