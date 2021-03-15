namespace TaskBucket.Scheduling.Options
{
    internal interface ISchedulerOptions
    {
        int MaxConcurrentThreads { get; }
    }
}
