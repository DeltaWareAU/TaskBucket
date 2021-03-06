using System;
using System.Threading.Tasks;

namespace TaskBucket.Jobs
{
    public interface IServiceJob: IJobReference
    {
        Task ExecuteAsync(IServiceProvider services, int threadIndex);
    }
}
