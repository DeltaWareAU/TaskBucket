using System;
using TaskBucket.Scheduling;
using TaskBucket.Tasks.Enums;

namespace TaskBucket.Tasks.Options
{
    internal class TaskOptions: ITaskOptions
    {
        public TaskPriority Priority { get; set; } = TaskPriority.Normal;

        public ITaskScheduler Scheduler { get; set; }

        public Action<ITaskReference> OnTaskFinished { get; set; }
    }
}
