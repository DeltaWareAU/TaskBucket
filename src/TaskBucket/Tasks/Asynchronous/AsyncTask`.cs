using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Tasks.Enums;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Tasks.Asynchronous
{
    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal class AsyncTask<TService> : AsyncTaskBase<TService>
    {
        protected Func<TService, Task> Task { get; }

        public AsyncTask(Func<TService, Task> task, ITaskOptions options) : base(options)
        {
            Task = task;
        }

        public override ITaskDetails Copy()
        {
            if (Options.InstanceLimit == InstanceLimit.Single && PreviouslyRanInstance is { State: TaskState.Running })
            {
                return new AsyncTask<TService>(Task, Options)
                {
                    PreviouslyRanInstance = PreviouslyRanInstance
                };
            }

            return new AsyncTask<TService>(Task, Options)
            {
                PreviouslyRanInstance = this
            };
        }

        public override Task RunTaskAsync(object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            return InternalRunTaskAsync(async i => await Task.Invoke(i), serviceInstance, threadIndex, cancellationToken);
        }
    }

    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal class AsyncTask<TService, TResult> : AsyncTaskBase<TService, TResult>
    {
        protected Func<TService, Task<TResult>> Task { get; }

        public AsyncTask(Func<TService, Task<TResult>> task, ITaskOptions options) : base(options)
        {
            Task = task;
        }

        public override ITaskDetails Copy()
        {
            if (Options.InstanceLimit == InstanceLimit.Single && PreviouslyRanInstance is { State: TaskState.Running })
            {
                return new AsyncTask<TService, TResult>(Task, Options)
                {
                    PreviouslyRanInstance = PreviouslyRanInstance
                };
            }

            return new AsyncTask<TService, TResult>(Task, Options)
            {
                PreviouslyRanInstance = this
            };
        }

        public override Task RunTaskAsync(object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            return InternalRunTaskAsync(async i => await Task.Invoke(i), serviceInstance, threadIndex, cancellationToken);
        }
    }
}