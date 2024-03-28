namespace TaskBucket.Extensions.EntityFrameworkCore.Entities
{
    public sealed class BackgroundTaskExecutionHistoryEntity
    {
        public Guid Id { get; set; }

        public Guid BackgroundTaskId { get; set; }

        public BackgroundTaskEntity BackgroundTask { get; set; } = null!;

        public bool WasSuccessful { get; set; }

        public int BucketIndex { get; set; }

        public DateTime LastExecutionDateUtc { get; set; }

        public TimeSpan ExecutionTime { get; set; }
    }
}
