namespace TaskBucket
{
    public interface IBucketOptions
    {
        /// <summary>
        /// How many task instances can be ran.
        /// </summary>
        int Instances { get; set; }
    }
}
