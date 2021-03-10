using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBucket.Jobs;

namespace TaskBucket
{
    public interface ITaskBucket
    {
        IReadOnlyList<IJobReference> JobHistory { get; }

        IReadOnlyList<IJobReference> Jobs { get; }

        void ClearJobHistory();

        IJobReference AddBackgroundJob<TInstance>(Func<TInstance, Task> action);
        IJobReference AddBackgroundJob<TInstance>(Func<TInstance, IJobReference, Task> action);
        IJobReference AddBackgroundJob<TInstance>(TInstance instance, Func<TInstance, Task> action);
        IJobReference AddBackgroundJob<TInstance>(TInstance instance, Func<TInstance, IJobReference, Task> action);
        List<IJobReference> AddBackgroundJobs<TInstance, TValue>(IEnumerable<TValue> values, Func<TInstance, TValue, Task> action);
        List<IJobReference> AddBackgroundJobs<TInstance, TValue>(IEnumerable<TValue> values, Func<TInstance, TValue, IJobReference, Task> action);
    }
}
