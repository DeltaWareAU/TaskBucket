using System.Threading.Tasks;

namespace TaskBucket.Tasks
{
    internal interface IInstanceTask: ITaskReference
    {
        Task ExecuteAsync(int threadIndex);
    }
}
