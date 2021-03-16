using System;
using TaskBucket.Scheduling;
using TaskBucket.Tasks.Enums;

namespace TaskBucket.Tasks.Options
{
    public interface ITaskOptions
    {
        /// <summary>
        /// Specifies the Priority of the Task
        /// </summary>
        TaskPriority Priority { get; }

        ITaskSchedule Schedule { get; }

        Action<ITaskReference> OnTaskFinished { get; }
    }
}
