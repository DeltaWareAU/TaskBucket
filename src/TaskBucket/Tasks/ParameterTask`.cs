using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Tasks.Options;
using TaskStatus = TaskBucket.Tasks.Enums.TaskStatus;

namespace TaskBucket.Tasks
{
    [DebuggerDisplay("Source: {Source} | {Status} - {Identity}")]
    internal class ParameterTask<TService, TValue>: TaskReference, ITask
    {
        private readonly TValue _parameter;

        private readonly Func<TService, TValue, Task> _task;

        private readonly Func<TService, TValue, ITask, Task> _referenceTask;

        public override bool IsCancelable { get; } = typeof(TService).GetInterfaces().Contains(typeof(ICancellableTask));

        public ParameterTask(Func<TService, TValue, Task> task, TValue parameter, ITaskOptions options) : base(typeof(TService).Name, options)
        {
            _task = task;
            _parameter = parameter;
        }

        public ParameterTask(Func<TService, TValue, ITask, Task> task, TValue parameter, ITaskOptions options) : base(typeof(TService).Name, options)
        {
            _referenceTask = task;
            _parameter = parameter;
        }

        public async Task StartAsync(IServiceProvider services, int threadIndex, CancellationToken cancellationToken)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            ThreadIndex = threadIndex;

            if(Status != TaskStatus.Pending)
            {
                throw new InvalidOperationException("A task cannot be started unless it is pending");
            }

            TService instance = services.GetRequiredService<TService>();

            if(instance is ICancellableTask cancellableTask)
            {
                cancellableTask.CancellationToken = cancellationToken;
            }

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
                Options.OnTaskFinished?.Invoke(this);
            }
        }
    }
}
