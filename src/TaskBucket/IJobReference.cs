using System;
using System.Diagnostics;

namespace TaskBucket
{
    public interface IJobReference
    {
        string Source { get; }

        Guid Identity { get; }

        TaskStatus Status { get; }

        Exception Exception { get; }
    }
}
