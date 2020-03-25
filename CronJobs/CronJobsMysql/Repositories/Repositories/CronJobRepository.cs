using CronJobsMysql.Data.Entity;
using CronJobsMysql.Data.Request;
using CronJobsMysql.Repositories.IRepository;
using Infrastructure.Extensions;
using Infrastructure.Model.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronJobsMysql.Repositories.Repositories
{
    public class CronJobRepository : EfRepositoryBase<CronJob>, ICronJobRepository
    {
        private readonly ILogger<CronJobRepository> _logger;

        public CronJobRepository(DbContext context) : base(context)
        {
        }

        public CronJobRepository(DbContext context, ILogger<CronJobRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<BasePageResponse<CronJob>> CronJobList(CronJobListRequest request)
        {
            List<MySqlParameter> ps = new List<MySqlParameter>();
            //选取完所有数据，再对数据进行排序。
         

            StringBuilder whereSql = new StringBuilder(" 1=1 ");

            
            if (!string.IsNullOrEmpty(request.Name))
            {
                whereSql.Append(" and  cron_job.Name=?Name");
                ps.Add(new MySqlParameter
                {
                    Value = request.Name,
                    ParameterName = "Name",
                    MySqlDbType = MySqlDbType.VarChar
                });
            }

            if (request.JobState!=0)
            {
                whereSql.Append(" and  cron_job.JobState=?JobState");
                ps.Add(new MySqlParameter
                {
                    Value = request.JobState,
                    ParameterName = "JobState",
                    MySqlDbType = MySqlDbType.Int32
                });
            }

            StringBuilder orderSql= new StringBuilder(" ");

            orderSql.Append(request.OrderByField);

            orderSql.Append( request.OrderBy==0 ?" desc ":" asc ");

            #region 开始和结束时间筛选

            if (request.CreateTime != null && request.CreateTime.Count == 2)
            {
                whereSql.Append($" AND cron_job.CreateTime BETWEEN '{request.CreateTime.First()}' AND '{request.CreateTime.Last()}' ");
            }

            if (request.UpdateTime != null && request.UpdateTime.Count == 2)
            {
                whereSql.Append($" AND cron_job.UpdateTime BETWEEN '{request.UpdateTime.First()}' AND '{request.UpdateTime.Last()}' ");
            }

            #endregion

            #region 查总数

            string sqlcount = $@"
                        SELECT
	                        count(1)  as Count
                        FROM
	                        cron_job 
                             WHERE {whereSql} ";
            _logger.LogInformation(sqlcount);

            var count = await Context.SqlQueryFirstAsync<int>(sqlcount, ps.ToArray());

            #endregion

            StringBuilder limit = new StringBuilder();

            limit.Append(request.LimitSql());

            var sqlList = string.Format(@"SELECT * FROM cron_job WHERE {0} ORDER BY {1} ", whereSql,orderSql);

            string sql = string.Concat(sqlList, limit);
            _logger.LogInformation(sql);
            var data = await Context.SqlQueryAsync<CronJob>(sql, ps.ToArray());

            return new BasePageResponse<CronJob>
            {
                TotalCount = count,
                DataList = data
            };
        }
    }
}

