using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBucket.Tasks;
using TaskBucket.Tasks.Options;

namespace TaskBucket
{
    public interface ITaskBucket
    {
        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <typeparam name="TDefinition">Specifies the definition</typeparam>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        /// <remarks>All methods MUST be awaited</remarks>
        ITaskReference AddBackgroundTask<TDefinition>(Func<TDefinition, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null);
        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <typeparam name="TDefinition">Specifies the definition</typeparam>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        /// <remarks>All methods MUST be awaited</remarks>
        ITaskReference AddBackgroundTask<TDefinition>(Func<TDefinition, ITaskReference, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null);
        /// <summary>
        /// Adds a new background tasks to the queue for every parameter
        /// </summary>
        /// <typeparam name="TDefinition">Specifies the definition</typeparam>
        /// <typeparam name="TParameter">Specifies the definition of the parameter</typeparam>
        /// <param name="parameters">Creates a task for each parameter provided injecting the parameter into the action</param>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        /// <remarks>All methods MUST be awaited</remarks>
        List<ITaskReference> AddBackgroundTasks<TDefinition, TParameter>(IEnumerable<TParameter> parameters, Func<TDefinition, TParameter, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null);
        /// <summary>
        /// Adds a new background tasks to the queue for every parameter
        /// </summary>
        /// <typeparam name="TDefinition">Specifies the definition</typeparam>
        /// <typeparam name="TParameter">Specifies the definition of the parameter</typeparam>
        /// <param name="parameters">Creates a task for each parameter provided injecting the parameter into the action</param>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <param name="optionsFactory">Specifies the options used by the Task</param>
        /// <remarks>All methods MUST be awaited</remarks>
        List<ITaskReference> AddBackgroundTasks<TDefinition, TParameter>(IEnumerable<TParameter> parameters, Func<TDefinition, TParameter, ITaskReference, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null);
    }
}
