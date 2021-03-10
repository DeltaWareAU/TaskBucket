using System;

namespace TaskBucket.Jobs
{
    public class JobReference: IJobReference
    {
        public string Source { get; protected set; }
        public int ThreadIndex { get; protected set; }
        public Guid Identity { get; } = Guid.NewGuid();
        public TaskStatus Status { get; protected set; }
        public TimeSpan ExecutionTime { get; protected set; }
        public Exception Exception { get; protected set; }

        public IJobReference GetJobReference()
        {
            return this;
        }
    }
}
