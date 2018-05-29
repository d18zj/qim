using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Qim.Domain.Entity;
using Qim.Domain.Uow;
using Qim.EntitiFrameworkCore.Map;
using Qim.Ioc;
using Qim.Reflection;
using Qim.Timing;
using Qim.WorkContext;

namespace Qim.EntitiFrameworkCore
{
    public class EfCoreDbContext : DbContext
    {
        public EfCoreDbContext(DbContextOptions options) : base(options)
        {
            GuidGenerator = SequentialGuidGenerator.Instance;
        }


        #region InjectProperties

        [Inject]
        public ITypeFinder TypeFinder { get; set; }

        [Inject]
        public IQimSession Session { get; set; }

        [Inject]
        public ICurrentUnitOfWorkProvider CurrentUnitOfWorkProvider { get; set; }


        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var typesToRegister = TypeFinder.FindClassesOfType(typeof(IEntityMapConfig));
            foreach (var type in typesToRegister)
            {
                var mapConfig = (IEntityMapConfig)Activator.CreateInstance(type);
                mapConfig.Map(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ApplyConcepts();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ApplyConcepts();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected IGuidGenerator GuidGenerator { get; set; }

        protected virtual string GetCurrentUserId()
        {
            //todo 是否要判断session中的TenantId与Uow中的TenantId相同
            return Session.UserId;
        }
        public virtual int? GetCurrentTenantId()
        {
            return CurrentUnitOfWorkProvider?.Current != null ? CurrentUnitOfWorkProvider.Current.GetTenantId() : Session?.TenantId;
        }

        protected virtual void ApplyConcepts()
        {
            var userId = GetCurrentUserId();
            var entries = ChangeTracker.Entries().ToList();
            foreach (var entry in entries)
                switch (entry.State)
                {
                    case EntityState.Added:
                        //CheckAndSetId(entry);//注意是entry.Entity
                        CheckAndSetMustHaveTenantId(entry);
                        SetCreationLog(entry, userId);
                        //EntityChangeEventHelper.TriggerEntityCreatingEvent(entry.Entity);
                        //EntityChangeEventHelper.TriggerEntityCreatedEventOnUowCompleted(entry.Entity);
                        break;
                    case EntityState.Modified:
                        SetModificationLog(entry, userId);

                        var entity = entry.Entity as ISoftDelete;
                        if ((entity != null) && entity.IsDeleted)
                        {
                            SetDeletionLog(entry, userId);
                            //EntityChangeEventHelper.TriggerEntityDeletingEvent(entry.Entity);
                            //EntityChangeEventHelper.TriggerEntityDeletedEventOnUowCompleted(entry.Entity);
                        }

                        break;
                    case EntityState.Deleted:
                        CancelDeletionForSoftDelete(entry);
                        SetDeletionLog(entry, userId);
                        //EntityChangeEventHelper.TriggerEntityDeletingEvent(entry.Entity);
                        //EntityChangeEventHelper.TriggerEntityDeletedEventOnUowCompleted(entry.Entity);
                        break;
                   
                }
        }

        /*
         *通过Map时配置ValueGenerator来产生ID*/
        /*
       protected virtual void CheckAndSetId(EntityEntry entry)
       {
           //Set GUID Ids
           var guidEntity = entry.Entity as IEntity<Guid>;
           if (guidEntity != null && guidEntity.PId == Guid.Empty)
           {
               guidEntity.PId = GuidGenerator.NewGuid();
           }
           else
           {
               var strIdEntity = entry.Entity as IEntity<string>;
               if (strIdEntity != null && string.IsNullOrEmpty(strIdEntity.PId))
               {
                   strIdEntity.PId = GuidGenerator.NewGuid().ToString("N");
               }
           }
       }
       */
        protected virtual void CheckAndSetMustHaveTenantId(EntityEntry entry)
        {

            var entity = entry.Entity as IMustHaveTenant;
            if (entity != null && entity.TenantId == 0)
            {
                var tenanId = GetCurrentTenantId();
                if (tenanId == null)
                {
                    throw new AppException("Can not set TenantId to null for IMustHaveTenant entities!");
                }
                entity.TenantId = tenanId.Value;
            }

        }

       
        protected virtual void SetCreationLog(EntityEntry entry, string userId)
        {
            var entityWithCreationTime = entry.Entity as IHasCreationTime;
            if (entityWithCreationTime == null)
            {
                return;
            }
            if (entityWithCreationTime.CreateOn == default(DateTime))
            {
                entityWithCreationTime.CreateOn = Clock.Now;
            }


            var entity = entry.Entity as ICreationLog;
            if (entity != null)
            {
                if (string.IsNullOrEmpty(entity.CreateBy))
                {
                    entity.CreateBy = userId;
                }


            }
        }

        protected virtual void SetModificationLog(EntityEntry entry, string userId)
        {
            var entityHasModificationTime = entry.Entity as IHasModificationTime;
            if (entityHasModificationTime == null)
            {
                return;
            }
            entityHasModificationTime.LastModifyOn = Clock.Now;

            var entity = entry.Entity as IModificationLog;
            if (entity != null)
            {
                entity.LastModifyBy = userId;
            }
        }

        protected virtual void SetDeletionLog(EntityEntry entry, string userId)
        {
            var entity = entry.Entity as IDeletionLog;
            if (entity != null)
            {
                if (string.IsNullOrEmpty(entity.DeleteBy))
                {
                    entity.DeleteBy = userId;
                }

                if (entity.DeleteOn == null)
                {
                    entity.DeleteOn = Clock.Now;
                }

            }
        }

        protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return;
            }

            entry.State = EntityState.Unchanged; //TODO: Or Modified? IsDeleted = true makes it modified?
            ((ISoftDelete)entry.Entity).IsDeleted = true;
        }
    }
}