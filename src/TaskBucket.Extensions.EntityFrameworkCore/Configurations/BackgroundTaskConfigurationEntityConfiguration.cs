using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskBucket.Extensions.EntityFrameworkCore.Entities;

namespace TaskBucket.Extensions.EntityFrameworkCore.Configurations
{
    internal sealed class BackgroundTaskConfigurationEntityConfiguration : IEntityTypeConfiguration<BackgroundTaskConfigurationEntity>
    {
        public void Configure(EntityTypeBuilder<BackgroundTaskConfigurationEntity> builder)
        {
            throw new NotImplementedException();
        }
    }
}
