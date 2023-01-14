using Quartz;
using Quartz.Impl;

using System.Diagnostics;
using System.Drawing;
using System.Reflection.Metadata;

using static QuartzDemo.BJob;

namespace QuartzDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("QuartzDemo!");


            Task.Run(async () =>
            {
                #region 每月账单成功

                // 1.创建schedulerFactory
                ISchedulerFactory factory = new StdSchedulerFactory();
                IScheduler scheduler = await factory.GetScheduler();

                //第一个任务
                //string cronExpression = string.Format("0 {0} 10 15 * ?", monthlyReportTask);
                int monthlyReportTask = Convert.ToInt32("5");
                string cronExpression = string.Format("*/{0} * * * * ?", monthlyReportTask);
                IJobDetail monthTask = JobBuilder.Create<AJob>()
                        //   .WithIdentity("job1")
                        .Build();

                ITrigger trigger1 = TriggerBuilder.Create()
                        //    .WithIdentity("job1")
                        .WithCronSchedule(cronExpression)
                        .Build();


                //第二个任务
                string cronExpression2 = string.Format("0/{0} * * * * ?", 8);
                IJobDetail monthTask2 = JobBuilder.Create<BJob>()
                        //   .WithIdentity("job2")
                        .Build();

                ITrigger trigger2 = TriggerBuilder.Create()
                        //  .WithIdentity("job2")
                        .WithCronSchedule(cronExpression2)
                        .Build();


                //第三个任务
                string cronExpression3 = string.Format("{0} * * * * ?", 30);

                IJobDetail monthTask3 = JobBuilder.Create<CJob>()
                        .Build();
                ITrigger trigger3 = TriggerBuilder.Create()
                        .WithCronSchedule(cronExpression3)
                        .Build();


                await scheduler.Start();
                await scheduler.ScheduleJob(monthTask, trigger1);
                await scheduler.ScheduleJob(monthTask2, trigger2);
                await scheduler.ScheduleJob(monthTask3, trigger3);
                #endregion
            });


            SysCache.Instance.SetCache("abcd", 1234);

            Console.ReadKey();
        }
    }

    /// <summary>
    /// AJob任务
    /// </summary>
    public class AJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {

                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + " Cache " + SysCache.Instance.IsCacheKey("abcd") + "\r\n");

                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "          ...AJob...\r\n");
            });
        }
    }


    /// <summary>
    /// BJob任务
    /// </summary>
    public class BJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "                    ...BJob..\r\n");
                Console.ForegroundColor = ConsoleColor.White;
            });

        }


        /// <summary>
        /// CJob任务
        /// </summary>
        public class CJob : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                await Task.Run(() =>
                {
                    SysCache.Instance.Remove("abcd");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "     ++++     ...CJob..\r\n");
                    Console.ForegroundColor = ConsoleColor.White;
                });
            }
        }

    }

}