using Qim.EntitiFrameworkCore.Configuration;
using Qim.Ioc;

namespace Qim.EntitiFrameworkCore
{
    public class DbContextResolver : IDbContextResolver
    {
        private readonly IIocResolver _resolver;

        public DbContextResolver(IIocResolver resolver)
        {
            _resolver = resolver;
        }


        public EfCoreDbContext Resolve(string connectionString = null)
        {
            var dbContextConstructorArgs = new
            {
                options = DbContextConfigurer.Instance.GetDbContextOptions(connectionString)
            };

            return _resolver.GetService<EfCoreDbContext>(dbContextConstructorArgs);
        }
    }
}