using System;
using System.Diagnostics;
using System.Threading;
using TaskBucket.Tasks.Enums;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Tasks.Synchronous
{
    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal class SyncTask<TService> : SyncTaskBase<TService>
    {
        protected Action<TService> Task { get; }

        public SyncTask(Action<TService> task, ITaskOptions options) : base(options)
        {
            Task = task;
        }

        public override ITaskDetails Copy()
        {
            if (Options.InstanceLimit == InstanceLimit.Single && PreviouslyRanInstance is { State: TaskState.Running })
            {
                return new SyncTask<TService>(Task, Options)
                {
                    PreviouslyRanInstance = PreviouslyRanInstance
                };
            }

            return new SyncTask<TService>(Task, Options)
            {
                PreviouslyRanInstance = this
            };
        }

        public override void RunTask(object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            InternalRunTask(i => Task.Invoke(i), serviceInstance, threadIndex, cancellationToken);
        }
    }

    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal class SyncTask<TService, TResult> : SyncTaskBase<TService, TResult>
    {
        protected Func<TService, TResult> Task { get; }

        public SyncTask(Func<TService, TResult> task, ITaskOptions options) : base(options)
        {
            Task = task;
        }

        public override ITaskDetails Copy()
        {
            if (Options.InstanceLimit == InstanceLimit.Single && PreviouslyRanInstance is { State: TaskState.Running })
            {
                return new SyncTask<TService, TResult>(Task, Options)
                {
                    PreviouslyRanInstance = PreviouslyRanInstance
                };
            }

            return new SyncTask<TService, TResult>(Task, Options)
            {
                PreviouslyRanInstance = this
            };
        }

        public override void RunTask(object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            InternalRunTask(i => Task.Invoke(i), serviceInstance, threadIndex, cancellationToken);
        }
    }
}