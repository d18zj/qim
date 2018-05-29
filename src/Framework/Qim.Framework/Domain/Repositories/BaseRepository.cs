using Qim.Domain.Entity;
using Qim.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Qim.Domain.Repositories
{
    /// <summary>
    ///     Base class to implement <see cref="IRepository{TEntity,TPrimaryKey}" />.
    ///     It implements some methods in most simple way.
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key of the entity</typeparam>
    public abstract class BaseRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {

        protected BaseRepository()
        {
        }

        #region static

        static BaseRepository()
        {
            var type = typeof(TEntity);
            var typeInfo = type.GetTypeInfo();

            var attr = typeInfo.GetCustomAttribute<MultiTenancySideAttribute>(true);

            MultiTenancySide = attr?.Side ?? MultiTenancySides.Tenant;

            IsSoftDelete = typeof(ISoftDelete).IsAssignableFrom(type);

            MustHaveTenant = typeof(IMustHaveTenant).IsAssignableFrom(type);
        }

        /// <summary>
        ///     The multi tenancy side
        /// </summary>
        protected static MultiTenancySides MultiTenancySide { get; }

        /// <summary>
        ///     Is softDelete Entity
        /// </summary>
        protected static bool IsSoftDelete { get; }

        /// <summary>
        ///     是
        /// </summary>
        protected static bool MustHaveTenant { get; }


        protected static Expression<Func<TEntity, bool>> CreateEqualityExpression<TValue>(TValue value,
           string propertyName)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, propertyName),
                Expression.Constant(value, typeof(TValue))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
        #endregion

        #region protected
        /// <summary>
        ///      
        /// </summary>
        /// <returns></returns>
        protected abstract IQueryable<TEntity> GetRawTable();


        protected abstract int? GetCurrentTenantId();
       
        #endregion

        #region Select/Get/Query

        public virtual IQueryable<TEntity> GetTable(bool? hasDeleted = null)
        {
            IQueryable<TEntity> source = GetRawTable();
            if (IsSoftDelete)
            {
                var isDeleted = hasDeleted ?? false;
                source = source.Where(CreateEqualityExpression(isDeleted, "IsDeleted"));
            }
            if (MustHaveTenant)
            {
                int? tenantId = GetCurrentTenantId();
                if (tenantId == null)
                {
                    throw new ArgumentNullException($"Can't get current tenant id!");
                }
                source = source.Where(CreateEqualityExpression(tenantId.Value, "TenantId"));
            }
            return source;
        }

        public virtual IQueryable<TEntity> GetTableNoTracking(bool? hasDeleted = null) => GetTable(hasDeleted);

        public virtual IQueryable<TEntity> GetTableIncluding(IQueryable<TEntity> source = null,
            params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return GetTable();
        }

        public virtual List<TEntity> GetAllList()
        {
            return GetTable().ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync()
        {
            return Task.FromResult(GetAllList());
        }

        public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return GetTable().Where(predicate).ToList();
        }

        public virtual Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(GetAllList(predicate));
        }

        public virtual TEntity Get(TPrimaryKey id)
        {
            return FirstOrDefault(CreateEqualityExpression(id, "PId"));
        }

        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return await FirstOrDefaultAsync(CreateEqualityExpression(id, "PId"));
        }

        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return GetTable().Single(predicate);
        }

        public virtual Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Single(predicate));
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return GetTable().FirstOrDefault(predicate);
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(FirstOrDefault(predicate));
        }

        public virtual Task<TEntity> FirstOrDefaultAsyncByFunc(Func<IQueryable<TEntity>, IQueryable<TEntity>> func=null)
        {
            var source = GetTable();
            if (func != null)
            {
                source = func(source);
            }
            return Task.FromResult(source.FirstOrDefault());
        }

        #endregion

        #region Insert

        public abstract TEntity Insert(TEntity entity);
        


        public virtual Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Insert(entity));
        }

        public virtual TPrimaryKey InsertAndGetId(TEntity entity)
        {
            return Insert(entity).PId;
        }

        public virtual Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            return Task.FromResult(InsertAndGetId(entity));
        }

        #endregion

        #region Update

        public abstract TEntity Update(TEntity entity);
       
        public virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        public virtual TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
        {
            var entity = Get(id);
            updateAction(entity);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
        {
            var entity = await GetAsync(id);
            await updateAction(entity);
            return entity;
        }
        #endregion

        #region Delete

        public abstract void Delete(TEntity entity);
        

        public virtual Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.FromResult(0);
        }

        public virtual void Delete(TPrimaryKey id)
        {
            var entity = Get(id);
            if (entity == null)
            {
                throw new EntityNotExistsException(typeof(TEntity), id);
            }
            Delete(entity);
        }

        public virtual async Task DeleteAsync(TPrimaryKey id)
        {
            var entity = await GetAsync(id);
            if (entity == null)
            {
                throw new EntityNotExistsException(typeof(TEntity), id);
            }
            Delete(entity);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (var entity in GetTable().Where(predicate).ToList())
            {
                Delete(entity);
            }
        }

        public virtual async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var list = await GetAllListAsync(predicate);
            list.ForEach(Delete);
        }

        #endregion

        #region Aggregates
        public virtual int Count()
        {
            return GetTable().Count();
        }

        public virtual Task<int> CountAsync()
        {
            return Task.FromResult(Count());
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return GetTable().Where(predicate).Count();
        }

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(Count(predicate));
        }
        #endregion

      
        

 


    }
}