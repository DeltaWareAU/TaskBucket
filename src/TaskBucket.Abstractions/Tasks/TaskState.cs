namespace TaskBucket.Abstractions.Tasks
{
    /// <summary>
    /// Specifies the state of an <see cref="IBackgroundTaskReference"/>.
    /// </summary>
    public enum TaskState
    {
        /// <summary>
        /// Indicates that the <see cref="IBackgroundTaskReference"/> has yet to be executed.
        /// </summary>
        Pending,
        /// <summary>
        /// Indicates that the <see cref="IBackgroundTaskReference"/> is currently executing.
        /// </summary>
        Running,
        /// <summary>
        /// Indicates that the <see cref="IBackgroundTaskReference"/> completed successfully.
        /// </summary>
        /// <remarks>The <see cref="IBackgroundTaskReference"/> did not encounter an <see cref="Exception"/> during its execution.</remarks>
        Completed,
        /// <summary>
        /// Indicates that the <see cref="IBackgroundTaskReference"/> failed.
        /// </summary>
        /// <remarks>The <see cref="IBackgroundTaskReference"/> encountered an <see cref="Exception"/> during its execution.</remarks>
        Failed
    }
}
