using TaskBucket.Abstractions.Tasks.Scheduling;

namespace TaskBucket.Abstractions.Tasks.Options
{
    public interface ITaskOptions
    {
        /// <summary>
        /// Sets the instance limit for the <see cref="IBackgroundTask"/>
        /// </summary>
        /// <remarks>A value of 0 means there is no limit.</remarks>
        int InstanceLimit { get; }

        /// <summary>
        /// Specifies an action to be performed when the task has completed.
        /// </summary>
        Action<ITaskReference>? OnTaskFinished { get; }

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