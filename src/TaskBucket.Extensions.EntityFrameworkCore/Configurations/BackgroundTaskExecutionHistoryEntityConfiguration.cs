using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskBucket.Extensions.EntityFrameworkCore.Entities;

namespace TaskBucket.Extensions.EntityFrameworkCore.Configurations
{
    internal sealed class BackgroundTaskExecutionHistoryEntityConfiguration : IEntityTypeConfiguration<BackgroundTaskExecutionHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<BackgroundTaskExecutionHistoryEntity> builder)
        {
            throw new NotImplementedException();
        }
    }
}
