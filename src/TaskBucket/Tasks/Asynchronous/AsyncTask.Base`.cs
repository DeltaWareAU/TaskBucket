using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Tasks.Enums;
using TaskBucket.Tasks.Options;

namespace TaskBucket.Tasks.Asynchronous
{
    internal abstract class AsyncTaskBase<TService> : TaskDetails, IAsynchronousTask
    {
        protected AsyncTaskBase(ITaskOptions options) : base(options)
        {
            ServiceType = typeof(TService);

            IsCancelable = ServiceType.ImplementsInterface<ICancellableTask>();
        }

        public async Task InternalRunTaskAsync(Func<TService, Task> taskExecuter, object serviceInstance, int threadIndex, CancellationToken cancellationToken)
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
                await taskExecuter.Invoke(instance);

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

        public abstract Task RunTaskAsync(object serviceInstance, int threadIndex, CancellationToken cancellationToken);
    }

    internal abstract class AsyncTaskBase<TService, TResult> : TaskDetails<TResult>, IAsynchronousTask
    {
        protected AsyncTaskBase(ITaskOptions options) : base(options)
        {
            ServiceType = typeof(TService);

            IsCancelable = ServiceType.ImplementsInterface<ICancellableTask>();
        }

        public async Task InternalRunTaskAsync(Func<TService, Task<TResult>> taskExecuter, object serviceInstance, int threadIndex, CancellationToken cancellationToken)
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
                Result = await taskExecuter.Invoke(instance);

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

        public abstract Task RunTaskAsync(object serviceInstance, int threadIndex, CancellationToken cancellationToken);
    }
}