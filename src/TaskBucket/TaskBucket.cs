using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBucket.Pooling;
using TaskBucket.Scheduling.Scheduler;
using TaskBucket.Tasks;
using TaskBucket.Tasks.Asynchronous;
using TaskBucket.Tasks.Options;
using TaskBucket.Tasks.Synchronous;

namespace TaskBucket
{
    internal class TaskBucket : ITaskBucket
    {
        private readonly ILogger<ITaskBucket> _logger;

        private readonly ITaskPool _taskPool;

        private readonly ITaskScheduler _taskScheduler;

        public TaskBucket(ITaskPool taskPool, ITaskScheduler taskScheduler, ILogger<ITaskBucket> logger)
        {
            _taskPool = taskPool ?? throw new ArgumentNullException(nameof(taskPool));
            _taskScheduler = taskScheduler ?? throw new ArgumentNullException(nameof(taskScheduler));
            _logger = logger;
        }

        #region Add Task with Parameter

        public ITask AddBackgroundTask<TService, TParameter>(TParameter parameter, Func<TService, TParameter, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails newTask = new AsyncParameterTask<TService, TParameter>(action, parameter, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        public ITask<TResult> AddBackgroundTask<TService, TParameter, TResult>(TParameter parameter, Func<TService, TParameter, Task<TResult>> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails<TResult> newTask = new AsyncParameterTask<TService, TParameter, TResult>(action, parameter, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        public ITask AddBackgroundTask<TService, TParameter>(TParameter parameter, Action<TService, TParameter> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails newTask = new SyncParameterTask<TService, TParameter>(action, parameter, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        public ITask<TResult> AddBackgroundTask<TService, TParameter, TResult>(TParameter parameter, Func<TService, TParameter, TResult> action,
            Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails<TResult> newTask = new SyncParameterTask<TService, TParameter, TResult>(action, parameter, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        public ITask AddBackgroundTask<TService, TParameter>(TParameter parameter, Func<TService, TParameter, ITask, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails newTask = new AsyncReferencedParameterTask<TService, TParameter>(action, parameter, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        public ITask<TResult> AddBackgroundTask<TService, TParameter, TResult>(TParameter parameter, Func<TService, TParameter, ITask, Task<TResult>> action,
            Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails<TResult> newTask = new AsyncReferencedParameterTask<TService, TParameter, TResult>(action, parameter, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        public ITask AddBackgroundTask<TService, TParameter>(TParameter parameter, Action<TService, TParameter, ITask> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails newTask = new SyncReferencedParameterTask<TService, TParameter>(action, parameter, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        public ITask<TResult> AddBackgroundTask<TService, TParameter, TResult>(TParameter parameter, Func<TService, TParameter, ITask, TResult> action,
            Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails<TResult> newTask = new SyncReferencedParameterTask<TService, TParameter, TResult>(action, parameter, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        #endregion Add Task with Parameter

        #region Add Task

        public ITask AddBackgroundTask<TInvokable>(Action<ITaskOptionsBuilder> optionsFactory = null) where TInvokable : IInvokableTask
        {
            return AddBackgroundTask<TInvokable>(async t => await t.InvokeAsync(), optionsFactory);
        }

        public ITask AddBackgroundTask<TService>(Func<TService, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails newTask = new AsyncTask<TService>(action, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        public ITask<TResult> AddBackgroundTask<TService, TResult>(Func<TService, Task<TResult>> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails<TResult> newTask = new AsyncTask<TService, TResult>(action, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        public ITask AddBackgroundTask<TService>(Action<TService> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails newTask = new SyncTask<TService>(action, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        public ITask<TResult> AddBackgroundTask<TService, TResult>(Func<TService, TResult> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails<TResult> newTask = new SyncTask<TService, TResult>(action, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        public ITask AddBackgroundTask<TService>(Func<TService, ITask, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails newTask = new AsyncReferencedTask<TService>(action, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        public ITask<TResult> AddBackgroundTask<TService, TResult>(Func<TService, ITask, Task<TResult>> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails<TResult> newTask = new AsyncReferencedTask<TService, TResult>(action, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        public ITask AddBackgroundTask<TService>(Action<TService, ITask> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails newTask = new SyncReferencedTask<TService>(action, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        public ITask<TResult> AddBackgroundTask<TService, TResult>(Func<TService, ITask, TResult> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskOptions options = BuildTaskOptions(optionsFactory);

            ITaskDetails<TResult> newTask = new SyncReferencedTask<TService, TResult>(action, options);

            if (newTask.Options.Schedule == null)
            {
                _taskPool.EnqueueTask(newTask);
            }
            else
            {
                _taskScheduler.ScheduleTask(newTask);
            }

            return newTask;
        }

        #endregion Add Task

        #region Add Tasks with Parameters

        public ITask[] AddBackgroundTasks<TService, TParameter>(IEnumerable<TParameter> parameters, Func<TService, TParameter, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ConcurrentBag<ITask> references = new ConcurrentBag<ITask>();

            Parallel.ForEach(parameters, parameter =>
            {
                references.Add(AddBackgroundTask(parameter, action, optionsFactory));
            });

            return references.ToArray();
        }

        public ITask<TResult>[] AddBackgroundTasks<TService, TParameter, TResult>(IEnumerable<TParameter> parameters, Func<TService, TParameter, Task<TResult>> action,
            Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ConcurrentBag<ITask<TResult>> references = new ConcurrentBag<ITask<TResult>>();

            Parallel.ForEach(parameters, parameter =>
            {
                references.Add(AddBackgroundTask(parameter, action, optionsFactory));
            });

            return references.ToArray();
        }

        public ITask[] AddBackgroundTasks<TService, TParameter>(IEnumerable<TParameter> parameters, Action<TService, TParameter> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ConcurrentBag<ITask> references = new ConcurrentBag<ITask>();

            Parallel.ForEach(parameters, parameter =>
            {
                references.Add(AddBackgroundTask(parameter, action, optionsFactory));
            });

            return references.ToArray();
        }

        public ITask<TResult>[] AddBackgroundTasks<TService, TParameter, TResult>(IEnumerable<TParameter> parameters, Func<TService, TParameter, TResult> action,
            Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ConcurrentBag<ITask<TResult>> references = new ConcurrentBag<ITask<TResult>>();

            Parallel.ForEach(parameters, parameter =>
            {
                references.Add(AddBackgroundTask(parameter, action, optionsFactory));
            });

            return references.ToArray();
        }

        public ITask[] AddBackgroundTasks<TService, TParameter>(IEnumerable<TParameter> parameters, Func<TService, TParameter, ITask, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ConcurrentBag<ITask> references = new ConcurrentBag<ITask>();

            Parallel.ForEach(parameters, parameter =>
            {
                references.Add(AddBackgroundTask(parameter, action, optionsFactory));
            });

            return references.ToArray();
        }

        public ITask<TResult>[] AddBackgroundTasks<TService, TParameter, TResult>(IEnumerable<TParameter> parameters, Func<TService, TParameter, ITask, Task<TResult>> action,
            Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ConcurrentBag<ITask<TResult>> references = new ConcurrentBag<ITask<TResult>>();

            Parallel.ForEach(parameters, parameter =>
            {
                references.Add(AddBackgroundTask(parameter, action, optionsFactory));
            });

            return references.ToArray();
        }

        public ITask[] AddBackgroundTasks<TService, TParameter>(IEnumerable<TParameter> parameters, Action<TService, TParameter, ITask> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ConcurrentBag<ITask> references = new ConcurrentBag<ITask>();

            Parallel.ForEach(parameters, parameter =>
            {
                references.Add(AddBackgroundTask(parameter, action, optionsFactory));
            });

            return references.ToArray();
        }

        public ITask<TResult>[] AddBackgroundTasks<TService, TParameter, TResult>(IEnumerable<TParameter> parameters, Func<TService, TParameter, ITask, TResult> action,
            Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ConcurrentBag<ITask<TResult>> references = new ConcurrentBag<ITask<TResult>>();

            Parallel.ForEach(parameters, parameter =>
            {
                references.Add(AddBackgroundTask(parameter, action, optionsFactory));
            });

            return references.ToArray();
        }

        #endregion Add Tasks with Parameters

        private ITaskOptions BuildTaskOptions(Action<TaskOptionsBuilder> optionsFactory = null)
        {
            TaskOptionsBuilder optionsBuilder = new TaskOptionsBuilder();

            optionsFactory?.Invoke(optionsBuilder);

            return optionsBuilder.BuildOptions();
        }
    }
}