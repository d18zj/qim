using System;
using System.Linq;
using Castle.DynamicProxy;
using DryIoc;
using Qim.Ioc.Interception;

namespace Qim.Ioc.DryIoc
{
    internal static class DynamicProxyExtensions
    {
        private static DefaultProxyBuilder _proxyBuilder;

        private static readonly ProxyGenerationOptions _generationOptions =
            new ProxyGenerationOptions(new InterceptMethodHook());
        public static void Intercept<TInterceptor>(this IRegistrator registrator, Type serviceType, object serviceKey = null)
            where TInterceptor : class, IInterceptor
        {
            Ensure.NotNull(serviceType, nameof(serviceType));

            Type proxyType;
            if (serviceType.IsInterface())
            {
                proxyType = ProxyBuilder.CreateInterfaceProxyTypeWithTargetInterface(
                    serviceType, new Type[] { }, _generationOptions);
            }
            else if (serviceType.IsClass())
            {
                proxyType = ProxyBuilder.CreateClassProxyType(
                    serviceType, new Type[] { }, _generationOptions);
            }
            else
            {
                throw new ArgumentException(
                    $"Intercepted service type {serviceType} is not a supported: it is nor class nor interface");
            }

            var decoratorSetup = serviceKey == null
                ? Setup.DecoratorWith(useDecorateeReuse: true)
                : Setup.DecoratorWith(r => serviceKey.Equals(r.ServiceKey), useDecorateeReuse: true);

            registrator.Register(serviceType, proxyType,
                made: Made.Of(type => type.GetPublicInstanceConstructors().SingleOrDefault(c => c.GetParameters().Length != 0),
                    Parameters.Of.Type<IInterceptor[]>(typeof(TInterceptor[]))),
                setup: decoratorSetup);

            if (!registrator.IsRegistered<TInterceptor>())
            {
                registrator.Register<TInterceptor>();
            }
        }

        private static DefaultProxyBuilder ProxyBuilder => _proxyBuilder ?? (_proxyBuilder = new DefaultProxyBuilder());
    }
}