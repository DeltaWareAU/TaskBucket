using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Abstractions.Tasks;
using TaskBucket.Abstractions.Tasks.Options;

namespace TaskBucket.Execution.Tasks
{
    internal abstract class TaskExecutor<TExecutor>(ITaskOptions options) : ITaskExecutor where TExecutor : class
    {
        public ITaskOptions Options { get; } = options ?? throw new ArgumentNullException(nameof(options));

        public Type ExecutorType { get; } = typeof(TExecutor);

        public Guid Identity { get; } = Guid.NewGuid();

        public TaskState State { get; private set; } = TaskState.Pending;

        public int BucketIndex { get; private set; } = -1;

        public Exception? Exception { get; private set; }

        public TimeSpan? ExecutionTime { get; private set; }

        public abstract Task ExecuteAsync(object executorInstance, int bucketIndex, CancellationToken cancellationToken);

        protected async Task InternalExecuteTaskAsync(Func<TExecutor, CancellationToken, Task> taskExecutor, object executorInstance, int bucketIndex, CancellationToken cancellationToken)
        {
            BucketIndex = bucketIndex;

            if (State != TaskState.Pending)
            {
                throw new InvalidOperationException("A task cannot be started unless it is pending");
            }

            if (executorInstance is not TExecutor instance)
            {
                throw new ArgumentException($"An invalid Service Instance was provided, the provided type is {executorInstance.GetType().Name} but it should be {ExecutorType.Name}");
            }

            State = TaskState.Running;

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                await taskExecutor.Invoke(instance, cancellationToken);

                State = TaskState.Completed;
            }
            catch (Exception e)
            {
                State = TaskState.Failed;

                Exception = e;

                // We want to throw this error so logging can pick it up.
                throw;
            }
            finally
            {
                stopwatch.Stop();

                ExecutionTime = stopwatch.Elapsed;

                Options.OnTaskFinished?.Invoke(this);
            }
        }
    }
}
