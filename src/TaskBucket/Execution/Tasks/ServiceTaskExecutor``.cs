using System;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Abstractions.Tasks.Options;

namespace TaskBucket.Execution.Tasks
{
    internal sealed class ServiceTaskExecutor<T, TResult>(Func<T, CancellationToken, Task<TResult>> taskFunc, ITaskOptions options) : TaskExecutor<T, TResult>(options) where T : class
    {
        public override Task ExecuteAsync(object executorInstance, int bucketIndex, CancellationToken cancellationToken)
            => InternalRunTaskAsync(taskFunc.Invoke, executorInstance, bucketIndex, cancellationToken);
    }
}
