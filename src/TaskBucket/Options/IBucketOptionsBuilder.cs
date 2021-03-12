namespace TaskBucket.Options
{
    public interface IBucketOptionsBuilder
    {
        /// <summary>
        /// How many serviceTask instances can be ran.
        /// </summary>
        int MaxBackgroundThreads { get; set; }

        /// <summary>
        /// Enables serviceTask history.
        /// </summary>
        /// <param name="historyDepth">How many jobs will be help in history.</param>
        void EnableJobHistory(int historyDepth = 1024);
    }
}
