using TaskBucket.Tasks.Enums;

namespace TaskBucket.Tasks.Options
{
    public interface ITaskOptionsBuilder
    {
        TaskPriority Priority { get; set; }
    }
}
