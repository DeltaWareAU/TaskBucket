namespace TaskBucket.Options
{
    internal class BucketOptionsBuilder: IBucketOptionsBuilder
    {
        private bool _jobHistoryEnabled;

        private int _jobHistoryDepth = 1024;

        public int MaxBackgroundThreads { get; set; } = 10;

        public void EnableJobHistory(int historyDepth = 1024)
        {
            _jobHistoryEnabled = true;
            _jobHistoryDepth = historyDepth;
        }

        public IBucketOptions BuildOptions()
        {
            BucketOptions options = new BucketOptions
            {
                JobHistoryDepth = _jobHistoryDepth,
                JobHistoryEnabled = _jobHistoryEnabled,
                MaxBackgroundThreads = MaxBackgroundThreads
            };

            return options;
        }
    }
}
