using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBucket.Options;
using TaskBucket.Pooling;
using TaskBucket.Scheduling.Scheduler;
using TaskBucket.Tasks;
using TaskBucket.Tasks.Options;

namespace TaskBucket
{
    internal class TaskBucket: ITaskBucket
    {
        private readonly ILogger<ITaskBucket> _logger;

        private readonly ITaskBucketOptions _options;

        private readonly ITaskPool _taskPool;

        private readonly ITaskScheduler _taskScheduler;

        public TaskBucket(ITaskBucketOptions options, ITaskPool taskPool, ILogger<ITaskBucket> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _taskPool = taskPool ?? throw new ArgumentNullException(nameof(taskPool));
            _logger = logger;
        }

        public ITaskReference AddBackgroundTask<TInvokable>(Action<ITaskOptionsBuilder> optionsFactory = null) where TInvokable : IInvokableTask
        {
            return AddBackgroundTask<TInvokable>(async t => await t.InvokeAsync(), optionsFactory);
        }

        public ITaskReference AddBackgroundTask<TDefinition>(Func<TDefinition, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITask newTask = new ServiceTask<TDefinition>(action, options);

            _taskPool.EnqueueTask(newTask);

            return newTask;
        }

        public ITaskReference AddBackgroundTask<TDefinition>(Func<TDefinition, ITaskReference, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITask newTask = new ServiceTask<TDefinition>(action, options);

            _taskPool.EnqueueTask(newTask);

            return newTask;
        }

        public List<ITaskReference> AddBackgroundTasks<TDefinition, TParameter>(IEnumerable<TParameter> parameters, Func<TDefinition, TParameter, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            List<ITaskReference> references = new List<ITaskReference>();

            foreach(TParameter parameter in parameters)
            {
                ITask newTask = new ParameterTask<TDefinition, TParameter>(action, parameter, options);

                _taskPool.EnqueueTask(newTask);

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

                _taskPool.EnqueueTask(newTask);

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
