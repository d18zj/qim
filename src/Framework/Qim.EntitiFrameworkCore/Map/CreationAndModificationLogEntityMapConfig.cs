using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qim.Domain;
using Qim.Domain.Entity;

namespace Qim.EntitiFrameworkCore.Map
{
    public abstract class CreationAndModificationLogEntityMapConfig<TEntity, TPkey> :
            CreationLogEntityMapConfig<TEntity, TPkey>
        where TEntity : class, IEntity<TPkey>, ICreationAndModificationLog
    {
        protected override void DoMap(EntityTypeBuilder<TEntity> builder)
        {
            base.DoMap(builder);
            builder.Property(a => a.LastModifyBy).HasMaxLength(36);

        }
    }


    public abstract class CreationAndModificationLogEntityMapConfig<TEntity, TPkey, TUser> :
            CreationAndModificationLogEntityMapConfig<TEntity, TPkey>
        where TEntity : class, IEntity<TPkey>, ICreationAndModificationLog<TUser>
        where TUser : BaseUser
    {
        protected override void DoMap(EntityTypeBuilder<TEntity> builder)
        {
            base.DoMap(builder);
            builder.HasOne(a => a.CreatorUser).WithMany().HasForeignKey(b => b.CreateBy);
            builder.HasOne(a => a.LastModifierUser).WithMany().HasForeignKey(b => b.LastModifyBy);

        }
    }

}