namespace TaskBucket.Pooling.Options
{
    internal interface ITaskPoolOptions
    {
        int MaxConcurrentThreads { get; }
    }
}
