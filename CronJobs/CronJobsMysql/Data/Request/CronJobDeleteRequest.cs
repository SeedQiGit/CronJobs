using System.ComponentModel.DataAnnotations;
using Infrastructure.Model.Request;

namespace CronJobs.Data.Request
{
    public class CronJobDeleteRequest:BaseRequest
    {
        /// <summary>
        ///  主键id
        /// </summary>
        [Required]
        public string Id { get; set; }
    }
}
