using System;

namespace TaskBucket.Tasks.Enums
{
    /// <summary>
    /// Specifies the state of an <see cref="ITask"/>.
    /// </summary>
    public enum TaskState
    {
        /// <summary>
        /// Indicates that the <see cref="ITask"/> has yet to be executed.
        /// </summary>
        Pending,
        /// <summary>
        /// Indicates that the <see cref="ITask"/> is currently executing.
        /// </summary>
        Running,
        /// <summary>
        /// Indicates that the <see cref="ITask"/> completed successfully.
        /// </summary>
        /// <remarks>The <see cref="ITask"/> did not encounter an <see cref="Exception"/> during its execution.</remarks>
        Completed,
        /// <summary>
        /// Indicates that the <see cref="ITask"/> failed.
        /// </summary>
        /// <remarks>The <see cref="ITask"/> encountered an <see cref="Exception"/> during its execution.</remarks>
        Failed
    }
}