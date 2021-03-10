using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskBucket.Jobs
{
    [DebuggerDisplay("Source: {Source} | {Status} - {Identity}")]
    internal class ParameterJob<T, TValue>: JobReference, IServiceJob
    {
        private readonly TValue _value;

        private readonly Func<T, TValue, Task> _task;

        private readonly Func<T, TValue, IJobReference, Task> _referenceTask;

        private readonly Action<IJobReference> _onJobFinished;

        private DateTime _startTime = DateTime.MinValue;

        private DateTime _endTime = DateTime.MinValue;

        public ParameterJob(Func<T, TValue, Task> task, TValue value, Action<IJobReference> onJobFinished)
        {
            _task = task;
            _value = value;
            _onJobFinished = onJobFinished;

            Source = typeof(T).Name;
        }

        public ParameterJob(Func<T, TValue, IJobReference, Task> task, TValue value, Action<IJobReference> onJobFinished)
        {
            _referenceTask = task;
            _value = value;
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
