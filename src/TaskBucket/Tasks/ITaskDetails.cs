using System;

namespace TaskBucket.Tasks
{
    internal interface ITaskDetails : ITask
    {
        /// <summary>
        /// Contains a reference to this tasks previously ran instance.
        /// </summary>
        ITask PreviouslyRanInstance { get; }

        /// <summary>
        /// Specifies the type required for the Task to run.
        /// </summary>
        Type ServiceType { get; }

        /// <summary>
        /// Copies the task and generates a new identity.
        /// </summary>
        ITaskDetails Copy();
    }

    internal interface ITaskDetails<out TResult> : ITask<TResult>, ITaskDetails
    {
    }
}