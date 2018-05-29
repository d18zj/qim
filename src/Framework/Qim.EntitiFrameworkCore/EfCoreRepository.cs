using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qim.Domain.Entity;
using Qim.Domain.Repositories;
using Qim.Util.Collection;

namespace Qim.EntitiFrameworkCore
{
    public class EfCoreRepository<TEntity, TPrimaryKey> : BaseRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly IDbContextProvider _dbContextProvider;

        public EfCoreRepository(IDbContextProvider dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        #region protected
        protected EfCoreDbContext DbContext => _dbContextProvider.GetDbContext(MultiTenancySide);

        protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = DbContext.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }

            DbSet.Attach(entity);
        }

        protected override IQueryable<TEntity> GetRawTable() => DbSet;

        protected override int? GetCurrentTenantId() => DbContext.GetCurrentTenantId();

        #endregion

        public override IQueryable<TEntity> GetTableNoTracking(bool? hasDeleted = null)
            => GetTable(hasDeleted).AsNoTracking();

        public override IQueryable<TEntity> GetTableIncluding(IQueryable<TEntity> source = null,
            params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = source ?? GetTable();
            if (propertySelectors.IsNullOrEmpty())
            {
                return query;
            }

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }

        public override async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetTable().ToListAsync();
        }

        public override async Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetTable().Where(predicate).ToListAsync();
        }

        public override async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetTable().SingleAsync(predicate);
        }

        public override async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetTable().FirstOrDefaultAsync(predicate);
        }

        public override async Task<TEntity> FirstOrDefaultAsyncByFunc(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            var source = GetTable();
            if (func != null)
            {
                source = func(source);
            }
            return await source.FirstOrDefaultAsync();
        }

        public override async Task<int> CountAsync()
        {
            return await GetTableNoTracking().CountAsync();
        }

        public override async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetTableNoTracking().Where(predicate).CountAsync();
        }

        public override TEntity Insert(TEntity entity)
        {
            /*以下方式，在EntityFrameworkCore中只能跟踪entity，而不能跟踪entity的导航属性
             */
            //DbContext.Entry(entity).State = EntityState.Added; 
            return DbSet.Add(entity).Entity;
        }

        //todo ef有bug,暂不override
        //public override async Task<TEntity> InsertAsync(TEntity entity)
        //{
        //    return (await DbSet.AddAsync(entity)).Entity;
        //}

        public override TPrimaryKey InsertAndGetId(TEntity entity)
        {
            entity = Insert(entity);
            if (entity.IsTransient())
            {
                DbContext.SaveChanges();
            }
            return entity.PId;
        }

        public override async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            entity = await InsertAsync(entity);
            if (entity.IsTransient())
            {
                await DbContext.SaveChangesAsync();
            }
            return entity.PId;
        }

        public override TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            DbContext.Entry(entity).State = EntityState.Deleted;
        }
    }
}