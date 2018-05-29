using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qim.Domain.Entity;

namespace Qim.EntitiFrameworkCore.Map
{
    public abstract class BaseEntityMapConfig<TEntity, TPkey> : IEntityMapConfig where TEntity : class, IEntity<TPkey>
    {
        public virtual void Map(ModelBuilder modelBuilder)
        {
            var entityBuild = modelBuilder.Entity<TEntity>();
            DoMap(entityBuild);
        }

        public Type EntityType => typeof(TEntity);


        protected virtual void DoMap(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(a => a.PId);
            var pkeyType = typeof(TPkey);
            if (pkeyType == typeof(string))
            {
                builder.Property(a => a.PId).IsRequired().HasMaxLength(36).HasValueGenerator<StringPKeyGenerator>();
            }
            else if (pkeyType == typeof(Guid))
            {
                builder.Property(a => a.PId).HasValueGenerator<GuidPKeyGenerator>();
            }
        }
    }
}