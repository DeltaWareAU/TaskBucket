using System;
using TaskBucket.Tasks.Enums;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Tasks
{
    /// <summary>
    /// References a Task
    /// </summary>
    public interface ITaskReference
    {
        /// <summary>
        /// Specifies the source of the Task
        /// </summary>
        string Source { get; }

        /// <summary>
        /// Specifies the Thread Index of the Task
        /// </summary>
        int ThreadIndex { get; }

        /// <summary>
        /// Specifies the Identity of the Task
        /// </summary>
        Guid Identity { get; }

        /// <summary>
        /// Specifies the Status of the Task
        /// </summary>
        TaskStatus Status { get; }

        ITaskOptions Options { get; }

        /// <summary>
        /// Specifies the Execution Time of the Task
        /// </summary>
        TimeSpan ExecutionTime { get; }

        /// <summary>
        /// Specifies the Exception of the Task
        /// </summary>
        /// <remarks>This will be null unless the Task Status is Failed</remarks>
        Exception Exception { get; }

        /// <summary>
        /// Gets the Task Reference
        /// </summary>
        ITaskReference GetTaskReference();
    }
}
