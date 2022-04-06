using System;
using TaskBucket.Scheduling;
using TaskBucket.Tasks.Enums;

namespace TaskBucket.Tasks.Options
{
    /// <summary>
    /// The options used during execution of an <see cref="ITask"/>.
    /// </summary>
    public interface ITaskOptions
    {
        /// <summary>
        /// Specifies the <see cref="InstanceLimit"/>.
        /// </summary>
        InstanceLimit InstanceLimit { get; }

        /// <summary>
        /// Specifies an action to be performed when the task has completed.
        /// </summary>
        Action<ITask> OnTaskFinished { get; }

        /// <summary>
        /// Specifies the Priority of the Task
        /// </summary>
        TaskPriority Priority { get; }

        /// <summary>
        /// Specifies the <see cref="ITaskSchedule"/>.
        /// </summary>
        ITaskSchedule Schedule { get; }
    }
}