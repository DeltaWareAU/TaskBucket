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

        public ParameterTask(Func<TService, TValue, Task> task, TValue parameter, Action<IServiceTask> onJobFinished) : base(typeof(TService).Name)
        {
            _task = task;
            _parameter = parameter;
            _onJobFinished = onJobFinished;
        }

        public ParameterTask(Func<TService, TValue, IServiceTask, Task> task, TValue parameter, Action<IServiceTask> onJobFinished) : base(typeof(TService).Name)
        {
            _referenceTask = task;
            _parameter = parameter;
            _onJobFinished = onJobFinished;
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

            StartDate = DateTime.Now;

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

                EndDate = DateTime.Now;

                Status = TaskStatus.Completed;
            }
            catch(Exception e)
            {
                EndDate = DateTime.Now;

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
