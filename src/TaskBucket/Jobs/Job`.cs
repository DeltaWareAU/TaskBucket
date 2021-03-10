using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskBucket.Jobs
{
    [DebuggerDisplay("Source: {Source} | {Status} - {Identity}")]
    internal class Job<TService>: JobReference, IServiceJob
    {
        private readonly Func<TService, Task> _task = null;

        private readonly Func<TService, IJobReference, Task> _referenceTask = null;

        private readonly Action<IJobReference> _onJobFinished;

        private DateTime _startTime = DateTime.MinValue;

        private DateTime _endTime = DateTime.MinValue;

        public Job(Func<TService, Task> task, Action<IJobReference> onJobFinished)
        {
            _task = task;
            _onJobFinished = onJobFinished;

            Source = typeof(TService).Name;
        }

        public Job(Func<TService, IJobReference, Task> task, Action<IJobReference> onJobFinished)
        {
            _referenceTask = task;
            _onJobFinished = onJobFinished;

            Source = typeof(TService).Name;
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

            TService instance = services.GetService<TService>();

            Status = TaskStatus.Running;

            _startTime = DateTime.Now;

            try
            {
                if(_task == null)
                {
                    await _referenceTask.Invoke(instance, this);
                }
                else
                {
                    await _task.Invoke(instance);
                }

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
