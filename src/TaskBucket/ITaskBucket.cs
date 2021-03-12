using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskBucket.Tasks;

namespace TaskBucket
{
    public interface ITaskBucket
    {
        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <typeparam name="TDefinition">Specifies the definition</typeparam>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <remarks>All methods MUST be awaited</remarks>
        ITaskReference AddBackgroundTask<TDefinition>(Func<TDefinition, Task> action);
        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <typeparam name="TDefinition">Specifies the definition</typeparam>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <remarks>All methods MUST be awaited</remarks>
        ITaskReference AddBackgroundTask<TDefinition>(Func<TDefinition, ITaskReference, Task> action);
        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <typeparam name="TDefinition">Specifies the definition</typeparam>
        /// <param name="instance">Provides an instance of the definition</param>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <remarks>All methods MUST be awaited</remarks>
        ITaskReference AddBackgroundTask<TDefinition>(TDefinition instance, Func<TDefinition, Task> action);
        /// <summary>
        /// Adds a new background tasks to the queue
        /// </summary>
        /// <typeparam name="TDefinition">Specifies the definition</typeparam>
        /// <param name="instance">Provides an instance of the definition</param>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <remarks>All methods MUST be awaited</remarks>
        ITaskReference AddBackgroundTask<TDefinition>(TDefinition instance, Func<TDefinition, ITaskReference, Task> action);
        /// <summary>
        /// Adds a new background tasks to the queue for every parameter
        /// </summary>
        /// <typeparam name="TDefinition">Specifies the definition</typeparam>
        /// <typeparam name="TParameter">Specifies the definition of the parameter</typeparam>
        /// <param name="parameters">Creates a task for each parameter provided injecting the parameter into the action</param>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <remarks>All methods MUST be awaited</remarks>
        List<ITaskReference> AddBackgroundTasks<TDefinition, TParameter>(IEnumerable<TParameter> parameters, Func<TDefinition, TParameter, Task> action);
        /// <summary>
        /// Adds a new background tasks to the queue for every parameter
        /// </summary>
        /// <typeparam name="TDefinition">Specifies the definition</typeparam>
        /// <typeparam name="TParameter">Specifies the definition of the parameter</typeparam>
        /// <param name="parameters">Creates a task for each parameter provided injecting the parameter into the action</param>
        /// <param name="action">The asynchronous action to be performed</param>
        /// <remarks>All methods MUST be awaited</remarks>
        List<ITaskReference> AddBackgroundTasks<TDefinition, TParameter>(IEnumerable<TParameter> parameters, Func<TDefinition, TParameter, ITaskReference, Task> action);
    }
}
