using System;

namespace TaskBucket.Tasks
{
    public interface ITaskReference
    {
        string Source { get; }

        int ThreadIndex { get; }

        Guid Identity { get; }

        TaskStatus Status { get; }

        TimeSpan ExecutionTime { get; }

        Exception Exception { get; }

        ITaskReference GetTaskReference();
    }
}
