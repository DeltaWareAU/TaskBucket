namespace TaskBucket.Abstractions.Tasks
{
    public interface IBackgroundTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
