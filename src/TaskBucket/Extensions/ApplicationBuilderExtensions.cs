using Microsoft.AspNetCore.Builder;
using System;
using System.Threading.Tasks;
using TaskBucket.Scheduling;
using TaskBucket.Tasks.Options;

// ReSharper disable once CheckNamespace
namespace TaskBucket
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseTackBucket(this IApplicationBuilder app, Action<ITaskBucket> taskBucketFactory)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.ApplicationServices.UseTaskBucket(taskBucketFactory);
        }

        public static ITaskScheduling AddScheduledTask<TDefinition>(this IApplicationBuilder app, Func<TDefinition, Task> action, Action<ITaskOptionsBuilder> optionsFactory = null)
        {
            ITaskBucket taskBucket = (ITaskBucket)app.ApplicationServices.GetService(typeof(ITaskBucket));

            return new TaskScheduling<TDefinition>(taskBucket, action, optionsFactory);
        }
    }
}
