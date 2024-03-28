using System;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Abstractions.Tasks.Options;

namespace TaskBucket.Execution.Tasks
{
    internal sealed class ServiceTaskExecutor<T>(Func<T, CancellationToken, Task> taskFunc, ITaskOptions options) : TaskExecutor<T>(options) where T : class
    {
        public override Task ExecuteAsync(object executorInstance, int bucketIndex, CancellationToken cancellationToken)
            => InternalExecuteTaskAsync(taskFunc.Invoke, executorInstance, bucketIndex, cancellationToken);
    }
}
