using System;
using System.Threading.Tasks;

namespace TaskBucket.Jobs
{
    public class InstanceJob<TInstance>: JobReference, IJob
    {
        private readonly TInstance _instance;

        private readonly Func<TInstance, Task> _task;

        private readonly Action<IJobReference> _onJobFinished;

        private DateTime _startTime = DateTime.MinValue;

        private DateTime _endTime = DateTime.MinValue;

        public InstanceJob(TInstance instance, Func<TInstance, Task> task, Action<IJobReference> onJobFinished)
        {
            _instance = instance;
            _task = task;
            _onJobFinished = onJobFinished;

            Source = typeof(TInstance).Name;
        }

        public async Task ExecuteAsync(int threadIndex)
        {
            ThreadIndex = threadIndex;

            if(Status != TaskStatus.Pending)
            {
                throw new MethodAccessException();
            }

            Status = TaskStatus.Running;

            _startTime = DateTime.Now;

            try
            {
                await _task.Invoke(_instance);

                _endTime = DateTime.Now;

                Status = TaskStatus.Completed;
            }
            catch(Exception e)
            {
                _endTime = DateTime.Now;

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
