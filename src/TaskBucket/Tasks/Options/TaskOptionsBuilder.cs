using System;
using TaskBucket.Tasks.Enums;

namespace TaskBucket.Tasks.Options
{
    internal class TaskOptionsBuilder: ITaskOptionsBuilder
    {
        public TaskPriority Priority { get; set; }

        public void AsReoccurringTask(TimeSpan interval)
        {
            throw new NotImplementedException();
        }

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
