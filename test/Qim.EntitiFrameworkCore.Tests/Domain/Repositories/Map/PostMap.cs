using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qim.EntitiFrameworkCore.Map;

namespace Qim.EntitiFrameworkCore.Tests.Domain.Repositories.Map
{
    public class PostMap : CreationAndModificationLogEntityMapConfig<Post, Guid>
    {
        protected override void DoMap(EntityTypeBuilder<Post> builder)
        {
            base.DoMap(builder);
            builder.ToTable("Test_Post");
            builder.Property(a => a.Title).IsRequired().HasMaxLength(200);
            builder.Property(a => a.Body).IsRequired();

            builder.HasOne(a => a.Blog).WithMany(b => b.Posts).HasForeignKey(a => a.BlogId);
        }
    }
}