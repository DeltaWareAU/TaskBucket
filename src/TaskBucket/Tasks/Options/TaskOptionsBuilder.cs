using TaskBucket.Tasks.Enums;

namespace TaskBucket.Tasks.Options
{
    internal class TaskOptionsBuilder: ITaskOptionsBuilder
    {
        public TaskPriority Priority { get; set; }

        public ITaskOptions BuildOptions()
        {
            TaskOptions options = new TaskOptions
            {
                Priority = Priority
            };

            return options;
        }
    }
}
