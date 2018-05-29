using System;
using Autofac;
using Qim.Configuration;

namespace Qim.Ioc.Autofac
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IIocAppConfiguration UseAutofac(this IAppConfiguration configuration)
        {
            Ensure.NotNull(configuration, nameof(configuration));
            var registrar = new IocRegistrar(new ContainerBuilder());

            registrar.RegisterInstance(registrar);
            registrar.RegisterInstance<IIocRegistrar>(registrar);
            //registrar.RegisterInstance<IIocResolver>(registrar);//不能注册成实例（单例模式)
            registrar.Register<ILifetimeScopeProvider, LifetimeScopeProvider>();
            registrar.Register<IIocResolver, IocResolver>();
            registrar.Register<IIocScopeResolverFactory, AutofacServiceScopeFactory>();

            registrar.Register<DynamicProxyInterceptor>();

            return configuration.CreateIocAppConfiguration(registrar,
                new IocResolver(new LifetimeScopeProvider(registrar, null)));
        }

        //public static IAutofacAppConfiguration LoadConfig(this IAutofacAppConfiguration configuration,
        //    IConfigurationRoot root)
        //{
        //    Ensure.NotNull(configuration, nameof(configuration));
        //    Ensure.NotNull(root, nameof(root));

        //    var registrar = (IocRegistrar) configuration.Registrar;
        //    var module = new ConfigurationModule(root);
        //    registrar.RegisterModule(module);
        //    return configuration;
        //}

        /// <summary>
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="register"></param>
        /// <returns></returns>
        public static IIocAppConfiguration Register(this IIocAppConfiguration configuration,
            Action<IIocRegistrar> register)
        {
            Ensure.NotNull(configuration, nameof(configuration));
            Ensure.NotNull(register, nameof(register));
            register(configuration.Registrar);
            return configuration;
        }
    }
}