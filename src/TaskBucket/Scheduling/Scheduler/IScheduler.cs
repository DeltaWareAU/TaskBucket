using TaskBucket.Tasks;

namespace TaskBucket.Scheduling.Scheduler
{
    internal interface IScheduler
    {
        void ScheduleTask(ITask task);

        void RunScheduler();
    }
}
