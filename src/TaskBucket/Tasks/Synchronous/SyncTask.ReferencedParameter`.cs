using System;
using System.Diagnostics;
using System.Threading;
using TaskBucket.Tasks.Enums;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Tasks.Synchronous
{
    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal class SyncReferencedParameterTask<TService, TValue> : SyncTaskBase<TService>
    {
        protected Action<TService, TValue, ITask> Task;

        protected TValue Parameter { get; }

        public SyncReferencedParameterTask(Action<TService, TValue, ITask> task, TValue parameter, ITaskOptions options) : base(options)
        {
            Task = task;
            Parameter = parameter;
        }

        public override ITaskDetails Copy()
        {
            if (Options.InstanceLimit == InstanceLimit.Single && PreviouslyRanInstance is { State: TaskState.Running })
            {
                return new SyncReferencedParameterTask<TService, TValue>(Task, Parameter, Options)
                {
                    PreviouslyRanInstance = PreviouslyRanInstance
                };
            }

            return new SyncReferencedParameterTask<TService, TValue>(Task, Parameter, Options)
            {
                PreviouslyRanInstance = this
            };
        }

        public override void RunTask(object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            InternalRunTask(i => Task.Invoke(i, Parameter, this), serviceInstance, threadIndex, cancellationToken);
        }
    }

    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal class SyncReferencedParameterTask<TService, TValue, TResult> : SyncTaskBase<TService, TResult>
    {
        protected Func<TService, TValue, ITask, TResult> Task;

        protected TValue Parameter { get; }

        public SyncReferencedParameterTask(Func<TService, TValue, ITask, TResult> task, TValue parameter, ITaskOptions options) : base(options)
        {
            Task = task;
            Parameter = parameter;
        }

        public override ITaskDetails Copy()
        {
            if (Options.InstanceLimit == InstanceLimit.Single && PreviouslyRanInstance is { State: TaskState.Running })
            {
                return new SyncReferencedParameterTask<TService, TValue, TResult>(Task, Parameter, Options)
                {
                    PreviouslyRanInstance = PreviouslyRanInstance
                };
            }

            return new SyncReferencedParameterTask<TService, TValue, TResult>(Task, Parameter, Options)
            {
                PreviouslyRanInstance = this
            };
        }

        public override void RunTask(object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            InternalRunTask(i => Task.Invoke(i, Parameter, this), serviceInstance, threadIndex, cancellationToken);
        }
    }
}