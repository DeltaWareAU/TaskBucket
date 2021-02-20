namespace TaskBucket
{
    internal class BucketOptions: IBucketOptions
    {
        public int MaxBackgrounThreads { get; set; } = 10;
    }
}
