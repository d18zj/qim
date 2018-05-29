using System;
using Qim.Configuration;
using Qim.Domain.Repositories;
using Qim.Domain.Uow;
using Qim.EntitiFrameworkCore.Configuration;
using Qim.EntitiFrameworkCore.Uow;
using Qim.Ioc;

namespace Qim.EntitiFrameworkCore
{
    public static class ConfigurationExtensions
    {
        public static IIocAppConfiguration UseEntityFrameworkCore(this IIocAppConfiguration configuration,
            Action<DbContextConfiguration> action, string connectionString = null)
        {
            Ensure.NotNull(configuration, nameof(configuration));

            Ensure.NotNull(action, nameof(action));
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = configuration.DefaultNameOrConnectionString;
            }

            DbContextConfigurer.Instance.Init(connectionString, action);

            configuration.Registrar.Register<IDbContextProvider, UnitOfWorkDbContextProvider>();
            configuration.Registrar.Register<IDbContextResolver, DbContextResolver>();
            configuration.Registrar.Register<IUnitOfWork, EfCoreUnitOfWork>();
            configuration.Registrar.Register<IUnitOfWorkManager, UnitOfWorkManager>();
            configuration.Registrar.Register<ICurrentUnitOfWorkProvider, CurrentUnitOfWorkProvider>(LifetimeType.Singleton);
            configuration.Registrar.Register<IConnectionStringResolver, ConnectionStringResolver>();
            configuration.Registrar.Register<EfCoreDbContext, EfCoreDbContext>();
            configuration.Registrar.RegisterType(typeof(IRepository<,>), typeof(EfCoreRepository<,>));
            return configuration;
        }


    }
}