using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qim.EntitiFrameworkCore.Map;
using QimErp.Domain;
using QimErp.Domain.Entity;

namespace QimErp.Repositories.Map
{
    public class GlobalAccountMap : CreationAndModificationLogEntityMapConfig<GlobalAccount, int>
    {
        protected override void DoMap(EntityTypeBuilder<GlobalAccount> builder)
        {
            base.DoMap(builder);
            builder.ToTable("Core_TenantAccount");
            builder.Property(a => a.UserId).IsRequired().HasMaxLength(36);
            builder.Property(a => a.Email).IsRequired().HasMaxLength(100);
            builder.Property(a => a.CellPhone).IsRequired().HasMaxLength(50);
            builder.HasIndex(a => a.Email);
            builder.HasIndex(a => a.CellPhone);
            builder.HasIndex(a => a.UserId);
            builder.HasOne(a => a.Tenant).WithMany().HasForeignKey(a => a.TenantId);
        }
    }
}