using System.Threading;

namespace TaskBucket.Tasks.Synchronous
{
    internal interface ISynchronousTask : ITaskDetails
    {
        void RunTask(object serviceInstance, int threadIndex, CancellationToken cancellationToken);
    }

    internal interface ISynchronousTask<out TResult> : ITaskDetails<TResult>, ISynchronousTask
    {
    }
}