using System;

namespace TaskBucket
{
    public interface IJobReference
    {
        Guid Identity { get; }

        TaskStatus Status { get; }

        Exception Exception { get; }
    }
}
