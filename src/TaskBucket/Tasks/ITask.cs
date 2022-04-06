using System;
using TaskBucket.Tasks.Enums;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Tasks
{
    /// <summary>
    /// References a Task
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// Specifies the Exception of the Task
        /// </summary>
        /// <remarks>This will be null unless the Task State is Failed</remarks>
        Exception Exception { get; }

        /// <summary>
        /// Specifies the Execution Time of the Task
        /// </summary>
        TimeSpan ExecutionTime { get; }

        /// <summary>
        /// Specifies the Identity of the Task
        /// </summary>
        Guid Identity { get; }

        /// <summary>
        /// Specifies is the task can be canceled.
        /// </summary>
        bool IsCancelable { get; }

        ITaskOptions Options { get; }

        /// <summary>
        /// Specifies the State of the Task
        /// </summary>
        TaskState State { get; }

        /// <summary>
        /// Specifies the Thread Index of the Task
        /// </summary>
        int ThreadIndex { get; }
    }

    /// <summary>
    /// References a Task
    /// </summary>
    public interface ITask<out TResult> : ITask
    {
        TResult Result { get; }
    }
}