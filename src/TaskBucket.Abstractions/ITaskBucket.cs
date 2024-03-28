using TaskBucket.Abstractions.Tasks;
using TaskBucket.Abstractions.Tasks.Options;

namespace TaskBucket.Abstractions
{
    public interface ITaskBucket
    {
        ITaskReference AddBackgroundTask<T>(Action<ITaskOptionsBuilder>? optionsBuilder = null) where T : class, IBackgroundTask;

        ITaskReference AddBackgroundTask<TService>(Func<TService, CancellationToken, Task> action, Action<ITaskOptionsBuilder>? optionsBuilder = null) where TService : class;

        ITaskReference<TResult> AddBackgroundTask<TService, TResult>(Func<TService, CancellationToken, Task<TResult>> action, Action<ITaskOptionsBuilder>? optionsBuilder = null) where TService : class;
    }
}
