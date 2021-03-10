namespace TaskBucket
{
    internal class BucketOptions: IBucketOptions
    {
        public int MaxBackgroundThreads { get; set; } = 10;
    }
}
