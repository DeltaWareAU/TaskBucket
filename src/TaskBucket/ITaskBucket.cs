using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskBucket
{
    public interface ITaskBucket
    {
        IReadOnlyList<IJobReference> JobHistory { get; }

        IReadOnlyList<IJobReference> Jobs { get; }

        void ClearJobHistory();

        IJobReference AddBackgroundJob<T>(Func<T, Task> action);
        List<IJobReference> AddBackgroundJobs<T, TValue>(IEnumerable<TValue> values, Func<T, TValue, Task> action);
    }
}
