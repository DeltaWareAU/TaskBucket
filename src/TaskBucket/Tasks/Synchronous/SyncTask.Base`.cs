using System;
using System.Diagnostics;
using System.Threading;
using TaskBucket.Tasks.Enums;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Tasks.Synchronous
{
    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal abstract class SyncTaskBase<TService> : TaskDetails, ISynchronousTask
    {
        protected SyncTaskBase(ITaskOptions options) : base(options)
        {
            ServiceType = typeof(TService);

            IsCancelable = ServiceType.ImplementsInterface<ICancellableTask>();
        }

        public abstract override ITaskDetails Copy();

        public void InternalRunTask(Action<TService> taskExecuter, object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            ThreadIndex = threadIndex;

            if (State != TaskState.Pending)
            {
                throw new InvalidOperationException("A task cannot be started unless it is pending");
            }

            if (serviceInstance is not TService instance)
            {
                throw new ArgumentException($"An invalid Service Instance was provided, the provided type is {serviceInstance.GetType().Name} but it should be {ServiceType.Name}");
            }

            if (instance is ICancellableTask cancellableTask)
            {
                cancellableTask.CancellationToken = cancellationToken;
            }

            State = TaskState.Running;

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                taskExecuter.Invoke(instance);

                State = TaskState.Completed;
            }
            catch (Exception e)
            {
                State = TaskState.Failed;

                Exception = e;

                // We want to throw this error so logging can pick it up.
                throw;
            }
            finally
            {
                stopwatch.Stop();

                ExecutionTime = stopwatch.Elapsed;

                Options.OnTaskFinished?.Invoke(this);
            }
        }

        public abstract void RunTask(object serviceInstance, int threadIndex, CancellationToken cancellationToken);
    }

    [DebuggerDisplay("Source: {ServiceType.Name} | {State} - {Identity}")]
    internal abstract class SyncTaskBase<TService, TResult> : TaskDetails<TResult>, ISynchronousTask
    {
        protected SyncTaskBase(ITaskOptions options) : base(options)
        {
            ServiceType = typeof(TService);

            IsCancelable = ServiceType.ImplementsInterface<ICancellableTask>();
        }

        public abstract override ITaskDetails Copy();

        public void InternalRunTask(Func<TService, TResult> taskExecuter, object serviceInstance, int threadIndex, CancellationToken cancellationToken)
        {
            ThreadIndex = threadIndex;

            if (State != TaskState.Pending)
            {
                throw new InvalidOperationException("A task cannot be started unless it is pending");
            }

            if (serviceInstance is not TService instance)
            {
                throw new ArgumentException($"An invalid Service Instance was provided, the provided type is {serviceInstance.GetType().Name} but it should be {ServiceType.Name}");
            }

            if (instance is ICancellableTask cancellableTask)
            {
                cancellableTask.CancellationToken = cancellationToken;
            }

            State = TaskState.Running;

            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                Result = taskExecuter.Invoke(instance);

                State = TaskState.Completed;
            }
            catch (Exception e)
            {
                State = TaskState.Failed;

                Exception = e;

                // We want to throw this error so logging can pick it up.
                throw;
            }
            finally
            {
                stopwatch.Stop();

                ExecutionTime = stopwatch.Elapsed;

                Options.OnTaskFinished?.Invoke(this);
            }
        }

        public abstract void RunTask(object serviceInstance, int threadIndex, CancellationToken cancellationToken);
    }
}