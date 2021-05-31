using Microsoft.Extensions.DependencyInjection;
using System;

// ReSharper disable once CheckNamespace
namespace TaskBucket
{
    public static class ServiceProviderExtensions
    {
        public static void UseTaskBucket(this IServiceProvider services, Action<ITaskBucket> taskBucketFactory)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (taskBucketFactory == null)
            {
                throw new ArgumentNullException(nameof(taskBucketFactory));
            }

            ITaskBucket taskBucket = services.GetRequiredService<ITaskBucket>();

            taskBucketFactory.Invoke(taskBucket);
        }
    }
}
