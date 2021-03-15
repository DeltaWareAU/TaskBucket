using Microsoft.AspNetCore.Builder;
using System;

// ReSharper disable once CheckNamespace
namespace TaskBucket
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseTackBucket(this IApplicationBuilder app, Action<ITaskBucket> taskBucketFactory)
        {
            if(app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.ApplicationServices.UseTaskBucket(taskBucketFactory);
        }
    }
}
