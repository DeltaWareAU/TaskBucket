using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBucket.Options;
using TaskBucket.Scheduling.Scheduler;
using TaskBucket.Tasks;
using TaskBucket.Tasks.Options;

namespace TaskBucket
{
    internal class TaskBucket: ITaskBucket
    {
        private readonly ILogger<ITaskBucket> _logger;

        private readonly IBucketOptions _options;

        private readonly IScheduler _scheduler;

        public TaskBucket(IBucketOptions options, IScheduler scheduler, ILogger<ITaskBucket> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _logger = logger;
        }

        public ITaskReference AddBackgroundTask<TDefinition>(Func<TDefinition, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITask newTask = new Tasks.Task<TDefinition>(action, options);

            _scheduler.ScheduleTask(newTask);

            return newTask;
        }

        public ITaskReference AddBackgroundTask<TDefinition>(Func<TDefinition, ITaskReference, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITask newTask = new Tasks.Task<TDefinition>(action, options);

            _scheduler.ScheduleTask(newTask);

            return newTask;
        }

        public List<ITaskReference> AddBackgroundTasks<TDefinition, TParameter>(IEnumerable<TParameter> parameters, Func<TDefinition, TParameter, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            List<ITaskReference> references = new List<ITaskReference>();

            foreach(TParameter parameter in parameters)
            {
                ITask newTask = new ParameterTask<TDefinition, TParameter>(action, parameter, options);

                _scheduler.ScheduleTask(newTask);

                references.Add(newTask);
            }

            return references;
        }

        public List<ITaskReference> AddBackgroundTasks<TDefinition, TParameter>(IEnumerable<TParameter> parameters, Func<TDefinition, TParameter, ITaskReference, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            List<ITaskReference> references = new List<ITaskReference>();

            foreach(TParameter parameter in parameters)
            {
                ITask newTask = new ParameterTask<TDefinition, TParameter>(action, parameter, options);

                _scheduler.ScheduleTask(newTask);

                references.Add(newTask);
            }

            return references;
        }

        private ITaskOptions BuildTaskOptions(Action<TaskOptionsBuilder> optionsFactory = null)
        {
            TaskOptionsBuilder optionsBuilder = new TaskOptionsBuilder();

            optionsFactory?.Invoke(optionsBuilder);

            return optionsBuilder.BuildOptions();
        }
    }
}
