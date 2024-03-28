namespace TaskBucket.Abstractions.Tasks
{
    /// <summary>
    /// Specifies the state of an <see cref="ITaskReference"/>.
    /// </summary>
    public enum TaskState
    {
        /// <summary>
        /// Indicates that the <see cref="ITaskReference"/> has yet to be executed.
        /// </summary>
        Pending,
        /// <summary>
        /// Indicates that the <see cref="ITaskReference"/> is currently executing.
        /// </summary>
        Running,
        /// <summary>
        /// Indicates that the <see cref="ITaskReference"/> completed successfully.
        /// </summary>
        /// <remarks>The <see cref="ITaskReference"/> did not encounter an <see cref="Exception"/> during its execution.</remarks>
        Completed,
        /// <summary>
        /// Indicates that the <see cref="ITaskReference"/> failed.
        /// </summary>
        /// <remarks>The <see cref="ITaskReference"/> encountered an <see cref="Exception"/> during its execution.</remarks>
        Failed
    }
}
