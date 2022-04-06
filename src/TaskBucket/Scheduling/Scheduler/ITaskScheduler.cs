using TaskBucket.Tasks;

namespace TaskBucket.Scheduling.Scheduler
{
    internal interface ITaskScheduler
    {
        /// <summary>
        /// Runs the Scheduler.
        /// </summary>
        void RunScheduler();

        /// <summary>
        /// Schedules the provided <see cref="ITask"/>.
        /// </summary>
        /// <param name="task">The <see cref="ITask"/> to be add to the schedule.</param>
        void ScheduleTask(ITaskDetails task);
    }
}