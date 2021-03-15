using TaskBucket.Tasks;

namespace TaskBucket.Scheduling.Scheduler
{
    public interface IScheduler
    {
        bool IsRunning { get; }

        void ScheduleTask(ITask task);

        void RunScheduler();

        void CancelAllCancellableTasks();
    }
}
