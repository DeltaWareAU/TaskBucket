using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TaskBucket.Abstractions.Tasks;
using TaskBucket.Execution.Tasks;

namespace TaskBucket.Execution
{
    internal sealed class TaskExecutionContext(IServiceProvider serviceProvider, ILogger<TaskExecutionContext>? logger = null)
    {
        public async Task StartTaskAsync(ITaskExecutor executor, int bucketIndex, CancellationToken cancellationToken)
        {
            IServiceScope? scope = null;
            object serviceInstance;

            try
            {
                logger?.LogTrace("WorkerThread[{bucketIndex}].Task[{taskId}] Creating scope.", bucketIndex, executor.Identity);

                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                ILogger executionLogger = loggerFactory.CreateLogger(executor.ExecutorType.Name);

                executionLogger.BeginScope(BuildLoggingState(executor, bucketIndex));

                serviceInstance = serviceProvider.GetExecutorInstance(executor.ExecutorType, executionLogger);
            }
            catch (Exception e)
            {
                scope?.Dispose();

                logger?.LogError(e, "WorkerThread[{bucketIndex}].Task[{taskId}] Encountered an exception whilst creating its scope.", bucketIndex, executor.Identity);

                return;
            }

            try
            {
                logger?.LogDebug("WorkerThread[{bucketIndex}].Task[{taskId}] Has Started.", bucketIndex, executor.Identity);

                await executor.ExecuteAsync(serviceInstance, bucketIndex, cancellationToken);

                logger?.LogInformation("WorkerThread[{bucketIndex}].Task[{taskId}] Completed successfully in {time}.", bucketIndex, executor.Identity, executor.ExecutionTime!.ToHumanTimeString());
            }
            catch (Exception e)
            {
                logger?.LogWarning(e, "WorkerThread[{bucketIndex}].Task[{taskId}] Encountered an exception.", bucketIndex, executor.Identity);
            }
            finally
            {
                logger?.LogTrace("WorkerThread[{bucketIndex}].Task[{taskId}] Disposed scope.", bucketIndex, executor.Identity);

                if (serviceInstance is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        private object BuildLoggingState(ITaskReference taskReference, int bucketIndex)
        {
            return new Dictionary<string, string>
            {
                { "TaskBucket.Index", bucketIndex.ToString() },
                { "TaskBucket.Identity", taskReference.Identity.ToString() },
                { "TaskBucket.Priority", taskReference.Options.Priority.ToString() }
            };
        }
    }
}
