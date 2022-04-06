using TaskBucket.Scheduling;
using TaskBucket.Tasks.Enums;

namespace TaskBucket.Tasks.Options
{
    /// <summary>
    /// Assists in building <see cref="ITaskOptions"/>.
    /// </summary>
    public interface ITaskOptionsBuilder
    {
        /// <summary>
        /// Sets the <see cref="InstanceLimit"/> for the <see cref="ITask"/>.
        /// </summary>
        InstanceLimit InstanceLimitation { get; set; }

        /// <summary>
        /// Sets the <see cref="TaskPriority"/> for the <see cref="ITask"/>.
        /// </summary>
        TaskPriority Priority { get; set; }

        /// <summary>
        /// Sets the <see cref="ITaskSchedule"/> for the <see cref="ITask"/>.
        /// </summary>
        ITaskSchedule Schedule { get; set; }
    }
}