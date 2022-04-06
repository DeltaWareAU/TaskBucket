using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Tasks.Enums;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Tasks.Asynchronous
{
    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal class AsyncParameterTask<TService, TValue> : AsyncTaskBase<TService>
    {
        protected TValue Parameter { get; }
        protected Func<TService, TValue, Task> Task { get; }

        public AsyncParameterTask(Func<TService, TValue, Task> task, TValue parameter, ITaskOptions options) : base(options)
        {
            Task = task;
            Parameter = parameter;
        }

        public override ITaskDetails Copy()
        {
            if (Options.InstanceLimit == InstanceLimit.Single && PreviouslyRanInstance is { State: TaskState.Running })
            {
                return new AsyncParameterTask<TService, TValue>(Task, Parameter, Options)
                {
                    PreviouslyRanInstance = PreviouslyRanInstance
                };
            }

            return new AsyncParameterTask<TService, TValue>(Task, Parameter, Options)
            {
                PreviouslyRanInstance = this
            };
        }

        public override Task RunTaskAsync(object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            return InternalRunTaskAsync(async i => await Task.Invoke(i, Parameter), serviceInstance, threadIndex, cancellationToken);
        }
    }

    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal class AsyncParameterTask<TService, TValue, TResult> : AsyncTaskBase<TService, TResult>
    {
        protected TValue Parameter { get; }
        protected Func<TService, TValue, Task<TResult>> Task { get; }

        public AsyncParameterTask(Func<TService, TValue, Task<TResult>> task, TValue parameter, ITaskOptions options) : base(options)
        {
            Task = task;
            Parameter = parameter;
        }

        public override ITaskDetails Copy()
        {
            if (Options.InstanceLimit == InstanceLimit.Single && PreviouslyRanInstance is { State: TaskState.Running })
            {
                return new AsyncParameterTask<TService, TValue, TResult>(Task, Parameter, Options)
                {
                    PreviouslyRanInstance = PreviouslyRanInstance
                };
            }

            return new AsyncParameterTask<TService, TValue, TResult>(Task, Parameter, Options)
            {
                PreviouslyRanInstance = this
            };
        }

        public override Task RunTaskAsync(object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            return InternalRunTaskAsync(async i => await Task.Invoke(i, Parameter), serviceInstance, threadIndex, cancellationToken);
        }
    }
}