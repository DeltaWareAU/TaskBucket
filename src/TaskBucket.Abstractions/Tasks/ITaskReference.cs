using TaskBucket.Abstractions.Tasks.Options;

namespace TaskBucket.Abstractions.Tasks
{
    public interface ITaskReference
    {
        /// <summary>
        /// Specifies the Exception of the Task
        /// </summary>
        /// <remarks>This will be null unless the Task State is Failed</remarks>
        Exception? Exception { get; }

        /// <summary>
        /// Specifies the Execution Time of the Task
        /// </summary>
        TimeSpan? ExecutionTime { get; }

        /// <summary>
        /// Specifies the Identity of the Task
        /// </summary>
        Guid Identity { get; }

        /// <summary>
        /// Specifies the State of the Task
        /// </summary>
        TaskState State { get; }

        /// <summary>
        /// Specifies the Bucket Index of the Task
        /// </summary>
        int BucketIndex { get; }

        ITaskOptions Options { get; }
    }
}
