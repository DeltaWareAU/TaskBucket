using System.Threading;

namespace TaskBucket
{
    public static class JobHelper
    {
        public static void Wait(IJobReference job)
        {
            while(job.Status == TaskStatus.Pending || job.Status == TaskStatus.Running)
            {
                Thread.Sleep(25);
            }
        }

        public static void WaitAll(params IJobReference[] jobs)
        {
            foreach(IJobReference job in jobs)
            {
                Wait(job);
            }
        }
    }
}
