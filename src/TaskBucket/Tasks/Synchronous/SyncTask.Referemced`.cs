using System;
using System.Diagnostics;
using System.Threading;
using TaskBucket.Tasks.Enums;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Tasks.Synchronous
{
    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal class SyncReferencedTask<TService> : SyncTaskBase<TService>
    {
        protected Action<TService, ITask> Task { get; }

        public SyncReferencedTask(Action<TService, ITask> task, ITaskOptions options) : base(options)
        {
            Task = task;
        }

        public override ITaskDetails Copy()
        {
            if (Options.InstanceLimit == InstanceLimit.Single && PreviouslyRanInstance is { State: TaskState.Running })
            {
                return new SyncReferencedTask<TService>(Task, Options)
                {
                    PreviouslyRanInstance = PreviouslyRanInstance
                };
            }

            return new SyncReferencedTask<TService>(Task, Options)
            {
                PreviouslyRanInstance = this
            };
        }

        public override void RunTask(object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            InternalRunTask(i => Task.Invoke(i, this), serviceInstance, threadIndex, cancellationToken);
        }
    }

    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal class SyncReferencedTask<TService, TResult> : SyncTaskBase<TService, TResult>
    {
        protected Func<TService, ITask, TResult> Task { get; }

        public SyncReferencedTask(Func<TService, ITask, TResult> task, ITaskOptions options) : base(options)
        {
            Task = task;
        }

        public override ITaskDetails Copy()
        {
            if (Options.InstanceLimit == InstanceLimit.Single && PreviouslyRanInstance is { State: TaskState.Running })
            {
                return new SyncReferencedTask<TService, TResult>(Task, Options)
                {
                    PreviouslyRanInstance = PreviouslyRanInstance
                };
            }

            return new SyncReferencedTask<TService, TResult>(Task, Options)
            {
                PreviouslyRanInstance = this
            };
        }

        public override void RunTask(object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            InternalRunTask(i => Task.Invoke(i, this), serviceInstance, threadIndex, cancellationToken);
        }
    }
}