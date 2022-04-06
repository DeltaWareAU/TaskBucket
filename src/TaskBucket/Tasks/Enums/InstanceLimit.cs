namespace TaskBucket.Tasks.Enums
{
    /// <summary>
    /// Specifies the Instance Limit of an <see cref="ITask"/>.
    /// </summary>
    public enum InstanceLimit
    {
        /// <summary>
        /// No Limit - Unlimited instances of the <see cref="ITask"/> can be executed.
        /// </summary>
        None,
        /// <summary>
        /// Single Instance - Only a single instance of the <see cref="ITask"/> can be executed.
        /// </summary>
        Single
    }
}