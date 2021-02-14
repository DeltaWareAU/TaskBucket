using System;
using System.Threading.Tasks;

namespace TaskBucket
{
    internal interface ITask: ITaskReference
    {
        Task ExecuteAsync(IServiceProvider services);
    }
}
