using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Tasks.Enums;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Tasks.Asynchronous
{
    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal class AsyncReferencedTask<TService> : AsyncTaskBase<TService>
    {
        protected Func<TService, ITask, Task> ReferenceTask { get; }

        public AsyncReferencedTask(Func<TService, ITask, Task> task, ITaskOptions options) : base(options)
        {
            ReferenceTask = task;
        }

        public override ITaskDetails Copy()
        {
            if (Options.InstanceLimit == InstanceLimit.Single && PreviouslyRanInstance is { State: TaskState.Running })
            {
                return new AsyncReferencedTask<TService>(ReferenceTask, Options)
                {
                    PreviouslyRanInstance = PreviouslyRanInstance
                };
            }

            return new AsyncReferencedTask<TService>(ReferenceTask, Options)
            {
                PreviouslyRanInstance = this
            };
        }

        public override Task RunTaskAsync(object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            return InternalRunTaskAsync(async i => await ReferenceTask.Invoke(i, this), serviceInstance, threadIndex, cancellationToken);
        }
    }

    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal class AsyncReferencedTask<TService, TResult> : AsyncTaskBase<TService, TResult>
    {
        protected Func<TService, ITask, Task<TResult>> ReferenceTask { get; }

        public AsyncReferencedTask(Func<TService, ITask, Task<TResult>> task, ITaskOptions options) : base(options)
        {
            ReferenceTask = task;
        }

        public override ITaskDetails Copy()
        {
            if (Options.InstanceLimit == InstanceLimit.Single && PreviouslyRanInstance is { State: TaskState.Running })
            {
                return new AsyncReferencedTask<TService, TResult>(ReferenceTask, Options)
                {
                    PreviouslyRanInstance = PreviouslyRanInstance
                };
            }

            return new AsyncReferencedTask<TService, TResult>(ReferenceTask, Options)
            {
                PreviouslyRanInstance = this
            };
        }

        public override Task RunTaskAsync(object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            return InternalRunTaskAsync(async i => await ReferenceTask.Invoke(i, this), serviceInstance, threadIndex, cancellationToken);
        }
    }
}