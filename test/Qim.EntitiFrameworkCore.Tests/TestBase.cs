using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qim.Configuration;
using Qim.EntitiFrameworkCore.Tests.Domain;
using Qim.EntitiFrameworkCore.Tests.Domain.Repositories;
using Qim.Ioc;
using Qim.Ioc.DryIoc;
using Qim.WorkContext;

namespace Qim.EntitiFrameworkCore.Tests
{
    public abstract class TestBase
    {
        //const string CONNECTING_STRING =
        //    "Data Source=.;Initial Catalog=QimTest;User ID=sa ;Password=txlzj.;Connect Timeout=30";

       
        public const string CONNECTING_STRING = "Server=localhost;Database=DemoA;Uid=postgres;Password=cleacls;";
        protected TestBase()
        {
            var result = Bootstrapper.Start(app =>
            {
#if NET451
               
#else
                app.BaseAppPath = AppContext.BaseDirectory;
#endif
                app.DefaultNameOrConnectionString = CONNECTING_STRING;
                return app.UseDryIoc().UseEmptyLogger()
                    .UseEntityFrameworkCore(c =>
                        {
                            //c.Builder.UseSqlServer(c.ConnectionString);
                            c.Builder.UseNpgsql(c.ConnectionString);
                        })
                    ;
            });
            Registrar = result.Registrar;
            Resolver = result.Resolver;
            Session = new TestSession();
            Registrar.Register<IPostRepository, PostRepository>();
            Registrar.Register<IBlogRepository, BlogRepository>();
            Registrar.RegisterInstance<IQimSession>(Session);

            var builder = new DbContextOptionsBuilder();
            //builder.UseSqlServer(CONNECTING_STRING);
            builder.UseNpgsql(CONNECTING_STRING);
            Registrar.RegisterInstance<DbContextOptions>(builder.Options);
            Registrar.Register<BloggingDbContext>();

        }

    

        protected TestSession Session { get; set; }

        protected IIocRegistrar Registrar { get; set; }

        protected IIocResolver Resolver { get; set; }

        public void UsingDbContext(Action<BloggingDbContext> action)
        {
            using (var context = Resolver.GetService<BloggingDbContext>())
            {
                action(context);
                context.SaveChanges();
            }
        }

        public T UsingDbContext<T>(Func<BloggingDbContext, T> func)
        {
            T result;

            using (var context = Resolver.GetService<BloggingDbContext>())
            {
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }

        public async Task UsingDbContextAsync(Func<BloggingDbContext, Task> action)
        {
            using (var context = Resolver.GetService<BloggingDbContext>())
            {
                await action(context);
                await context.SaveChangesAsync(true);
            }
        }

        public async Task<T> UsingDbContextAsync<T>(Func<BloggingDbContext, Task<T>> func)
        {
            T result;

            using (var context = Resolver.GetService<BloggingDbContext>())
            {
                result = await func(context);
                context.SaveChanges();
            }

            return result;
        }
    }
}