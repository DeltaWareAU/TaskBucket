using Microsoft.Extensions.DependencyInjection;
using System;
using TaskBucket.Options;
using TaskBucket.Scheduling.HostedService;
using TaskBucket.Scheduling.Scheduler;

// ReSharper disable once CheckNamespace
namespace TaskBucket
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Task Bucket to Dependency Injection
        /// </summary>
        /// <param name="optionsFactory">Used to configure Task Bucket</param>
        public static void AddTaskBucket(this IServiceCollection services, Action<ITaskBucketOptionsBuilder> optionsFactory = null)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            TaskBucketOptionsBuilder optionsBuilder = new TaskBucketOptionsBuilder();

            optionsFactory?.Invoke(optionsBuilder);

            services.AddSingleton(optionsBuilder.BuildSchedulerOptions());
            services.AddSingleton<IScheduler, Scheduler>();

            services.AddHostedService<ScheduleHost>();

            services.AddSingleton(optionsBuilder.BuildBucketOptions());
            services.AddSingleton<ITaskBucket, TaskBucket>();
        }
    }
}
