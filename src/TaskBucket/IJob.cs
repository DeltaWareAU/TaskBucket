using System;
using System.Threading.Tasks;

namespace TaskBucket
{
    internal interface IJob: IJobReference
    {
        Task ExecuteAsync(IServiceProvider services, int threadIndex);
    }
}
