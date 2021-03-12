using System.Collections.Generic;
using TaskBucket.Tasks;

namespace TaskBucket
{
    public interface ITaskBucketStatus
    {
        /// <summary>
        /// Contains previously ran tasks
        /// </summary>
        IReadOnlyList<ITaskReference> TaskHistory { get; }

        /// <summary>
        /// Contains all pending tasks
        /// </summary>
        IReadOnlyList<ITaskReference> PendingTasks { get; }

        /// <summary>
        /// Contains all running tasks
        /// </summary>
        IReadOnlyList<ITaskReference> RunningTasks { get; }

        /// <summary>
        /// Clears task history
        /// </summary>
        void ClearTaskHistory();
    }
}
