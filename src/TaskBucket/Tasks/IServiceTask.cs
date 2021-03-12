using System;
using System.Threading.Tasks;

namespace TaskBucket.Tasks
{
    internal interface IServiceTask: ITaskReference
    {
        Task ExecuteAsync(IServiceProvider services, int threadIndex);
    }
}
