using TaskBucket.Abstractions.Tasks;

namespace TaskBucket.Execution.Tasks
{
    internal interface ITaskExecutor<out TResult> : ITaskExecutor, ITaskReference<TResult>
    {
    }
}
