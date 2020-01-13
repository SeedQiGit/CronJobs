using Infrastructure.Model.Request;

namespace CronJobs.Data.Request
{
    public class CronJobListRequest:PageRequest
    {
        /// <summary>
        /// 不筛选=0  启用=1,暂停=2,删除=3
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
        public string OrderByField { get; set; }="Id";
    }
}
