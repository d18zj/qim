using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qim.EntitiFrameworkCore.Map;
using QimErp.Domain;
using QimErp.Domain.Entity;

namespace QimErp.Repositories.Map
{
    public class DatabaseInfoMap : CreationAndModificationLogEntityMapConfig<DatabaseInfo, int>
    {
        protected override void DoMap(EntityTypeBuilder<DatabaseInfo> builder)
        {
            base.DoMap(builder);
            builder.ToTable("Core_DatabaseInfo");
            builder.Property(a => a.DbNo).IsRequired().HasMaxLength(20);
            builder.HasIndex(a => a.DbNo).IsUnique();
            builder.Property(a => a.DbName).IsRequired().HasMaxLength(20);
            builder.Property(a => a.IpAddress).IsRequired().HasMaxLength(50);
            builder.Property(a => a.UserName).IsRequired().HasMaxLength(20);
            builder.Property(a => a.Password).IsRequired().HasMaxLength(50);
        }
    }
}