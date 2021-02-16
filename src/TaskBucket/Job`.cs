﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskBucket
{
    [DebuggerDisplay("Source: {Source} | {Status} - {Identity}")]
    internal class Job<T>: IJob
    {
        private readonly Func<T, Task> _task;

        private readonly Action<IJobReference> _onJobFinished;

        public string Source { get; }

        public int ThreadIndex { get; private set; } = -1;

        public Guid Identity { get; } = Guid.NewGuid();

        public TaskStatus Status { get; private set; } = TaskStatus.Pending;

        public Exception Exception { get; private set; }

        public Job(Func<T, Task> task, Action<IJobReference> onJobFinished)
        {
            _task = task;
            _onJobFinished = onJobFinished;

            Source = typeof(T).Name;
        }

        public async Task ExecuteAsync(IServiceProvider services, int threadIndex)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            ThreadIndex = threadIndex;

            if(Status != TaskStatus.Pending)
            {
                throw new MethodAccessException();
            }

            try
            {
                T instance = services.GetService<T>();

                Status = TaskStatus.Running;

                await _task.Invoke(instance);

                Status = TaskStatus.Completed;
            }
            catch(Exception e)
            {
                Status = TaskStatus.Failed;

                Exception = e;
            }
            finally
            {
                _onJobFinished.Invoke(this);
            }
        }
    }

    [DebuggerDisplay("Source: {Source} | {Status} - {Identity}")]
    internal class Job<T, TValue>: IJob
    {
        private readonly TValue _value;

        private readonly Func<T, TValue, Task> _task;

        private readonly Action<IJobReference> _onJobFinished;

        public string Source { get; }

        public int ThreadIndex { get; private set; } = -1;

        public Guid Identity { get; } = Guid.NewGuid();

        public TaskStatus Status { get; private set; } = TaskStatus.Pending;

        public Exception Exception { get; private set; }

        public Job(Func<T, TValue, Task> task, TValue value, Action<IJobReference> onJobFinished)
        {
            _task = task;
            _value = value;
            _onJobFinished = onJobFinished;

            Source = typeof(T).Name;
        }

        public async Task ExecuteAsync(IServiceProvider services, int threadIndex)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            ThreadIndex = threadIndex;

            if(Status != TaskStatus.Pending)
            {
                throw new MethodAccessException();
            }

            try
            {
                T instance = services.GetService<T>();

                Status = TaskStatus.Running;

                await _task.Invoke(instance, _value);

                Status = TaskStatus.Completed;
            }
            catch(Exception e)
            {
                Status = TaskStatus.Failed;

                Exception = e;
            }
            finally
            {
                _onJobFinished.Invoke(this);
            }
        }
    }
}
