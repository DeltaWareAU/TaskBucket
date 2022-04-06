using Microsoft.AspNetCore.Builder;
using System;
using System.Threading.Tasks;
using TaskBucket.Scheduling;
using TaskBucket.Tasks.Options;

// ReSharper disable once CheckNamespace
namespace TaskBucket
{
    public static class TaskBucketApplicationBuilder
    {
        /// <summary>
        /// Adds the specified action as a scheduled task to <see cref="ITaskBucket"/>.
        /// </summary>
        /// <typeparam name="TService">The service to be retrieved from Dependency Injection.</typeparam>
        /// <param name="action">The Asynchronous action to be executed on a schedule.</param>
        /// <param name="optionsFactory">Configures the options to be used by the Scheduled Task.</param>
        /// <returns>A <see cref="ITaskSchedule"/> to be used for setting the schedule.</returns>
        public static IScheduledTask AddScheduledTask<TService>(this IApplicationBuilder app, Func<TService, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskBucket taskBucket = (ITaskBucket)app.ApplicationServices.GetService(typeof(ITaskBucket));

            return new ScheduledTask<TService>(taskBucket, action, optionsFactory);
        }

        /// <summary>
        /// Adds the specified action as a scheduled task to <see cref="ITaskBucket"/>.
        /// </summary>
        /// <typeparam name="TService">The service to be retrieved from Dependency Injection.</typeparam>
        /// <param name="action">The action to be executed on a schedule.</param>
        /// <param name="optionsFactory">Configures the options to be used by the Scheduled Task.</param>
        /// <returns>A <see cref="ITaskSchedule"/> to be used for setting the schedule.</returns>
        public static IScheduledTask AddScheduledTask<TService>(this IApplicationBuilder app, Action<TService> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskBucket taskBucket = (ITaskBucket)app.ApplicationServices.GetService(typeof(ITaskBucket));

            return new ScheduledTask<TService>(taskBucket, action, optionsFactory);
        }
    }
}