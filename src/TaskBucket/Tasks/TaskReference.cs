using TaskBucket.Abstractions.Tasks;

namespace TaskBucket.Tasks
{
    internal abstract class TaskReference : ITaskReference
    {
        public Exception? Exception { get; protected set; }

        public TimeSpan ExecutionTime { get; protected set; }

        public Guid Identity { get; protected set; }

        public TaskState State { get; protected set; }

        public int BucketIndex { get; protected set; }

        public abstract TaskReference Copy();
    }
}
