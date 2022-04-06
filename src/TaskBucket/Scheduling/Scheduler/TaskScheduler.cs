using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskBucket.Pooling;
using TaskBucket.Scheduling.Options;
using TaskBucket.Tasks;

namespace TaskBucket.Scheduling.Scheduler
{
    internal class TaskScheduler : ITaskScheduler
    {
        private readonly object _concurrencyLock = new();
        private readonly ILogger _logger;

        private readonly ITaskSchedulerOptions _options;
        private readonly Dictionary<DateTime, List<ITaskDetails>> _scheduledTasks = new();
        private readonly ITaskPool _taskPool;

        public TaskScheduler(ITaskSchedulerOptions options, ITaskPool taskPool, ILogger<ITaskScheduler> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _taskPool = taskPool ?? throw new ArgumentNullException(nameof(taskPool));

            _logger = logger;
        }

        public void RunScheduler()
        {
            List<DateTime> completedSchedules = new();
            List<ITaskDetails> pendingTasks = new();

            lock (_concurrencyLock)
            {
                DateTime currentTime = DateTime.UtcNow;

                foreach (KeyValuePair<DateTime, List<ITaskDetails>> taskSchedules in _scheduledTasks.Where(t => currentTime > t.Key))
                {
                    completedSchedules.Add(taskSchedules.Key);
                    pendingTasks.AddRange(taskSchedules.Value);
                }

                // Cleanup old task schedules
                foreach (DateTime completedSchedule in completedSchedules)
                {
                    _scheduledTasks.Remove(completedSchedule);
                }
            }

            // Process Pending Tasks
            foreach (ITaskDetails pendingTask in pendingTasks)
            {
                ScheduleTask(pendingTask.Copy());

                _taskPool.EnqueueTask(pendingTask);
            }
        }

        public void ScheduleTask(ITaskDetails task)
        {
            if (task.Options.Schedule == null)
            {
                throw new ArgumentNullException(nameof(task.Options.Schedule), "A Task Schedule must be set before scheduling a task");
            }

            DateTime? nextRun = task.Options.Schedule.GetNextSchedule(DateTime.UtcNow, _options.TimeZone);

            if (nextRun == null)
            {
                _logger?.LogInformation("Task[{taskId}] Has completed its schedule and will no longer be ran.", task.Identity);

                // If the next return date returns null it means the task has completed its schedule
                // and won't be run again.
                return;
            }

            lock (_concurrencyLock)
            {
                if (_scheduledTasks.TryGetValue(nextRun.Value, out List<ITaskDetails> tasks))
                {
                    tasks.Add(task);
                }
                else
                {
                    tasks = new List<ITaskDetails>
                    {
                        task
                    };

                    _scheduledTasks.Add(nextRun.Value, tasks);
                }
            }
        }
    }
}