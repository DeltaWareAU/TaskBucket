using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskBucket.Jobs
{
    [DebuggerDisplay("Source: {Source} | {Status} - {Identity}")]
    internal class ParameterJob<TService, TValue>: JobReference, IServiceJob
    {
        private readonly TValue _value;

        private readonly Func<TService, TValue, Task> _task;

        private readonly Func<TService, TValue, IJobReference, Task> _referenceTask;

        private readonly Action<IJobReference> _onJobFinished;

        private DateTime _startTime = DateTime.MinValue;

        private DateTime _endTime = DateTime.MinValue;

        public ParameterJob(Func<TService, TValue, Task> task, TValue value, Action<IJobReference> onJobFinished)
        {
            _task = task;
            _value = value;
            _onJobFinished = onJobFinished;

            Source = typeof(TService).Name;
        }

        public ParameterJob(Func<TService, TValue, IJobReference, Task> task, TValue value, Action<IJobReference> onJobFinished)
        {
            _referenceTask = task;
            _value = value;
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
                    await _referenceTask.Invoke(instance, _value, this);
                }
                else
                {
                    await _task.Invoke(instance, _value);
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
