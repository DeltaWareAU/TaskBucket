using TaskBucket.Tasks;

namespace TaskBucket.Scheduling.Scheduler
{
    internal interface ITaskScheduler
    {
        void ScheduleTask(ITask task);

        void RunScheduler();
    }
}
