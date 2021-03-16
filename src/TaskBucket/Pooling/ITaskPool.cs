using System.Threading.Tasks;
using TaskBucket.Tasks;

namespace TaskBucket.Pooling
{
    internal interface ITaskPool
    {
        bool IsRunning { get; }

        void EnqueueTask(ITask task);

        Task StartPendingTasksAsync();

        void CancelAllCancellableTasks();
    }
}
