using System;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Abstractions.Tasks;

namespace TaskBucket.Execution.Tasks
{
    internal interface ITaskExecutor : ITaskReference
    {
        /// <summary>
        /// Specifies the type required for the Task to run.
        /// </summary>
        Type ExecutorType { get; }

        Task ExecuteAsync(object executorInstance, int bucketIndex, CancellationToken cancellationToken);
    }
}
