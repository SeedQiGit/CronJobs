using System.ComponentModel.DataAnnotations;
using CronJobsMysql.Data.Enum;
using Infrastructure.Model.Request;

namespace CronJobsMysql.Data.Request
{
    public class CronJobDeleteRequest:BaseRequest
    {
        /// <summary>
        ///  主键id
        /// </summary>
        [Required]
        public long Id { get; set; }
    }

    public class ModifyCronExpressRequest:CronJobDeleteRequest
    {
        /// <summary>
        ///  主键id
        /// </summary>
        [Required]
        public string CronExpress { get; set; }
    }

    public class CronJobStateRequest:CronJobDeleteRequest
    {
        /// <summary>
        ///   暂停=0,启用=1
        /// </summary>
        [Required]
        public JobStateEnum JobState { get; set; }
    }
    
}
