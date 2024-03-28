using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Abstractions.Tasks;
using TaskBucket.Abstractions.Tasks.Options;

namespace TaskBucket.Execution.Tasks
{
    internal sealed class BackgroundTaskExecutor<T>(ITaskOptions options) : TaskExecutor<T>(options) where T : class, IBackgroundTask
    {
        public override Task ExecuteAsync(object executorInstance, int bucketIndex, CancellationToken cancellationToken)
            => InternalExecuteTaskAsync((t, c) => t.ExecuteAsync(c), executorInstance, bucketIndex, cancellationToken);
    }
}
