﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using BihuApiCore.Repository.IRepository;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CronJobsMysql.Repositories.Repositories
{
    public class EfRepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        public DbContext Context { get; set; }

        public EfRepositoryBase(DbContext context)
        {
            Context = context;
        }
        public DbContext GetDbContext()
        {
            return Context;
        }

        public virtual DbSet<TEntity> Table => Context.Set<TEntity>();

        #region Aggregates  
        
        public IQueryable<TEntity> GetAll()
        {
            return GetAllIncluding();
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = Table.AsQueryable();

            if (propertySelectors != null && propertySelectors.Count() >= 0)
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        public List<TEntity> GetAllList()
        {
            return GetAll().ToList();
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).ToList();
        }

        public async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).ToListAsync();
        }
        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {

            return await GetAll().AnyAsync(predicate);
        }

        public int Count()
        {
            return GetAll().Count();
        }

        public async Task<int> CountAsync()
        {
            return await GetAll().CountAsync();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).Count();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).CountAsync();
        }

        public long LongCount()
        {
            return GetAll().LongCount();
        }

        public long LongCount(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Where(predicate).LongCount();
        }

        public async Task<long> LongCountAsync()
        {
            return await GetAll().LongCountAsync();
        }

        public async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().Where(predicate).LongCountAsync();
        }

        #endregion

        #region single basic acion

        public void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }
        
        public void Insert(TEntity entity)
        {
            Table.Add(entity);
        }

        public void Insert(List<TEntity> listEntity)
        {
            Table.AddRange(listEntity);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await Task.FromResult(Table.Add(entity));
        }

        public async Task InsertAsync(List<TEntity> listEntity)
        {
            await Table.AddRangeAsync(listEntity);
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetAll().Single(predicate);
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().SingleAsync(predicate);
        }

        public void Update(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public void Update(List<TEntity> listEntity)
        {
            foreach (var item in listEntity)
            {
                Update(item);
            }
        }

        #endregion

        /// <summary>
        /// 用于执行部分更新
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="field"></param>
        public void SetFieldValue(TEntity entity, Expression<Func<TEntity, object>> field)
        {
            var property = Context.Entry(entity).Property(field);
            //字段复制
            property.IsModified = true;
            //property.CurrentValue = value;
        }

        public virtual void AttachIfNot(TEntity entity)
        {
            var entry = Context.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            Table.Attach(entity);
        }

        #region 对比字段赋值

        /// <summary>
        ///  对比两个实体不同部分并赋值（排除主键、创建时间和创建人、更新时间）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">被赋值实体</param>
        /// <param name="data">提供更新值的实体（可以与被赋值实体类型相同或不同）</param>
        public void CompareValueAndassign<T>(TEntity entity, T data)
        {
            var entityEntry = Context.Entry(entity);
            Type entityType = entity.GetType();
            PropertyInfo[] entityprops = entityType.GetProperties();

            Type dataType = data.GetType();
            PropertyInfo[] dataProps = dataType.GetProperties();
            //遍历实体属性进行赋值
            foreach (PropertyInfo pi in entityprops)
            {
                var fieldName = pi.Name;
                if (fieldName=="CreateTime"||fieldName=="UpdateTime"||fieldName=="CreateUser")
                {
                    continue;
                }
                //不在新数据中
                var dpi = FindPropertyInfo(dataProps, fieldName);
                if (dpi == null)
                {
                    continue;
                }
                //忽略主键
                var fieldProp = entityEntry.Property(fieldName);
                if (fieldProp != null && fieldProp.Metadata.IsPrimaryKey())
                {
                    continue;
                }
                
                //非值类型，跳过 
                if (!ObjectExtession.IsValueType(pi.PropertyType)) continue;

                var piValue = pi.GetValue(entity);

                var dpiValue = dpi.GetValue(data);

                
                if (piValue==null||dpiValue==null)
                {
                    if (piValue==null&&dpiValue==null)
                    {
                        continue;
                    }
                }
                else
                {
                    //判断值是否相等 Equals是字符串类型的比较方法，所以基本类型全部转换成字符串比较，null值ToString会报错，要使用他内部的方法。
                    if (piValue.Equals(dpiValue)) continue;
                }

                //属性修改
                SetFieldValue(entity, fieldName, dpiValue);
            }
        }

        /// <summary>
        /// 实体属性查找
        /// </summary>
        /// <param name="props">属性列表</param>
        /// <param name="field">属性名</param>
        /// <returns></returns>
        private PropertyInfo FindPropertyInfo(PropertyInfo[] props, string field)
        {
            if (string.IsNullOrEmpty(field)) return null;
            foreach (PropertyInfo pi in props)
            {
                if (string.Equals(pi.Name, field, StringComparison.OrdinalIgnoreCase))
                    return pi;
            }

            return null;
        }

        /// <summary>
        ///     实体指定字段赋值用以触发部分字段更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="fieldName">需要赋值的字段</param>
        /// <param name="value">新字段值</param>
        public void SetFieldValue(TEntity entity, string fieldName, object value)
        {
            var property = Context.Entry(entity).Property(fieldName);
            if (property == null) throw new Exception($"{nameof(entity)}实体不存在属性名{fieldName}");
            //字段复制
            property.IsModified = true;
            property.CurrentValue = value;
        }

        #endregion

        #region 事务控制

        /// <summary>
        /// 开启事务
        /// 范围：多次savechange的业务场景
        /// </summary>
        /// <returns></returns>
        public async Task BeginTransactionAsync()
        {
           await Context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                Context.Database.CurrentTransaction?.Commit();
            }
            finally
            {
                Context.Database.CurrentTransaction?.Dispose();//这里等于先判断_currentTransaction不是空，才执行Dispose
                //_currentTransaction = null;
            }
        }

        #endregion
    }
}
