namespace TaskBucket.Scheduling.Options
{
    internal class SchedulerOptions: ISchedulerOptions
    {
        public int MaxConcurrentThreads { get; set; }
    }
}
