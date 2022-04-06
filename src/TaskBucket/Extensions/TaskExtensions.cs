using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace TaskBucket.Tasks
{
    public static class TaskExtensions
    {
        /// <summary>
        /// Waits for the task to complete synchronously
        /// </summary>
        /// <param name="task">The task to be waited for</param>
        public static void Wait(this ITask task)
        {
            TaskBucketAwaiter.Wait(task);
        }

        /// <summary>
        /// Waits for the tasks to complete synchronously
        /// </summary>
        /// <param name="tasks">The tasks to be waited for</param>
        public static void WaitAll(this ITask[] tasks)
        {
            TaskBucketAwaiter.WaitAll(tasks);
        }

        /// <summary>
        /// Waits for the tasks to complete asynchronously
        /// </summary>
        /// <param name="tasks">The tasks to be waited for</param>
        public static Task WaitAllAsync(this ITask[] tasks)
        {
            return TaskBucketAwaiter.WaitAllAsync(tasks);
        }

        /// <summary>
        /// Waits for the task to complete asynchronously
        /// </summary>
        /// <param name="task">The task to be waited for</param>
        public static Task WaitAsync(this ITask task)
        {
            return TaskBucketAwaiter.WaitAsync(task);
        }
    }
}