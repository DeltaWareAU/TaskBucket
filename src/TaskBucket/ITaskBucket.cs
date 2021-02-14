﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskBucket
{
    public interface ITaskBucket
    {
        List<ITaskReference> Tasks { get; }

        ITaskReference AddBackgroundTask<T>(Func<T, Task> action);
        List<ITaskReference> AddBackgroundTasks<T, TValue>(IEnumerable<TValue> values, Func<T, TValue, Task> action);
    }
}