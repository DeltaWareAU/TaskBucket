using TaskBucket.Scheduling;
using TaskBucket.Tasks.Enums;

namespace TaskBucket.Tasks.Options
{
    /// <inheritdoc/>
    internal class TaskOptionsBuilder : ITaskOptionsBuilder
    {
        /// <inheritdoc/>
        public InstanceLimit InstanceLimitation { get; set; } = InstanceLimit.None;

        /// <inheritdoc/>
        public TaskPriority Priority { get; set; } = TaskPriority.Normal;

        /// <inheritdoc/>
        public ITaskSchedule Schedule { get; set; }

        /// <summary>
        /// Builds an instance of <see cref="ITaskOptions"/>.
        /// </summary>
        /// <returns>A instance of <see cref="ITaskOptions"/> containing the provided settings.</returns>
        public ITaskOptions BuildOptions()
        {
            TaskOptions options = new TaskOptions
            {
                Priority = Priority,
                Schedule = Schedule,
                InstanceLimit = InstanceLimitation
            };

            return options;
        }
    }
}