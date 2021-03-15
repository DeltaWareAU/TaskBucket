using System;
using TaskBucket.Tasks.Enums;

namespace TaskBucket.Tasks.Options
{
    public interface ITaskOptions
    {
        /// <summary>
        /// Specifies the Priority of the Task
        /// </summary>
        TaskPriority Priority { get; }

        Action<ITaskReference> OnTaskFinished { get; }
    }
}
