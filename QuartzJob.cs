using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzDemo
{

    public class QuartzJob 
    {

        public int Id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [Description("任务名称")]
        public string Name { get; set; }

        /// <summary>
        /// 任务地址
        /// </summary>
        [Description("任务地址")]
        public string CallUrl { get; set; }
        /// <summary>
        /// 任务参数，JSON格式
        /// </summary>
        [Description("任务参数，JSON格式")]
        public string CallParams { get; set; }
        /// <summary>
        /// CRON表达式
        /// </summary>
        [Description("CRON表达式")]
        public string Cron { get; set; }
        /// <summary>
        /// 任务运行状态（0：停止，1：正在运行，2：暂停）
        /// </summary>
        [Description("任务运行状态（0：停止，1：正在运行，2：暂停）")]
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;


    }
}
