using System;
using System.Collections.Concurrent;
using TaskBucket.Abstractions.Tasks;
using TaskBucket.Execution.Tasks;

namespace TaskBucket.Queue
{
    internal class PriorityTaskQueue
    {
        private readonly ConcurrentQueue<ITaskExecutor> _criticalPriorityQueue = new();
        private readonly ConcurrentQueue<ITaskExecutor> _highPriorityQueue = new();
        private readonly ConcurrentQueue<ITaskExecutor> _lowPriorityQueue = new();
        private readonly ConcurrentQueue<ITaskExecutor> _normalPriorityQueue = new();

        public void Enqueue(ITaskExecutor task)
        {
            switch (task.Options.Priority)
            {
                case TaskPriority.Critical:
                    _criticalPriorityQueue.Enqueue(task);
                    break;
                case TaskPriority.High:
                    _highPriorityQueue.Enqueue(task);
                    break;
                case TaskPriority.Normal:
                    _normalPriorityQueue.Enqueue(task);
                    break;
                case TaskPriority.Low:
                    _lowPriorityQueue.Enqueue(task);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Always returns the most recently added task with the highest priority.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool TryDequeue(out ITaskExecutor? task)
        {
            if (_criticalPriorityQueue.TryDequeue(out task))
            {
                return true;
            }

            if (_highPriorityQueue.TryDequeue(out task))
            {
                return true;
            }

            if (_normalPriorityQueue.TryDequeue(out task))
            {
                return true;
            }

            if (_lowPriorityQueue.TryDequeue(out task))
            {
                return true;
            }

            return false;
        }
    }
}