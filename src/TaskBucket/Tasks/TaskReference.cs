using System;

namespace TaskBucket.Tasks
{
    internal class TaskReference: ITaskReference
    {
        protected DateTime StartDate { get; set; } = DateTime.MinValue;

        protected DateTime EndDate { get; set; } = DateTime.MinValue;

        public int ThreadIndex { get; protected set; }

        public TaskStatus Status { get; protected set; }

        public string Source { get; }

        public Guid Identity { get; } = Guid.NewGuid();

        public TimeSpan ExecutionTime
        {
            get
            {
                if(StartDate == DateTime.MinValue)
                {
                    return TimeSpan.Zero;
                }

                if(EndDate == DateTime.MinValue)
                {
                    return DateTime.Now - StartDate;
                }

                return EndDate - StartDate;
            }
        }

        public Exception Exception { get; protected set; }

        protected TaskReference(string source)
        {
            Source = source;
        }

        public ITaskReference GetTaskReference()
        {
            return this;
        }
    }
}
