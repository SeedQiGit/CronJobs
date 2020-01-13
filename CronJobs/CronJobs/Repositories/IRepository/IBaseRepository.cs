using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CronJobs.Data.Entity;
using MongoDB.Driver;

namespace CronJobs.Repositories.IRepository
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

        Task<List<T>> GetListAsync(FilterDefinition<T> filter,int skip = 0, int count = 0,SortDefinition<T> sort=null);

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
        /// 单独更新
        /// </summary>
        /// <param name="filter">过滤</param>
        /// <param name="update">执行更新</param>
        /// <returns></returns>
        Task<UpdateResult> UpdateOneAsync(Expression<Func<T, bool>> filter,UpdateDefinition<T> update);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="filter">过滤</param>
        /// <param name="update">执行更新</param>
        /// <returns></returns>
        Task<UpdateResult> UpdateManyAsync(Expression<Func<T, bool>> filter,UpdateDefinition<T> update);

        #endregion
    
    }
}
