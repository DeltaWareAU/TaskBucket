using System;
using System.Threading.Tasks;

namespace TaskBucket.Tasks
{
    public interface IServiceTask: ITaskReference
    {
        Task ExecuteAsync(IServiceProvider services, int threadIndex);
    }
}
