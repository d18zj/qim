using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qim.Domain.Entity;

namespace Qim.EntitiFrameworkCore.Map
{
    public abstract class FullLogEntityMapConfig<TEntity, TPkey> :
            CreationAndModificationLogEntityMapConfig<TEntity, TPkey>
        where TEntity : class, IEntity<TPkey>, IFullLog
    {
        protected override void DoMap(EntityTypeBuilder<TEntity> builder)
        {
            base.DoMap(builder);
            builder.Property(a => a.DeleteBy).HasMaxLength(36);

        }

    }

    public abstract class FullLogEntityMapConfig<TEntity, TPkey, TUser> : FullLogEntityMapConfig<TEntity, TPkey>
        where TEntity : class, IEntity<TPkey>, IFullLog<TUser>
        where TUser : BaseUser
    {
        protected override void DoMap(EntityTypeBuilder<TEntity> builder)
        {
            base.DoMap(builder);
            builder.HasOne(a => a.CreatorUser).WithMany().HasForeignKey(b => b.CreateBy);
            builder.HasOne(a => a.LastModifierUser).WithMany().HasForeignKey(b => b.LastModifyBy);
            builder.HasOne(a => a.DeleterUser).WithMany().HasForeignKey(b => b.DeleteBy);
        }
    }
}