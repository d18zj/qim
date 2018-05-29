using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Qim.EntitiFrameworkCore.Map;

namespace Qim.EntitiFrameworkCore.Tests.Domain.Repositories.Map
{
    public class SeqGuidMap : BaseEntityMapConfig<SeqGuid, int>
    {
        protected override void DoMap(EntityTypeBuilder<SeqGuid> builder)
        {
            base.DoMap(builder);
            builder.ToTable("Test_SeqGuid");
            builder.Property(a => a.GuidKey).IsRequired();
            builder.Property(a => a.StrKey).IsRequired().HasMaxLength(36);
            builder.Property(a => a.ArrKey).IsRequired().HasMaxLength(16);

        }
    }
}