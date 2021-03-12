using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskBucket.Tasks
{
    [DebuggerDisplay("Source: {Source} | {Status} - {Identity}")]
    internal class ParameterTask<TService, TValue>: TaskReference, IServiceTask
    {
        private readonly TValue _parameter;

        private readonly Func<TService, TValue, Task> _task;

        private readonly Func<TService, TValue, IServiceTask, Task> _referenceTask;

        private readonly Action<IServiceTask> _onJobFinished;

        private DateTime _startTime = DateTime.MinValue;

        private DateTime _endTime = DateTime.MinValue;

        public ParameterTask(Func<TService, TValue, Task> task, TValue parameter, Action<IServiceTask> onJobFinished)
        {
            _task = task;
            _parameter = parameter;
            _onJobFinished = onJobFinished;

            Source = typeof(TService).Name;
        }

        public ParameterTask(Func<TService, TValue, IServiceTask, Task> task, TValue parameter, Action<IServiceTask> onJobFinished)
        {
            _referenceTask = task;
            _parameter = parameter;
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
                    await _referenceTask.Invoke(instance, _parameter, this);
                }
                else
                {
                    await _task.Invoke(instance, _parameter);
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
