using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace TaskBucket
{
    internal class Task<T>: ITask
    {
        private readonly Func<T, Task> _task;

        private readonly Action<ITaskReference> _onJobFinished;

        public Guid Identity { get; } = Guid.NewGuid();

        public TaskStatus Status { get; private set; } = TaskStatus.Pending;

        public Exception Exception { get; private set; }

        public Task(Func<T, Task> task, Action<ITaskReference> onJobFinished)
        {
            _task = task;
            _onJobFinished = onJobFinished;
        }

        public async Task ExecuteAsync(IServiceScope scope)
        {
            if(scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            if(Status != TaskStatus.Pending)
            {
                throw new MethodAccessException();
            }

            try
            {
                T instance = scope.ServiceProvider.GetService<T>();

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
    
    internal class Task<T, TValue>: ITask
    {
        private readonly TValue _value;

        private readonly Func<T, TValue, Task> _task;

        private readonly Action<ITaskReference> _onJobFinished;

        public Guid Identity { get; } = Guid.NewGuid();

        public TaskStatus Status { get; private set; } = TaskStatus.Pending;

        public Exception Exception { get; private set; }

        public Task(TValue value, Func<T, TValue, Task> task, Action<ITaskReference> onJobFinished)
        {
            _value = value;
            _task = task;
            _onJobFinished = onJobFinished;
        }

        public async Task ExecuteAsync(IServiceScope scope)
        {
            if(scope == null)
            {
                throw new ArgumentNullException(nameof(scope));
            }

            if(Status != TaskStatus.Pending)
            {
                throw new MethodAccessException();
            }

            try
            {
                T instance = scope.ServiceProvider.GetService<T>();

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
