using System;
using System.Collections.Generic;
using System.Text;

namespace TaskBucket
{
    public class JobReference: IJobReference
    {
        public string Source { get; }
        public int ThreadIndex { get; }
        public Guid Identity { get; }
        public TaskStatus Status { get; }
        public TimeSpan ExecutionTime { get; }
        public Exception Exception { get; }

        public JobReference(IJobReference job)
        {
            Source = job.Source;
            ThreadIndex = job.ThreadIndex;
            Identity = job.Identity;
            Status = job.Status;
            Exception = job.Exception;
            ExecutionTime = job.ExecutionTime;
        }
    }
}
