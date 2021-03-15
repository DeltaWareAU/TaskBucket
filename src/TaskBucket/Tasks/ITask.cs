using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBucket.Tasks
{
    public interface ITask: ITaskReference
    {
        Task StartAsync(IServiceProvider services, int threadIndex, CancellationToken cancellationToken);
    }
}
