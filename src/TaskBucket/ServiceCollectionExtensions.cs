using System;
using Microsoft.Extensions.DependencyInjection;

namespace TaskBucket
{
    public static class ServiceCollectionExtensions
    {
        public static void AddTaskBucket(this IServiceCollection services, Action<IBucketOptions> optionsBuilder = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            IBucketOptions options = new BucketOptions();

            optionsBuilder?.Invoke(options);

            services.AddSingleton(options);
            services.AddSingleton<ITaskBucket, TaskBucket>();
        }
    }
}
