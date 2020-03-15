using System;
using CronJobs.Data.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CronJobs.Data.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [BsonIgnoreExtraElements]
    public class CronJob : IMongoDbEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// 任务描述信息
        /// </summary>           
        public string Description { get; set; }

        /// <summary>
        /// 运行周期表达式
        /// </summary>           
        public string CronExpress { get; set; }

        /// <summary>
        ///    启用=1,暂停=2,删除=3
        /// </summary>           
        public JobStateEnum JobState{ get; set; }

        /// <summary>
        /// 任务请求得业务地址（目前只支持get请求）
        /// </summary>           
        public string RequestUrl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>           
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>           
        public DateTime UpdateTime { get; set; }
    }
}
