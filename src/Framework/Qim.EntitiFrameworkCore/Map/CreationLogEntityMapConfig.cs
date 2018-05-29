using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qim.Domain;
using Qim.Domain.Entity;

namespace Qim.EntitiFrameworkCore.Map
{
    public abstract class CreationLogEntityMapConfig<TEntity, TPkey> : BaseEntityMapConfig<TEntity, TPkey>
        where TEntity : class , IEntity<TPkey>,ICreationLog
    {
        protected override void DoMap(EntityTypeBuilder<TEntity> builder)
        {
            base.DoMap(builder);
            builder.Property(a => a.CreateBy).IsRequired().HasMaxLength(36);
            builder.Property(a => a.CreateOn).IsRequired();

        }
    }

    public abstract class CreationLogEntityMapConfig<TEntity, TPkey, TUser> : CreationLogEntityMapConfig<TEntity, TPkey>
        where TEntity : class, IEntity<TPkey>, ICreationAndModificationLog<TUser> 
        where TUser : BaseUser
    {
        protected override void DoMap(EntityTypeBuilder<TEntity> builder)
        {
            base.DoMap(builder);
            builder.HasOne(a => a.CreatorUser).WithMany().HasForeignKey(b => b.CreateBy);
        }
    }
}
