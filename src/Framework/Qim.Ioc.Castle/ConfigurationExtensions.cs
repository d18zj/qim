using System;
using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;
using ServiceDescriptor = Microsoft.Extensions.DependencyInjection.ServiceDescriptor;
using Castle.MicroKernel.Lifestyle;
using Qim.Configuration;

namespace Qim.Ioc.Castle
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IIocAppConfiguration UseCastleWindsor(this IAppConfiguration configuration)
        {
            return configuration.UseCastleWindsor(false);
        }

        /// <summary>
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="descriptors"></param>
        /// <returns></returns>
        public static IIocAppConfiguration UseCastleWindsorForAspNetCore(this IAppConfiguration configuration,
            IEnumerable<ServiceDescriptor> descriptors)
        {
            return configuration.UseCastleWindsor(false).Register(descriptors);
        }

        /// <summary>
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IIocAppConfiguration UseCastleWindsorForAspNet(this IAppConfiguration configuration)
        {
            return configuration.UseCastleWindsor(true);
        }

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



        #region private

        private static IIocAppConfiguration UseCastleWindsor(this IAppConfiguration configuration, bool classicWeb)
        {
            Ensure.NotNull(configuration,nameof(configuration));
            var manager = new IocManager(classicWeb);
            var container = manager.IocContainer;

            container.Register(
                Component.For<IWindsorContainer>()
                    .Instance(container)
                    .LifestyleSingleton()

            //Component.For<ChildServiceScope>()
            //   .LifestyleTransient()
            //.ApplyLifestyle(LifetimeType.Scoped,classicWeb)
            //Component.For<MsLifetimeScopeProvider>()
            //    .LifestyleTransient()
            );

            container.Register(Component.For<IIocResolver, IIocRegistrar>().Instance(manager));
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel,
                allowEmptyCollections: true));
            container.Register(
                Component.For<IIocResolverScopeFactory, IServiceScopeFactory>()
                    .ImplementedBy<WindsorServiceScopeFactory>()
                    .LifestyleSingleton());
            //container.Register(Component.For<IIocResolverScope, IServiceScope>().ImplementedBy<WindsorServiceScopeFactory>().LifestyleTransient());
            manager.Register<IServiceProvider, WindsorServiceProvider>();

            //manager.Register<IIocResolverScope, IocScopeResolver>();
            container.BeginScope();
             //manager.GetService<IocAppConfiguration>();
            var result = new CastleAppConfiguration(configuration);
            
            return result;
        }

        private static LifetimeType GetLifetimeType(this ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    return LifetimeType.Singleton;
                case ServiceLifetime.Scoped:
                    return LifetimeType.Scoped;
                // case ServiceLifetime.Transient:
                default:
                    return LifetimeType.Transient;
            }
        }

        private static IIocAppConfiguration Register(
            this IIocAppConfiguration configuration,
            IEnumerable<ServiceDescriptor> descriptors)
        {
            var manager = (IocManager)configuration.Registrar;

            foreach (var descriptor in descriptors)
            {
                var uniqueName = descriptor.ServiceType.FullName + "_" + Guid.NewGuid();

                var lifetime = GetLifetimeType(descriptor.Lifetime);
                if (descriptor.ImplementationType != null)
                    manager.IocContainer.Register(
                        Component.For(descriptor.ServiceType)
                            //.Named(uniqueName)
                            .NamedAutomatically(uniqueName)
                            .IsDefault()
                            .ImplementedBy(descriptor.ImplementationType)
                            .ApplyLifestyle(lifetime));
                else if (descriptor.ImplementationFactory != null)
                    manager.IocContainer.Register(Component.For(descriptor.ServiceType).UsingFactoryMethod(
                        (kernel, context) => descriptor.ImplementationFactory(kernel.Resolve<IServiceProvider>())
                    ).ApplyLifestyle(lifetime).IsDefault());
                else
                    manager.IocContainer.Register(
                        Component.For(descriptor.ServiceType)
                            .NamedAutomatically(uniqueName)
                            .IsDefault()
                            .Instance(descriptor.ImplementationInstance)
                            .ApplyLifestyle(lifetime)
                    );
            }

            return configuration;
        }

        internal static ComponentRegistration<T> ApplyLifestyle<T>(this ComponentRegistration<T> registration,
            LifetimeType lifetimeType, bool classicWeb = false)
            where T : class
        {
            switch (lifetimeType)
            {
                case LifetimeType.Transient:
                    return registration.LifestyleTransient();
                //return registration.LifestyleCustom<MsScopedTransientLifestyleManager>();
                case LifetimeType.Singleton:
                    return registration.LifestyleSingleton();
                //return registration.LifestyleCustom<MsScopedLifestyleManager>();
                case LifetimeType.Scoped:
                    //return classicWeb ? registration.LifestylePerWebRequest() : registration.LifestyleCustom<MyScopedSingletonLifestyleManager>(); //registration.LifestyleScoped();////
                    return classicWeb ? registration.LifestylePerWebRequest() : registration.LifestyleScoped(); ////

                default:
                    return registration;
            }
        }

        #endregion
    }
}