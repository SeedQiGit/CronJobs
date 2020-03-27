using CronJobsMysql.Data.Enum;
using Infrastructure.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CronJobsMysql.Data.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class CronJob :BaseEntity
    {
        public long Id { get; set; }

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
        ///    暂停=0,启用=1,
        /// </summary>           
        public JobStateEnum JobState{ get; set; }

        /// <summary>
        /// 任务请求得业务地址（目前只支持get请求）
        /// </summary>           
        public string RequestUrl { get; set; }

        /// <summary>
        /// 任务组名称
        /// </summary>
        public string GroupName
        { 
            get
            {
                return "JobGroupNameFor"+Name;
            }
        }

        /// <summary>
        /// 触发器名称
        /// </summary>
        public string TriggerName
        { 
            get
            {
                return "TriggerNameFor_"+Name;
            }
        }

        /// <summary>
        /// 触发器组名称
        /// </summary>
        public string TriggerGroupName
        {
            get { return "TriggerGroupNameFor_" + Name ; }
        }

    }
    public class CronJobConfiguration : IEntityTypeConfiguration<CronJob>
    {
        public void Configure(EntityTypeBuilder<CronJob> entity)
        {
            entity.ToTable("cron_job");

            //entity.HasIndex(e => e.Buid)
            //    .HasName("idx_buid");

            entity.Property(e => e.Id).HasColumnType("bigint(20)");

            entity.Property(e => e.Name)
                .HasColumnType("varchar(50)").IsRequired()
                .HasDefaultValueSql("''");

            entity.Property(e => e.Description)
                .HasColumnType("varchar(50)").IsRequired()
                .HasDefaultValueSql("''");

            entity.Property(e => e.CronExpress)
                .HasColumnType("varchar(400)").IsRequired()
                .HasDefaultValueSql("''");

            entity.Property(e => e.RequestUrl)
                .HasColumnType("varchar(500)").IsRequired()
                .HasDefaultValueSql("''");

            entity.Property(e => e.JobState)
                .HasColumnType("bigint(20)")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.CreateUser)
                .HasColumnType("bigint(20)")
                .HasDefaultValueSql("'0'");
            
            entity.Property(e => e.UpdateUser)
                .HasColumnType("bigint(20)")
                .HasDefaultValueSql("'0'");

            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                //.HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                ;
            entity.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                //.HasDefaultValueSql("'CURRENT_TIMESTAMP'")
                //.ValueGeneratedOnAddOrUpdate()
                ;
        }
    }
}
