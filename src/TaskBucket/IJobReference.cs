using System;

namespace TaskBucket
{
    public interface IJobReference
    {
        string Source { get; }

        int ThreadIndex { get; }

        Guid Identity { get; }

        TaskStatus Status { get; }

        TimeSpan ExecutionTime { get; }

        Exception Exception { get; }
    }
}
