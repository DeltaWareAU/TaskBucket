namespace TaskBucket.Options
{
    internal interface IBucketOptions
    {
        bool JobHistoryEnabled { get; }

        int JobHistoryDepth { get; }

        int MaxBackgroundThreads { get; }
    }
}
