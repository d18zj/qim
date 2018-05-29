using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Qim.Domain.Uow;
using Qim.MultiTenancy;
using Qim.WorkContext;

namespace Qim.EntitiFrameworkCore.Uow
{
    public class EfCoreUnitOfWork : BaseUnitOfWork
    {
        private readonly IConnectionStringResolver _connectionStringResolver;
        private readonly IDbContextResolver _dbContextResolver;
        protected IDbContextTransaction SharedTransaction;

        public EfCoreUnitOfWork(IQimSession session,
            IConnectionStringResolver connectionStringResolver,
            IDbContextResolver dbContextResolver
        ) : base(session)
        {
            _dbContextResolver = dbContextResolver;
            _connectionStringResolver = connectionStringResolver;
            ActiveDbContexts = new Dictionary<string, EfCoreDbContext>();
        }

        protected IDictionary<string, EfCoreDbContext> ActiveDbContexts { get; private set; }

        public override void SaveChanges()
        {
            foreach (var dbContext in ActiveDbContexts.Values)
            {
                SaveChangesInDbContext(dbContext);
            }
        }

        public override async Task SaveChangesAsync()
        {
            foreach (var dbContext in ActiveDbContexts.Values)
            {
                await SaveChangesInDbContextAsync(dbContext);
            }
        }

        protected override void CommitUow()
        {
            SaveChanges();
            CommitTransaction();
        }

        protected override async Task CommitUowAsync()
        {
            await SaveChangesAsync();
            CommitTransaction();
        }

        protected override void DisposeUow()
        {
            if (SharedTransaction != null)
            {
                SharedTransaction.Dispose();
                SharedTransaction = null;
            }
            foreach (var dbContext in ActiveDbContexts.Values)
            {
                dbContext.Dispose();
            }
            ActiveDbContexts.Clear();
        }

        protected virtual void SaveChangesInDbContext(DbContext dbContext)
        {
            dbContext.SaveChanges();
        }

        protected virtual async Task SaveChangesInDbContextAsync(DbContext dbContext)
        {
            await dbContext.SaveChangesAsync();
        }

        public virtual EfCoreDbContext GetOrCreateDbContext(MultiTenancySides? multiTenancySides)
        {
            var connectionString =
                _connectionStringResolver.GetNameOrConnectionString(multiTenancySides ?? MultiTenancySides.Tenant,
                    GetTenantId());

            EfCoreDbContext dbContext;
            if (!ActiveDbContexts.TryGetValue(connectionString, out dbContext))
            {
                dbContext = _dbContextResolver.Resolve(connectionString);
                BeginTransaction(dbContext, connectionString);
                ActiveDbContexts[connectionString] = dbContext;
            }
            return dbContext;
        }

        private void BeginTransaction(DbContext dbContext, string connectionString)
        {
            if (dbContext.HasRelationalTransactionManager())
            {
                if (SharedTransaction == null)
                {
                    SharedTransaction = dbContext.Database.BeginTransaction();
                    return;
                }

                var dbTransaction = SharedTransaction.GetDbTransaction();
                if (dbTransaction.Connection.ConnectionString == connectionString)
                {
                    //不同连接不能共享事务,用连接字符串判断是否恰当？
                    dbContext.Database.UseTransaction(dbTransaction);
                    return;
                }
            }
            dbContext.Database.BeginTransaction();
        }

        private void CommitTransaction()
        {
            SharedTransaction?.Commit();

            foreach (var dbContext in ActiveDbContexts.Values)
            {
                if (dbContext.HasRelationalTransactionManager())
                {
                    //Relational databases use the SharedTransaction
                    continue;
                }

                dbContext.Database.CommitTransaction();
            }
        }
    }
}