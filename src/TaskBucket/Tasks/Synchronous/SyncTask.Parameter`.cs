using System;
using System.Diagnostics;
using System.Threading;
using TaskBucket.Tasks.Enums;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Tasks.Synchronous
{
    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal class SyncParameterTask<TService, TValue> : SyncTaskBase<TService>
    {
        protected Action<TService, TValue> Task;

        protected TValue Parameter { get; }

        public SyncParameterTask(Action<TService, TValue> task, TValue parameter, ITaskOptions options) : base(options)
        {
            Task = task;
            Parameter = parameter;
        }

        public override ITaskDetails Copy()
        {
            if (Options.InstanceLimit == InstanceLimit.Single && PreviouslyRanInstance is { State: TaskState.Running })
            {
                return new SyncParameterTask<TService, TValue>(Task, Parameter, Options)
                {
                    PreviouslyRanInstance = PreviouslyRanInstance
                };
            }

            return new SyncParameterTask<TService, TValue>(Task, Parameter, Options)
            {
                PreviouslyRanInstance = this
            };
        }

        public override void RunTask(object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            InternalRunTask(i => Task.Invoke(i, Parameter), serviceInstance, threadIndex, cancellationToken);
        }
    }

    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal class SyncParameterTask<TService, TValue, TResult> : SyncTaskBase<TService, TResult>
    {
        protected Func<TService, TValue, TResult> Task;

        protected TValue Parameter { get; }

        public SyncParameterTask(Func<TService, TValue, TResult> task, TValue parameter, ITaskOptions options) : base(options)
        {
            Task = task;
            Parameter = parameter;
        }

        public override ITaskDetails Copy()
        {
            if (Options.InstanceLimit == InstanceLimit.Single && PreviouslyRanInstance is { State: TaskState.Running })
            {
                return new SyncParameterTask<TService, TValue, TResult>(Task, Parameter, Options)
                {
                    PreviouslyRanInstance = PreviouslyRanInstance
                };
            }

            return new SyncParameterTask<TService, TValue, TResult>(Task, Parameter, Options)
            {
                PreviouslyRanInstance = this
            };
        }

        public override void RunTask(object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            InternalRunTask(i => Task.Invoke(i, Parameter), serviceInstance, threadIndex, cancellationToken);
        }
    }
}