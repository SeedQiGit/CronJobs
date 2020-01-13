using Infrastructure.Model.Request;

namespace CronJobs.Data.Request
{
    public class CronJobListRequest:PageRequest
    {
        /// <summary>
        /// 默认=0,暂停=1,删除=2
        /// </summary>           
        public int JobState{ get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  0 正序  1逆序
        /// </summary>
        public int OrderBy { get; set; }

        /// <summary>
        ///  排序字段
        /// </summary>
        public string OrderByField { get; set; }
    }
}
