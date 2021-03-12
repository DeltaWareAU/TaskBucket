﻿using System;
using System.Threading.Tasks;

namespace TaskBucket.Tasks
{
    internal class InstanceTask<TInstance>: TaskReference, IInstanceTask
    {
        private readonly TInstance _instance;

        private readonly Func<TInstance, Task> _task = null;

        private readonly Func<TInstance, ITaskReference, Task> _referenceTask = null;

        private readonly Action<ITaskReference> _onJobFinished;

        public InstanceTask(TInstance instance, Func<TInstance, Task> task, Action<ITaskReference> onJobFinished) : base(typeof(TInstance).Name)
        {
            _instance = instance;
            _task = task;
            _onJobFinished = onJobFinished;
        }

        public InstanceTask(TInstance instance, Func<TInstance, ITaskReference, Task> task, Action<ITaskReference> onJobFinished) : base(typeof(TInstance).Name)
        {
            _instance = instance;
            _referenceTask = task;
            _onJobFinished = onJobFinished;
        }

        public async Task ExecuteAsync(int threadIndex)
        {
            ThreadIndex = threadIndex;

            if(Status != TaskStatus.Pending)
            {
                throw new MethodAccessException();
            }

            Status = TaskStatus.Running;

            StartDate = DateTime.Now;

            try
            {
                if(_task == null)
                {
                    await _referenceTask.Invoke(_instance, this);
                }
                else
                {
                    await _task.Invoke(_instance);
                }

                EndDate = DateTime.Now;

                Status = TaskStatus.Completed;
            }
            catch(Exception e)
            {
                EndDate = DateTime.Now;

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
