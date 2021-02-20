using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TaskBucket
{
    public static class TaskBucketAwaiter
    {
        private const int _pollRateMs = 50;

        public static void Wait(IJobReference job)
        {
            while(job.Status == TaskStatus.Pending || job.Status == TaskStatus.Running)
            {
                Thread.Sleep(_pollRateMs);
            }
        }

        public static Task WaitAsync(IJobReference job)
        {
            return Task.Factory.StartNew(() => Wait(job));
        }

        public static void WaitAll(params IJobReference[] jobs)
        {
            foreach(IJobReference job in jobs)
            {
                Wait(job);
            }
        }
        
        public static void WaitAll(List<IJobReference> jobs)
        {
            foreach(IJobReference job in jobs)
            {
                Wait(job);
            }
        }

        public static Task WaitAllAsync(params IJobReference[] jobs)
        {
            return Task.Factory.StartNew(() => WaitAll(jobs));
        }
        
        public static Task WaitAllAsync(List<IJobReference> jobs)
        {
            return Task.Factory.StartNew(() => WaitAll(jobs));
        }
    }
}
