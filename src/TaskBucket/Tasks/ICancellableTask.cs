using System.Threading;

namespace TaskBucket.Tasks
{
    public interface ICancellableTask
    {
        CancellationToken CancellationToken { get; set; }
    }
}
