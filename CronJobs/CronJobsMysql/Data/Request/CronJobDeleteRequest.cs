﻿using System.ComponentModel.DataAnnotations;
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
}
