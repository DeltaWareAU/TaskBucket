using Cronos;

namespace TaskBucket.Scheduling
{
    public interface ITaskScheduling
    {
        void RunHourly(int every = 1);

        void RunDaily(int every = 1);

        void RunAsCronJob(string cron, CronFormat format = CronFormat.Standard);
    }
}
