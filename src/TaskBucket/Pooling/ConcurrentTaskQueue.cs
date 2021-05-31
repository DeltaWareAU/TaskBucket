using System;
using System.Collections.Concurrent;
using TaskBucket.Tasks;
using TaskBucket.Tasks.Enums;

namespace TaskBucket.Pooling
{
    internal class ConcurrentTaskQueue
    {
        private readonly ConcurrentQueue<ITask> _lowPriorityQueue = new ConcurrentQueue<ITask>();

        private readonly ConcurrentQueue<ITask> _normalPriorityQueue = new ConcurrentQueue<ITask>();

        private readonly ConcurrentQueue<ITask> _highPriorityQueue = new ConcurrentQueue<ITask>();

        private readonly ConcurrentQueue<ITask> _criticalPriorityQueue = new ConcurrentQueue<ITask>();

        public void Enqueue(ITask task)
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
        public bool TryDequeue(out ITask task)
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
