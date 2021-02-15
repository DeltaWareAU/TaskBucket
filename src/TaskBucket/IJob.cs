using System;
using System.Threading.Tasks;

namespace TaskBucket
{
    public interface IJob: IJobReference
    {
        Task ExecuteAsync(IServiceProvider services);
    }
}
