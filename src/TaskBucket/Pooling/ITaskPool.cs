using TaskBucket.Tasks;

namespace TaskBucket.Pooling
{
    internal interface ITaskPool
    {
        bool IsRunning { get; }

        void EnqueueTask(ITask task);

        void StartPendingTasks();

        void CancelAllCancellableTasks();
    }
}
