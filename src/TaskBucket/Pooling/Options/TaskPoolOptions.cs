namespace TaskBucket.Pooling.Options
{
    internal class TaskPoolOptions: ITaskPoolOptions
    {
        public int MaxConcurrentThreads { get; set; }
    }
}
