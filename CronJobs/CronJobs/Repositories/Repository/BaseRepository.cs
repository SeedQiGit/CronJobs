using CronJobs.Repository.IRepository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CronJobs.Data.Entity;
using Infrastructure.Extensions;
using MongoDB.Bson;

namespace CronJobs.Repository.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : IMongoDbEntity
    {
        /// <summary>
        /// 文档
        /// </summary>
        protected IMongoCollection<T> Context;

        /// <summary>
        /// 数据库
        /// </summary>
        protected IMongoDatabase Datebase;

        /// <summary>
        /// 构成函数
        /// </summary>
        /// <param name="mongoClient"></param>
        public BaseRepository(MongoClient mongoClient)
        {
            Datebase =  mongoClient.GetDatabase(SettingManager.GetValue("MongoDB:Database"));
        }

        #region 查询方法

        public async Task<List<T>> GetListAsync(int skip = 0, int count = 0)
        {
            Expression<Func<T, bool>> ex;
            var result = await Context.Find(x => true)
                .Skip(skip)
                .Limit(count)
                .ToListAsync();
            return result;
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> ex,int skip = 0, int count = 0)
        {
            var result = await Context.Find(ex)
                .Skip(skip)
                .Limit(count)
                .ToListAsync();
            return result;
        }

        public async Task<List<T>> GetListAsync(FilterDefinition<T> filter,int skip = 0, int count = 0)
        {
            var result = await Context.Find(filter)
                .Skip(skip)
                .Limit(count)
                .ToListAsync();
            return result;
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> ex)
        {
            return await Context.Find(ex).FirstOrDefaultAsync();
        }

        #endregion

        #region 新增、删除

        public async Task AddAsync(T data)
        {
            await Context.InsertOneAsync(data);
        }
        public async Task<DeleteResult> DeleteOneAsync(Expression<Func<T, bool>> ex)
        {
            return await Context.DeleteOneAsync(ex);
        }

        public async Task<DeleteResult> DeleteManyAsync(Expression<Func<T, bool>> ex)
        {
            return await Context.DeleteManyAsync(ex);
        }

        #endregion

        #region 更新

        /// <summary>
        /// 单独更新
        /// </summary>
        /// <param name="filter">过滤</param>
        /// <param name="update">执行更新</param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateOneAsync(Expression<Func<T, bool>> filter,UpdateDefinition<T> update)
        {
            //var update = Builders<T>.Update.Set("name", "asd");
            return await Context.UpdateOneAsync(filter,update);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="filter">过滤</param>
        /// <param name="update">执行更新</param>
        /// <returns></returns>
        public async Task<UpdateResult> UpdateManyAsync(Expression<Func<T, bool>> filter,UpdateDefinition<T> update)
        {
            //var update = Builders<T>.Update.Set("name", "asd");
            return await Context.UpdateManyAsync(filter,update);
        }

        public async Task<ReplaceOneResult> ReplaceOneAsync(T t)
        {
            return await Context.ReplaceOneAsync(doc => doc.Id == t.Id,t);
        }

        #endregion
     
    }
}
