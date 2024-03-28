using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskBucket.Extensions.EntityFrameworkCore.Entities;

namespace TaskBucket.Extensions.EntityFrameworkCore.Configurations
{
    internal sealed class BackgroundTaskEntityConfiguration : IEntityTypeConfiguration<BackgroundTaskEntity>
    {
        public void Configure(EntityTypeBuilder<BackgroundTaskEntity> builder)
        {
            throw new NotImplementedException();
        }
    }
}
