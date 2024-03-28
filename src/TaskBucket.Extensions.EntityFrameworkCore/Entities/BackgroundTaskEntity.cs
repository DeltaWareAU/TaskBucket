namespace TaskBucket.Extensions.EntityFrameworkCore.Entities
{
    public sealed class BackgroundTaskEntity
    {
        public Guid Id { get; set; }

        public bool Enabled { get; set; }

        public string CronSchedule { get; set; } = null!;

        public string AssemblyName { get; set; } = null!;

        public string TypeName { get; set; } = null!;

        public DateTime CreatedDateUtc { get; set; }

        public DateTime ModifiedDateUtc { get; set; }

        public ICollection<BackgroundTaskExecutionHistoryEntity> ExecutionHistory { get; set; } = null!;
    }
}
