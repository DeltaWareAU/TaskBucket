namespace TaskBucket.Options
{
    interface IBucketOptions
    {
        bool JobHistoryEnabled { get; }

        int JobHistoryDepth { get; }

        int MaxBackgroundThreads { get; }
    }
}
