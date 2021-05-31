using System;
using TaskBucket.Scheduling;
using TaskBucket.Tasks.Enums;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Tasks
{
    internal abstract class TaskReference : ITaskReference
    {
        protected DateTime StartDate { get; set; } = DateTime.MinValue;

        protected DateTime EndDate { get; set; } = DateTime.MinValue;

        public int ThreadIndex { get; protected set; } = -1;

        public abstract bool IsCancelable { get; }

        public TaskStatus Status { get; protected set; }

        public ITaskSchedule Schedule { get; set; }

        public ITaskOptions Options { get; }

        public string Source { get; }

        public Guid Identity { get; } = Guid.NewGuid();

        public TimeSpan ExecutionTime
        {
            get
            {
                if (StartDate == DateTime.MinValue)
                {
                    return TimeSpan.Zero;
                }

                if (EndDate == DateTime.MinValue)
                {
                    return DateTime.Now - StartDate;
                }

                return EndDate - StartDate;
            }
        }

        public Exception Exception { get; protected set; }

        protected TaskReference(string source, ITaskOptions options)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            Source = source;
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public ITaskReference GetTaskReference()
        {
            return this;
        }
    }
}
