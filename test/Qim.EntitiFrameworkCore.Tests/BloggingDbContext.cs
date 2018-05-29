using System;
using Microsoft.EntityFrameworkCore;
using Qim.EntitiFrameworkCore.Map;
using Qim.EntitiFrameworkCore.Tests.Domain;
using Qim.Ioc;
using Qim.Reflection;

namespace Qim.EntitiFrameworkCore.Tests
{
    public class BloggingDbContext :DbContext
    {
        public BloggingDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var typesToRegister = TypeFinder.FindClassesOfType(typeof(IEntityMapConfig));
            foreach (var type in typesToRegister)
            {
                var mapConfig = (IEntityMapConfig)Activator.CreateInstance(type);
                mapConfig.Map(modelBuilder);
            }
            base.OnModelCreating(modelBuilder);
        }

        [Inject]
        public ITypeFinder TypeFinder { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }


        public DbSet<SeqGuid> SeqGuids { get; set; }
        
    }
}