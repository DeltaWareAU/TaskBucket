using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBucket.Tasks;
using TaskBucket.Tasks.Options;

namespace TaskBucket
{
    public interface ITaskBucket
    {
        #region Add Task

        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        ITask AddBackgroundTask<TInvokable>(Action<ITaskOptionsBuilder> optionsFactory = null) where TInvokable : IInvokableTask;

        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        /// <remarks>All methods MUST be awaited</remarks>
        ITask AddBackgroundTask<TService>(Func<TService, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <typeparam name="TResult">The type returned.</typeparam>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        /// <remarks>All methods MUST be awaited</remarks>
        ITask<TResult> AddBackgroundTask<TService, TResult>(Func<TService, Task<TResult>> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <param name="action">The synchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        ITask AddBackgroundTask<TService>(Action<TService> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <typeparam name="TResult">The type returned.</typeparam>
        /// <param name="action">The synchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        ITask<TResult> AddBackgroundTask<TService, TResult>(Func<TService, TResult> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        /// <remarks>All methods MUST be awaited</remarks>
        ITask AddBackgroundTask<TService>(Func<TService, ITask, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <typeparam name="TResult">The type returned.</typeparam>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        /// <remarks>All methods MUST be awaited</remarks>
        ITask<TResult> AddBackgroundTask<TService, TResult>(Func<TService, ITask, Task<TResult>> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <param name="action">The synchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        ITask AddBackgroundTask<TService>(Action<TService, ITask> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <typeparam name="TResult">The type returned.</typeparam>
        /// <param name="action">The synchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        ITask<TResult> AddBackgroundTask<TService, TResult>(Func<TService, ITask, TResult> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        #endregion Add Task

        #region Add Tasks with Parameters

        /// <summary>
        /// Adds a new background tasks to the queue for every parameter
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <typeparam name="TParameter">Specifies the service to be used. of the parameter</typeparam>
        /// <param name="parameters">
        /// Creates a task for each parameter provided injecting the parameter into the action
        /// </param>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        /// <remarks>All methods MUST be awaited</remarks>
        ITask[] AddBackgroundTasks<TService, TParameter>(IEnumerable<TParameter> parameters, Func<TService, TParameter, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        /// <summary>
        /// Adds a new background tasks to the queue for every parameter
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <typeparam name="TParameter">Specifies the service to be used. of the parameter</typeparam>
        /// <typeparam name="TResult">The type returned.</typeparam>
        /// <param name="parameters">
        /// Creates a task for each parameter provided injecting the parameter into the action
        /// </param>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        /// <remarks>All methods MUST be awaited</remarks>
        ITask<TResult>[] AddBackgroundTasks<TService, TParameter, TResult>(IEnumerable<TParameter> parameters, Func<TService, TParameter, Task<TResult>> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        /// <summary>
        /// Adds a new background tasks to the queue for every parameter
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <typeparam name="TParameter">Specifies the service to be used. of the parameter</typeparam>
        /// <param name="parameters">
        /// Creates a task for each parameter provided injecting the parameter into the action
        /// </param>
        /// <param name="action">The synchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        ITask[] AddBackgroundTasks<TService, TParameter>(IEnumerable<TParameter> parameters, Action<TService, TParameter> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        /// <summary>
        /// Adds a new background tasks to the queue for every parameter
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <typeparam name="TParameter">Specifies the service to be used. of the parameter</typeparam>
        /// <typeparam name="TResult">The type returned.</typeparam>
        /// <param name="parameters">
        /// Creates a task for each parameter provided injecting the parameter into the action
        /// </param>
        /// <param name="action">The synchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        ITask<TResult>[] AddBackgroundTasks<TService, TParameter, TResult>(IEnumerable<TParameter> parameters, Func<TService, TParameter, TResult> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        /// <summary>
        /// Adds a new background tasks to the queue for every parameter
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <typeparam name="TParameter">Specifies the service to be used. of the parameter</typeparam>
        /// <param name="parameters">
        /// Creates a task for each parameter provided injecting the parameter into the action
        /// </param>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        /// <remarks>All methods MUST be awaited</remarks>
        ITask[] AddBackgroundTasks<TService, TParameter>(IEnumerable<TParameter> parameters, Func<TService, TParameter, ITask, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        /// <summary>
        /// Adds a new background tasks to the queue for every parameter
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <typeparam name="TParameter">Specifies the service to be used. of the parameter</typeparam>
        /// <typeparam name="TResult">The type returned.</typeparam>
        /// <param name="parameters">
        /// Creates a task for each parameter provided injecting the parameter into the action
        /// </param>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        /// <remarks>All methods MUST be awaited</remarks>
        ITask<TResult>[] AddBackgroundTasks<TService, TParameter, TResult>(IEnumerable<TParameter> parameters, Func<TService, TParameter, ITask, Task<TResult>> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        /// <summary>
        /// Adds a new background tasks to the queue for every parameter
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <typeparam name="TParameter">Specifies the service to be used. of the parameter</typeparam>
        /// <param name="parameters">
        /// Creates a task for each parameter provided injecting the parameter into the action
        /// </param>
        /// <param name="action">The synchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        ITask[] AddBackgroundTasks<TService, TParameter>(IEnumerable<TParameter> parameters, Action<TService, TParameter, ITask> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        /// <summary>
        /// Adds a new background tasks to the queue for every parameter
        /// </summary>
        /// <typeparam name="TService">Specifies the service to be used.</typeparam>
        /// <typeparam name="TParameter">Specifies the service to be used. of the parameter</typeparam>
        /// <typeparam name="TResult">The type returned.</typeparam>
        /// <param name="parameters">
        /// Creates a task for each parameter provided injecting the parameter into the action
        /// </param>
        /// <param name="action">The synchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        ITask<TResult>[] AddBackgroundTasks<TService, TParameter, TResult>(IEnumerable<TParameter> parameters, Func<TService, TParameter, ITask, TResult> action, Action<ITaskOptionsBuilder> optionsFactory = null);

        #endregion Add Tasks with Parameters
    }
}