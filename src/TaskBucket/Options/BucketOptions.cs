namespace TaskBucket.Options
{
    internal class BucketOptions: IBucketOptions
    {
        public bool JobHistoryEnabled { get; set; }
        public int JobHistoryDepth { get; set; }
        public int MaxBackgroundThreads { get; set; }
    }
}
