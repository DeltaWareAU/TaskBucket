namespace TaskBucket.Tasks.Enums
{
    /// <summary>
    /// Specifies when a task will be executed.
    /// </summary>
    public enum TaskPriority
    {
        /// <summary>
        /// Scheduled to execute once a thread is available with low priority.
        /// </summary>
        Low,

        /// <summary>
        /// Scheduled to execute once a thread is available.
        /// </summary>
        Normal,

        /// <summary>
        /// Scheduled to execute once a thread is available with high priority.
        /// </summary>
        High,

        /// <summary>
        /// Scheduled to execute immediately. NOTE: THIS HAS NOT BEEN IMPLEMENTED
        /// </summary>
        /// <remarks>Will stop execution of existing tasks.</remarks>
        Critical
    }
}