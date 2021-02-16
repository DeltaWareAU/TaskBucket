using System;

namespace TaskBucket
{
    public interface IJobReference
    {
        string Source { get; }

        int ThreadIndex { get; }

        Guid Identity { get; }

        TaskStatus Status { get; }

        Exception Exception { get; }
    }
}
