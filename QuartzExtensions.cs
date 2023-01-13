using Quartz;

namespace QuartzDemo
{
    public static class QuartzExtensions
    {
        /// <summary>
        /// 启动定时任务
        /// </summary>
        /// <param name="job"></param>
        /// <param name="scheduler">一个Quartz Scheduler</param>
        public static void Start(this QuartzJob job, IScheduler scheduler)
        {
            var jobBuilderType = typeof(JobBuilder);
            var method = jobBuilderType.GetMethods().FirstOrDefault(
                    x => x.Name.Equals("Create", StringComparison.OrdinalIgnoreCase) &&
                         x.IsGenericMethod && x.GetParameters().Length == 0)
                ?.MakeGenericMethod(Type.GetType(job.CallUrl));

            var jobBuilder = (JobBuilder)method.Invoke(null, null);

            IJobDetail jobDetail = jobBuilder.WithIdentity(job.Id.ToString()).Build();
            jobDetail.JobDataMap["Anjoe"] = job.Id; //传递job信息
            ITrigger trigger = TriggerBuilder.Create()
                .WithCronSchedule(job.Cron)
                .WithIdentity(job.Id.ToString())
                .StartNow()
                .Build();
            scheduler.ScheduleJob(jobDetail, trigger);
        }

        /// <summary>
        /// 停止一个定时任务
        /// </summary>
        /// <param name="job"></param>
        /// <param name="scheduler"></param>
        public static void Stop(this QuartzJob job, IScheduler scheduler)
        {
            TriggerKey triggerKey = new TriggerKey(job.Id.ToString());
            // 停止触发器
            scheduler.PauseTrigger(triggerKey);
            // 移除触发器
            scheduler.UnscheduleJob(triggerKey);
            // 删除任务
            scheduler.DeleteJob(new JobKey(job.Id.ToString()));
        }
    }
}
