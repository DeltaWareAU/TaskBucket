using System.Collections.Generic;
using TaskBucket.Tasks;

namespace TaskBucket
{
    public interface ITaskBucketStatus
    {
        IReadOnlyList<ITaskReference> TaskHistory { get; }

        IReadOnlyList<ITaskReference> PendingTasks { get; }

        IReadOnlyList<ITaskReference> RunningTasks { get; }

        void ClearTaskHistory();
    }
}
