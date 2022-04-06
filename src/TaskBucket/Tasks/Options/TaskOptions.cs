using System;
using TaskBucket.Scheduling;
using TaskBucket.Tasks.Enums;

namespace TaskBucket.Tasks.Options
{
    /// <inheritdoc/>
    internal class TaskOptions : ITaskOptions
    {
        /// <inheritdoc/>
        public InstanceLimit InstanceLimit { get; set; } = InstanceLimit.None;

        /// <inheritdoc/>
        public Action<ITask> OnTaskFinished { get; set; }

        /// <inheritdoc/>
        public TaskPriority Priority { get; set; } = TaskPriority.Normal;

        /// <inheritdoc/>
        public ITaskSchedule Schedule { get; set; }
    }
}