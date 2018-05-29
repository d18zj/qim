using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qim.EntitiFrameworkCore.Map;

namespace Qim.EntitiFrameworkCore.Tests.Domain.Repositories.Map
{
    public class BlogMap : FullLogEntityMapConfig<Blog,string>
    {
        protected override void DoMap(EntityTypeBuilder<Blog> builder)
        {
            base.DoMap(builder);
            builder.ToTable("Test_Blog");
            builder.Property(a => a.Name).IsRequired().HasMaxLength(50);
            builder.Property(a => a.Url).IsRequired().HasMaxLength(100);


        }
    }
}