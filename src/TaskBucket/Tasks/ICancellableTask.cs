using System.Threading;

namespace TaskBucket.Tasks
{
    /// <summary>
    /// Indicates to Task Bucket that the current <see cref="ITask"/> can be cancelled.
    /// </summary>
    public interface ICancellableTask
    {
        /// <summary>
        /// The Cancellation Token provided by Task Bucket that will be used to Cancel the <see cref="ITask"/>.
        /// </summary>
        CancellationToken CancellationToken { get; set; }
    }
}
