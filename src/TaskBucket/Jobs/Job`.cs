using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskBucket.Jobs
{
    [DebuggerDisplay("Source: {Source} | {Status} - {Identity}")]
    internal class Job<T>: JobReference, IServiceJob
    {
        private readonly Func<T, Task> _task;

        private readonly Action<IJobReference> _onJobFinished;

        private DateTime _startTime = DateTime.MinValue;

        private DateTime _endTime = DateTime.MinValue;

        public Job(Func<T, Task> task, Action<IJobReference> onJobFinished)
        {
            _task = task;
            _onJobFinished = onJobFinished;

            Source = typeof(T).Name;
        }

        public async Task ExecuteAsync(IServiceProvider services, int threadIndex)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            ThreadIndex = threadIndex;

            if(Status != TaskStatus.Pending)
            {
                throw new MethodAccessException();
            }

            T instance = services.GetService<T>();

            Status = TaskStatus.Running;

            _startTime = DateTime.Now;

            try
            {
                await _task.Invoke(instance);

                _endTime = DateTime.Now;

                Status = TaskStatus.Completed;
            }
            catch(Exception e)
            {
                _endTime = DateTime.Now;

                Status = TaskStatus.Failed;

                Exception = e;
            }
            finally
            {
                _onJobFinished.Invoke(this);
            }
        }
    }
}
