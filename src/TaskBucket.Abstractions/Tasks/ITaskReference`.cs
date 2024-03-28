namespace TaskBucket.Abstractions.Tasks
{
    /// <summary>
    /// References a Task
    /// </summary>
    public interface ITaskReference<out TResult> : ITaskReference
    {
        TResult? Result { get; }
    }
}
