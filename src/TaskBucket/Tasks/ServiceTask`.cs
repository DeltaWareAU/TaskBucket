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
    internal class ServiceTask<TService> : TaskReference, ITask
    {
        private readonly Func<TService, Task> _task = null;

        private readonly Func<TService, ITaskReference, Task> _referenceTask = null;

        public override bool IsCancelable { get; } = typeof(TService).GetInterfaces().Contains(typeof(ICancellableTask));

        public ServiceTask(Func<TService, Task> task, ITaskOptions options) : base(typeof(TService).Name, options)
        {
            _task = task;
        }

        public ServiceTask(Func<TService, ITaskReference, Task> task, ITaskOptions options) : base(typeof(TService).Name, options)
        {
            _referenceTask = task;
        }

        public ITask Copy()
        {
            if (_task != null)
            {
                return new ServiceTask<TService>(_task, Options);
            }

            return new ServiceTask<TService>(_referenceTask, Options);
        }

        public async Task StartAsync(IServiceProvider services, int threadIndex, CancellationToken cancellationToken)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            ThreadIndex = threadIndex;

            if (Status != TaskStatus.Pending)
            {
                throw new InvalidOperationException("A task cannot be started unless it is pending");
            }

            TService instance = services.GetRequiredService<TService>();

            if (instance is ICancellableTask cancellableTask)
            {
                cancellableTask.CancellationToken = cancellationToken;
            }

            Status = TaskStatus.Running;

            StartDate = DateTime.Now;

            try
            {
                if (_task == null)
                {
                    await _referenceTask.Invoke(instance, this);
                }
                else
                {
                    await _task.Invoke(instance);
                }

                EndDate = DateTime.Now;

                Status = TaskStatus.Completed;
            }
            catch (Exception e)
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
