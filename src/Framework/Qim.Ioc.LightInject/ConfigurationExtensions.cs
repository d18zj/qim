using System;
using LightInject;
using Qim.Configuration;

namespace Qim.Ioc.LightInject
{
    public static class ConfigurationExtensions
    {
        public static IIocAppConfiguration UseLightInject(this IAppConfiguration configuration)
        {
            Ensure.NotNull(configuration, nameof(configuration));
            var contaier = new ServiceContainer { ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider() };
            var register = new IocRegistrar(contaier);
            var resolver = new IocResolver(new Lazy<Scope>(() => contaier.BeginScope()));
            register.RegisterInstance<IIocRegistrar>(register);
            register.RegisterInstance<IIocResolver>(resolver);
            //register.Register<IIocResolverScopeFactory,IocResolverScopeFactory>();
            contaier.Register<IIocResolverScopeFactory>(factory => new IocResolverScopeFactory(factory), new PerScopeLifetime());
            return new IocAppConfiguration(configuration.ConfigDictionary)
            {
                Registrar = register,
                Resolver = resolver
            };

        }
    }
}