using TaskBucket.Abstractions.Tasks.Scheduling;

namespace TaskBucket.Abstractions.Tasks.Options
{
    public interface ITaskOptionsBuilder
    {
        /// <summary>
        /// Sets the instance limit for the <see cref="IBackgroundTask"/>
        /// </summary>
        /// <remarks>A value of 0 means there is no limit.</remarks>
        int InstanceLimitation { get; set; }

        /// <summary>
        /// Sets the <see cref="TaskPriority"/> for the <see cref="IBackgroundTask"/>.
        /// </summary>
        TaskPriority Priority { get; set; }

        /// <summary>
        /// Sets the <see cref="ITaskSchedule"/> for the <see cref="IBackgroundTask"/>.
        /// </summary>
        ITaskSchedule Schedule { get; set; }
    }
}
