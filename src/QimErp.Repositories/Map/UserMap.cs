using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qim.EntitiFrameworkCore.Map;
using QimErp.Domain;
using QimErp.Domain.Entity;

namespace QimErp.Repositories.Map
{
    public class UserMap : CreationAndModificationLogEntityMapConfig<User, string, User>
    {
        protected override void DoMap(EntityTypeBuilder<User> builder)
        {
            base.DoMap(builder);
            builder.ToTable("Core_Users");

            builder.HasIndex(a => new {a.UserAccount}).IsUnique(true);
            builder.HasIndex(a => a.TenantId);

            builder.Property(a => a.UserAccount).IsRequired().HasMaxLength(20);
            builder.Property(a => a.UserName).IsRequired().HasMaxLength(20);
            builder.Property(a => a.Password).IsRequired().HasMaxLength(50);
            builder.Property(a => a.Salt).IsRequired().HasMaxLength(50);
            builder.Property(a => a.CellPhone).IsRequired().HasMaxLength(50);
            builder.Property(a => a.Email).IsRequired().HasMaxLength(100);
            builder.Property(a => a.Description);
        }
    }
}