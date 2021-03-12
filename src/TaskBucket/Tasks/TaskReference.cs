using System;

namespace TaskBucket.Tasks
{
    internal class TaskReference: ITaskReference
    {
        public string Source { get; protected set; }
        public int ThreadIndex { get; protected set; }
        public Guid Identity { get; } = Guid.NewGuid();
        public TaskStatus Status { get; protected set; }
        public TimeSpan ExecutionTime { get; protected set; }
        public Exception Exception { get; protected set; }

        public ITaskReference GetTaskReference()
        {
            return this;
        }
    }
}
