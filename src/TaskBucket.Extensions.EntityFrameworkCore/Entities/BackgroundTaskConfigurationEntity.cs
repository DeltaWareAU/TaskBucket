namespace TaskBucket.Extensions.EntityFrameworkCore.Entities
{
    public sealed class BackgroundTaskConfigurationEntity
    {
        public Guid Id { get; set; }

        public Guid BackgroundTaskId { get; set; }

        public BackgroundTaskEntity BackgroundTask { get; set; } = null!;

        public string AssemblyName { get; set; } = null!;

        public string TypeName { get; set; } = null!;

        public string ConfigurationJson { get; set; } = null!;
    }
}
