using TaskBucket.Scheduling;
using TaskBucket.Tasks.Enums;

namespace TaskBucket.Tasks.Options
{
    internal class TaskOptionsBuilder : ITaskOptionsBuilder
    {
        public TaskPriority Priority { get; set; }

        public ITaskSchedule Schedule { get; set; }

        public ITaskOptions BuildOptions()
        {
            TaskOptions options = new TaskOptions
            {
                Priority = Priority,
                Schedule = Schedule
            };

            return options;
        }
    }
}
