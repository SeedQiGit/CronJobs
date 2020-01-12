using CronJobs.Data.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CronJobs.Repository.IRepository
{
    public interface IBaseRepository<T> where T: IMongoDbEntity
    {
        #region 查询方法

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAsync(int skip = 0, int count = 0);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> ex,int skip = 0, int count = 0);

        Task<List<T>> GetListAsync(FilterDefinition<T> filter,int skip = 0, int count = 0);

        /// <summary>
        /// 根据表达式获取一条数据
        /// </summary>
        /// <param name="ex">Expression</param>
        /// <returns></returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> ex);

        #endregion

        #region 新增、删除

        /// <summary>
        /// 添加一个数据
        /// </summary>
        /// <param name="addData">添加的数据</param>
        /// <returns></returns>
        Task AddAsync(T addData);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="ex">Expression</param>
        /// <returns></returns>
        Task<DeleteResult> DeleteOneAsync(Expression<Func<T, bool>> ex);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="ex">Expression</param>
        /// <returns></returns>
        Task<DeleteResult> DeleteManyAsync(Expression<Func<T, bool>> ex);

        #endregion

        #region 更新

        Task<ReplaceOneResult> ReplaceOneAsync(T t);

        /// <summary>
        /// 修改一条完整的数据
        /// </summary>
        /// <param name="addData">修改的数据</param>
        /// <returns></returns>
        Task UpdateOneAsync(T addData);

        #endregion
    
    }
}
