using System;
using TaskBucket.Tasks.Enums;

namespace TaskBucket.Tasks.Options
{
    internal class TaskOptions: ITaskOptions
    {
        public TaskPriority Priority { get; set; } = TaskPriority.Normal;

        public Action<ITaskReference> OnTaskFinished { get; set; }
    }
}
