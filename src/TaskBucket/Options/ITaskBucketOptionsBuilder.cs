using System;

namespace TaskBucket.Options
{
    public interface ITaskBucketOptionsBuilder
    {
        /// <summary>
        /// How many task instances can be ran at any given time.
        /// </summary>
        /// <remarks>By default this is set to the number of logical cores that are available for use by the common language runtime (CLR)</remarks>
        int MaxConcurrentThreads { get; set; }

        TimeZoneInfo TimeZone { get; }
    }
}
