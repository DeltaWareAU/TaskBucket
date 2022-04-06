using System.Threading;
using System.Threading.Tasks;

namespace TaskBucket.Tasks.Asynchronous
{
    internal interface IAsynchronousTask : ITaskDetails
    {
        Task RunTaskAsync(object serviceInstance, int threadIndex, CancellationToken cancellationToken);
    }

    internal interface IAsynchronousTask<out TResult> : ITaskDetails<TResult>, IAsynchronousTask
    {
    }
}