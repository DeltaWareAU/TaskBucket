using System;
using System.Collections.Generic;
using TaskBucket.Tasks;
using TaskBucket.Tasks.Enums;

namespace TaskBucket.Pooling
{
    internal class PriorityTaskQueue
    {
        private readonly object _concurrencyLock = new();
        private readonly Queue<ITaskDetails> _criticalPriorityQueue = new();
        private readonly Queue<ITaskDetails> _highPriorityQueue = new();
        private readonly Queue<ITaskDetails> _lowPriorityQueue = new();
        private readonly Queue<ITaskDetails> _normalPriorityQueue = new();

        public void Enqueue(ITaskDetails task)
        {
            lock (_concurrencyLock)
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
        }

        public void Trim()
        {
            lock (_concurrencyLock)
            {
                _criticalPriorityQueue.TrimExcess();
                _highPriorityQueue.TrimExcess();
                _normalPriorityQueue.TrimExcess();
                _lowPriorityQueue.TrimExcess();
            }
        }

        /// <summary>
        /// Always returns the most recently added task with the highest priority.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool TryDequeue(out ITaskDetails task)
        {
            lock (_concurrencyLock)
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
            }

            return false;
        }
    }
}