using System.Threading.Tasks;

namespace TaskBucket.Tasks
{
    public interface IInvokableTask
    {
        Task InvokeAsync();
    }
}
