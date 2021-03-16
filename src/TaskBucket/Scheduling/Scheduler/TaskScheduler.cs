using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskBucket.Pooling;
using TaskBucket.Scheduling.Options;
using TaskBucket.Tasks;

namespace TaskBucket.Scheduling.Scheduler
{
    internal class TaskScheduler: ITaskScheduler
    {
        private readonly ILogger _logger;

        private readonly ITaskPool _taskPool;

        private readonly ITaskSchedulerOptions _options;

        private readonly SortedList<DateTime, List<ITask>> _taskSchedule = new SortedList<DateTime, List<ITask>>();

        public TaskScheduler(ITaskSchedulerOptions options, ITaskPool taskPool, ILogger<ITaskScheduler> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _taskPool = taskPool ?? throw new ArgumentNullException(nameof(taskPool));

            _logger = logger;
        }

        public void ScheduleTask(ITask task)
        {
            DateTime? nextRun = task.Options.Schedule.GetNextSchedule(DateTime.UtcNow, _options.TimeZone);

            if(nextRun == null)
            {
                // A null next run date means we won't be running the task again.
                return;
            }

            if(_taskSchedule.TryGetValue(nextRun.Value, out List<ITask> tasks))
            {
                tasks.Add(task);
            }
            else
            {
                tasks = new List<ITask>
                {
                    task
                };

                _taskSchedule.Add(nextRun.Value, tasks);
            }
        }

        public void RunScheduler()
        {
            DateTime currentTime = DateTime.UtcNow;

            List<KeyValuePair<DateTime, List<ITask>>> pendingTasks = _taskSchedule.Where(t => currentTime > t.Key).ToList();

            foreach(KeyValuePair<DateTime, List<ITask>> scheduledTasks in pendingTasks)
            {
                // Removed the DateTime task List as this is no longer valid.
                _taskSchedule.Remove(scheduledTasks.Key);

                foreach(ITask scheduledTask in scheduledTasks.Value)
                {
                    _taskPool.EnqueueTask(scheduledTask);

                    // Reschedule the task in case it may need to run again.
                    ScheduleTask(scheduledTask);
                }
            }
        }
    }
}
