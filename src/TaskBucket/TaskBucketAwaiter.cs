using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Tasks;
using TaskStatus = TaskBucket.Tasks.Enums.TaskStatus;

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
        /// Locks the current thread until the task has completed
        /// </summary>
        /// <param name="task">The task to be waited for</param>
        public static void Wait(ITaskReference task)
        {
            while (task.Status == TaskStatus.Pending || task.Status == TaskStatus.Running)
            {
                Thread.Sleep(PollRateMs);
            }
        }

        /// <summary>
        /// Locks the current thread until the tasks have completed
        /// </summary>
        /// <param name="tasks">The tasks to be waited for</param>
        public static void WaitAll(params ITaskReference[] tasks)
        {
            foreach (ITaskReference task in tasks)
            {
                Wait(task);
            }
        }

        /// <summary>
        /// Locks the current thread until the tasks have completed
        /// </summary>
        /// <param name="tasks">The tasks to be waited for</param>
        public static void WaitAll(List<ITaskReference> tasks)
        {
            foreach (ITaskReference task in tasks)
            {
                Wait(task);
            }
        }

        /// <summary>
        /// Waits for the task to complete
        /// </summary>
        /// <param name="task">The task to be waited for</param>
        public static async Task WaitAsync(ITaskReference task)
        {
            while (task.Status == TaskStatus.Pending || task.Status == TaskStatus.Running)
            {
                await Task.Delay(PollRateMs);
            }
        }

        /// <summary>
        /// Waits for the tasks to complete
        /// </summary>
        /// <param name="tasks">The tasks to be waited for</param>
        public static async Task WaitAllAsync(params ITaskReference[] tasks)
        {
            foreach (ITaskReference task in tasks)
            {
                await WaitAsync(task);
            }
        }

        /// <summary>
        /// Waits for the tasks to complete
        /// </summary>
        /// <param name="tasks">The tasks to be waited for</param>
        public static Task WaitAllAsync(List<ITaskReference> tasks)
        {
            return WaitAllAsync(tasks.ToArray());
        }
    }
}
