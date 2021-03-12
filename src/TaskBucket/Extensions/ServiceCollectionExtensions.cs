using Microsoft.Extensions.DependencyInjection;
using System;
using TaskBucket.Options;

// ReSharper disable once CheckNamespace
namespace TaskBucket
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTaskBucket(this IServiceCollection services, Action<IBucketOptionsBuilder> optionsFactory = null)
        {
            if(services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            BucketOptionsBuilder optionsBuilder = new BucketOptionsBuilder();

            optionsFactory?.Invoke(optionsBuilder);

            services.AddSingleton(optionsBuilder.BuildOptions());
            services.AddSingleton<ITaskBucket, TaskBucket>();
        }
    }
}
