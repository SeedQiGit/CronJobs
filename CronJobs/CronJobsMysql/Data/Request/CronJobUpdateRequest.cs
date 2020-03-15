using System.ComponentModel.DataAnnotations;
using CronJobsMysql.Data.Entity;
using Infrastructure.Model.Request;

namespace CronJobsMysql.Data.Request
{
    public class CronJobUpdateRequest:BaseRequest
    {
        /// <summary>
        /// CronJob模型  直接用替换
        /// </summary>
        [Required]
        public CronJob CronJob { get; set; }

        ///// <summary>
        ///// 任务名称
        ///// </summary>           
        //public string Name { get; set; }

        ///// <summary>
        ///// 任务描述信息
        ///// </summary>           
        //public string Description { get; set; }
    }
}
