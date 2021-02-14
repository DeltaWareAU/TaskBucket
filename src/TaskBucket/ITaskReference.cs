using System;

namespace TaskBucket
{
    public interface ITaskReference
    {
        Guid Identity { get; }

        TaskStatus Status { get; }

        Exception Exception { get; }
    }
}
