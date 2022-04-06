using System;
using TaskBucket.Scheduling;
using TaskBucket.Tasks.Enums;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Tasks
{
    internal abstract class TaskDetails : ITaskDetails
    {
        public Exception Exception { get; protected set; }
        public TimeSpan ExecutionTime { get; protected set; }
        public Guid Identity { get; } = Guid.NewGuid();
        public bool IsCancelable { get; protected set; }
        public ITaskOptions Options { get; }
        public ITask PreviouslyRanInstance { get; protected set; }
        public ITaskSchedule Schedule { get; set; }
        public Type ServiceType { get; protected set; }
        public TaskState State { get; protected set; }
        public int ThreadIndex { get; protected set; } = -1;

        protected TaskDetails(ITaskOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public abstract ITaskDetails Copy();
    }

    internal abstract class TaskDetails<TResult> : TaskDetails, ITaskDetails<TResult>
    {
        public TResult Result { get; protected set; }

        protected TaskDetails(ITaskOptions options) : base(options)
        {
        }
    }
}