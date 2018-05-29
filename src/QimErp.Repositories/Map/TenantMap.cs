using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qim.EntitiFrameworkCore.Map;
using QimErp.Domain;
using QimErp.Domain.Entity;

namespace QimErp.Repositories.Map
{
    public class TenantMap: CreationAndModificationLogEntityMapConfig<Tenant,int>
    {
        protected override void DoMap(EntityTypeBuilder<Tenant> builder)
        {
            base.DoMap(builder);
            builder.ToTable("Core_Tenant");
            builder.Property(a => a.TenantName).IsRequired().HasMaxLength(50);
            builder.Property(a => a.StartTime).IsRequired().HasColumnType("date");
            builder.Property(a => a.EndTime).IsRequired().HasColumnType("date");

            builder.HasOne(a => a.DatabaseInfo).WithMany(d=>d.Tenants).HasForeignKey(a => a.DatabaseId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}