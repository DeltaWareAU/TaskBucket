using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace TaskBucket
{
    internal interface ITask: ITaskReference
    {
        Task ExecuteAsync(IServiceScope scope);
    }
}
