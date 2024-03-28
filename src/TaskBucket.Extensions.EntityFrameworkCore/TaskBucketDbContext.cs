using Microsoft.EntityFrameworkCore;
using TaskBucket.Extensions.EntityFrameworkCore.Entities;

namespace TaskBucket.Extensions.EntityFrameworkCore
{
    public sealed class TaskBucketDbContext : DbContext
    {
        public DbSet<BackgroundTaskEntity> Tasks { get; set; } = null!;

        public DbSet<BackgroundTaskConfigurationEntity> TaskConfigurations { get; set; } = null!;

        public DbSet<BackgroundTaskExecutionHistoryEntity> TaskExecutionHistories { get; set; } = null!;

        public TaskBucketDbContext(DbContextOptions<TaskBucketDbContext> options) : base(options)
        {
        }
    }
}
