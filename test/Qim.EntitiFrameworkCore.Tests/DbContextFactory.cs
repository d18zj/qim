using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Qim.Configuration;
using Qim.Ioc.DryIoc;
using Qim.Reflection;

namespace Qim.EntitiFrameworkCore.Tests
{
    public class DbContextFactory : IDesignTimeDbContextFactory<EfCoreDbContext>
    {
        //const string ConnectingString =
        //   "Data Source=.;Initial Catalog=QimTest;User ID=sa ;Password=postgres;Connect Timeout=30";

     
        public EfCoreDbContext CreateDbContext(string[] args)
        {
            var resolver = Bootstrapper.Start(app =>
            {
                app.BaseAppPath = AppContext.BaseDirectory;
                return app.UseDryIoc();
            }).Resolver;

            var builder = new DbContextOptionsBuilder<EfCoreDbContext>();
            //builder.UseSqlServer(ConnectingString, b => b.MigrationsAssembly("Qim.EntitiFrameworkCore.Tests"));
            builder.UseNpgsql(TestBase.CONNECTING_STRING, b=>b.MigrationsAssembly("Qim.EntitiFrameworkCore.Tests"));

            return new EfCoreDbContext(builder.Options) { TypeFinder = resolver.GetService<ITypeFinder>() };
        }
    }
}