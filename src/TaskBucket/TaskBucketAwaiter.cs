using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Tasks;
using TaskBucket.Tasks.Enums;

namespace TaskBucket
{
    /// <summary>
    /// Contains methods used for awaiting tasks both Synchronously and Asynchronously
    /// </summary>
    public static class TaskBucketAwaiter
    {
        /// <summary>
        /// Specifies how often in milliseconds a tasks status will be checked
        /// </summary>
        private const int PollRateMs = 50;

        /// <summary>
        /// Waits for the task to complete synchronously
        /// </summary>
        /// <param name="task">The task to be waited for</param>
        public static void Wait(this ITask task)
        {
            while (task.State is TaskState.Pending or TaskState.Running)
            {
                Thread.Sleep(PollRateMs);
            }
        }

        /// <summary>
        /// Waits for the task to complete synchronously
        /// </summary>
        /// <param name="task">The task to be waited for</param>
        public static TResult Wait<TResult>(this ITask<TResult> task)
        {
            while (task.State is TaskState.Pending or TaskState.Running)
            {
                Thread.Sleep(PollRateMs);
            }

            return task.Result;
        }

        /// <summary>
        /// Waits for the tasks to complete synchronously
        /// </summary>
        /// <param name="tasks">The tasks to be waited for</param>
        public static IEnumerable<TResult> WaitAll<TResult>(this IEnumerable<ITask<TResult>> tasks)
        {
            List<TResult> results = new List<TResult>();

            foreach (ITask<TResult> task in tasks)
            {
                results.Add(task.Wait());
            }

            return results;
        }

        /// <summary>
        /// Waits for the tasks to complete synchronously
        /// </summary>
        /// <param name="tasks">The tasks to be waited for</param>
        public static void WaitAll(this IEnumerable<ITask> tasks)
        {
            foreach (ITask task in tasks)
            {
                task.Wait();
            }
        }

        /// <summary>
        /// Waits for the tasks to complete asynchronously
        /// </summary>
        /// <param name="tasks">The task to be waited for</param>
        public static async Task WaitAllAsync(this IEnumerable<ITask> tasks)
        {
            foreach (ITask task in tasks)
            {
                await task.WaitAsync();
            }
        }

        /// <summary>
        /// Waits for the tasks to complete asynchronously
        /// </summary>
        /// <param name="tasks">The task to be waited for</param>
        public static async Task<IEnumerable<TResult>> WaitAllAsync<TResult>(this IEnumerable<ITask<TResult>> tasks)
        {
            List<TResult> results = new List<TResult>();

            foreach (ITask<TResult> task in tasks)
            {
                results.Add(await task.WaitAsync());
            }

            return results;
        }

        /// <summary>
        /// Waits for the task to complete asynchronously
        /// </summary>
        /// <param name="task">The tasks to be waited for</param>
        public static async Task WaitAsync(this ITask task)
        {
            while (task.State is TaskState.Pending or TaskState.Running)
            {
                await Task.Delay(PollRateMs);
            }
        }

        /// <summary>
        /// Waits for the task to complete asynchronously
        /// </summary>
        /// <param name="task">The tasks to be waited for</param>
        public static async Task<TResult> WaitAsync<TResult>(this ITask<TResult> task)
        {
            while (task.State is TaskState.Pending or TaskState.Running)
            {
                await Task.Delay(PollRateMs);
            }

            return task.Result;
        }
    }
}