using System.Threading.Tasks;
using TaskBucket.Tasks;

namespace TaskBucket.Pooling
{
    internal interface ITaskPool
    {
        /// <summary>
        /// Specifies if the <see cref="ITaskPool"/> is currently running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Specifies how many <see cref="ITask"/>s are currently executing.
        /// </summary>
        int ExecutingTaskCount { get; }

        /// <summary>
        /// Cancels all <see cref="ITask"/>s that implement <see cref="ICancellableTask"/>.
        /// </summary>
        void CancelAllCancellableTasks();

        /// <summary>
        /// Adds the provided <see cref="ITask"/> to the pool.
        /// </summary>
        /// <param name="task">The <see cref="ITask"/> to be added to the pool.</param>
        void EnqueueTask(ITaskDetails task);

        /// <summary>
        /// Starts any pending <see cref="ITask"/>s.
        /// </summary>
        Task StartPendingTasksAsync();
    }
}