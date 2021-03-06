using System.Threading.Tasks;

namespace TaskBucket.Jobs
{
    internal interface IJob: IJobReference
    {
        Task ExecuteAsync(int threadIndex);
    }
}
