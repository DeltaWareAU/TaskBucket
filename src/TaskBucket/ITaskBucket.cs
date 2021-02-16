using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskBucket
{
    public interface ITaskBucket
    {
        List<IJobReference> Tasks { get; }

        IJobReference AddBackgroundJob<T>(Func<T, Task> action);
        List<IJobReference> AddBackgroundJobs<T, TValue>(IEnumerable<TValue> values, Func<T, TValue, Task> action);
    }
}
