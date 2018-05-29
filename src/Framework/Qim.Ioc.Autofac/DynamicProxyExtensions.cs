using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Features.Scanning;
using Castle.DynamicProxy;
using Qim.Ioc.Interception;
using System.Reflection;
using Castle.DynamicProxy.Internal;

namespace Qim.Ioc.Autofac
{
    internal static class DynamicProxyExtensions
    {
        private const string INTERCEPTORS_PROPERTY_NAME =
            "Autofac.Extras.DynamicProxy.RegistrationExtensions.InterceptorsPropertyName";

        private static readonly IEnumerable<Service> _emptyServices = new Service[0];
        private static readonly ProxyGenerationOptions _defaultOptions = new ProxyGenerationOptions(new InterceptMethodHook());
        private static readonly ProxyGenerator _proxyGenerator = new ProxyGenerator();

        /// <summary>
        ///     Enable class interception on the target type. Interceptors will be determined
        ///     via Intercept attributes on the class or added with InterceptedBy().
        ///     Only virtual methods can be intercepted this way.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to apply interception to.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> EnableClassInterceptors
            <TLimit, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> registration)
        {
            return EnableClassInterceptors(registration, _defaultOptions);
        }

        /// <summary>
        ///     Enable class interception on the target type. Interceptors will be determined
        ///     via Intercept attributes on the class or added with InterceptedBy().
        ///     Only virtual methods can be intercepted this way.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TRegistrationStyle">Registration style.</typeparam>
        /// <typeparam name="TConcreteReflectionActivatorData">Activator data type.</typeparam>
        /// <param name="registration">Registration to apply interception to.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>
            EnableClassInterceptors<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> registration)
            where TConcreteReflectionActivatorData : ReflectionActivatorData
        {
            return EnableClassInterceptors(registration, _defaultOptions);
        }

        /// <summary>
        ///     Enable class interception on the target type. Interceptors will be determined
        ///     via Intercept attributes on the class or added with InterceptedBy().
        ///     Only virtual methods can be intercepted this way.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to apply interception to.</param>
        /// <param name="options">Proxy generation options to apply.</param>
        /// <param name="additionalInterfaces">Additional interface types. Calls to their members will be proxied as well.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> EnableClassInterceptors
            <TLimit, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, ScanningActivatorData, TRegistrationStyle> registration,
                ProxyGenerationOptions options,
                params Type[] additionalInterfaces)
        {
            if (registration == null)
                throw new ArgumentNullException(nameof(registration));

            registration.ActivatorData.ConfigurationActions.Add(
                (t, rb) => rb.EnableClassInterceptors(options, additionalInterfaces));
            return registration;
        }

        /// <summary>
        ///     Enable class interception on the target type. Interceptors will be determined
        ///     via Intercept attributes on the class or added with InterceptedBy().
        ///     Only virtual methods can be intercepted this way.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TRegistrationStyle">Registration style.</typeparam>
        /// <typeparam name="TConcreteReflectionActivatorData">Activator data type.</typeparam>
        /// <param name="registration">Registration to apply interception to.</param>
        /// <param name="options">Proxy generation options to apply.</param>
        /// <param name="additionalInterfaces">Additional interface types. Calls to their members will be proxied as well.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>
            EnableClassInterceptors<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> registration,
                ProxyGenerationOptions options,
                params Type[] additionalInterfaces)
            where TConcreteReflectionActivatorData : ReflectionActivatorData
        {
            if (registration == null)
                throw new ArgumentNullException(nameof(registration));

            PipelineManager.Instance.InterceptByVirtualMethod(registration.ActivatorData.ImplementationType);

            registration.ActivatorData.ImplementationType =
                _proxyGenerator.ProxyBuilder.CreateClassProxyType(
                    registration.ActivatorData.ImplementationType, additionalInterfaces ?? new Type[0],
                    options);

            registration.OnPreparing(e =>
            {
                var proxyParameters = new List<Parameter>();
                var index = 0;

                if (options.HasMixins)
                    foreach (var mixin in options.MixinData.Mixins)
                        proxyParameters.Add(new PositionalParameter(index++, mixin));

                proxyParameters.Add(new PositionalParameter(index++,
                    GetInterceptorServices(e.Component, registration.ActivatorData.ImplementationType)
                        .Select(s => e.Context.ResolveService(s))
                        .Cast<IInterceptor>()
                        .ToArray()));
         
                if (options.Selector != null)
                    proxyParameters.Add(new PositionalParameter(index, options.Selector));

                e.Parameters = proxyParameters.Concat(e.Parameters).ToArray();
            });

            return registration;
        }



        /// <summary>
        ///     Allows a list of interceptor services to be assigned to the registration.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TStyle">Registration style.</typeparam>
        /// <param name="builder">Registration to apply interception to.</param>
        /// <param name="interceptorServices">The interceptor services.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        /// <exception cref="System.ArgumentNullException">builder or interceptorServices</exception>
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> InterceptedBy<TLimit, TActivatorData, TStyle>
        (
            this IRegistrationBuilder<TLimit, TActivatorData, TStyle> builder,
            params Service[] interceptorServices)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if ((interceptorServices == null) || interceptorServices.Any(s => s == null))
                throw new ArgumentNullException(nameof(interceptorServices));

            object existing;
            if (builder.RegistrationData.Metadata.TryGetValue(INTERCEPTORS_PROPERTY_NAME, out existing))
                builder.RegistrationData.Metadata[INTERCEPTORS_PROPERTY_NAME] =
                    ((IEnumerable<Service>)existing).Concat(interceptorServices).Distinct();
            else
                builder.RegistrationData.Metadata.Add(INTERCEPTORS_PROPERTY_NAME, interceptorServices);

            return builder;
        }


        /// <summary>
        /// Enable interface interception on the target type. Interceptors will be determined
        /// via Intercept attributes on the class or interface, or added with InterceptedBy() calls.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TSingleRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to apply interception to.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> EnableInterfaceInterceptors<TLimit, TActivatorData, TSingleRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration)
            where TActivatorData : ReflectionActivatorData
        {
            return EnableInterfaceInterceptors(registration, _defaultOptions);
        }

        /// <summary>
        /// Enable interface interception on the target type. Interceptors will be determined
        /// via Intercept attributes on the class or interface, or added with InterceptedBy() calls.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TSingleRegistrationStyle">Registration style.</typeparam>
        /// <param name="registration">Registration to apply interception to.</param>
        /// <param name="options">Proxy generation options to apply.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> EnableInterfaceInterceptors<TLimit, TActivatorData, TSingleRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration, ProxyGenerationOptions options)
             where TActivatorData : ReflectionActivatorData
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }
          
            PipelineManager.Instance.InterceptByInterface(registration.ActivatorData.ImplementationType); //GetProxiedInterfaces(registration.ActivatorData.ImplementationType)


            registration.RegistrationData.ActivatingHandlers.Add((sender, e) =>
            {
                EnsureInterfaceInterceptionApplies(e.Component);

                var proxiedInterfaces = GetProxiedInterfaces(e.Instance.GetType());

                if (!proxiedInterfaces.Any())
                {
                    return;
                }

                var theInterface = proxiedInterfaces.First();
                var interfaces = proxiedInterfaces.Skip(1).ToArray();

                var interceptors = GetInterceptorServices(e.Component, e.Instance.GetType())
                .Select(s => e.Context.ResolveService(s))
                .Cast<IInterceptor>()
                .ToArray();

                e.Instance = options == null
                ? _proxyGenerator.CreateInterfaceProxyWithTarget(theInterface, interfaces, e.Instance, interceptors)
                : _proxyGenerator.CreateInterfaceProxyWithTarget(theInterface, interfaces, e.Instance, options, interceptors);
            });

            return registration;
        }

        /// <summary>
        ///     Allows a list of interceptor services to be assigned to the registration.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TStyle">Registration style.</typeparam>
        /// <param name="builder">Registration to apply interception to.</param>
        /// <param name="interceptorServiceNames">The names of the interceptor services.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        /// <exception cref="System.ArgumentNullException">builder or interceptorServices</exception>
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> InterceptedBy<TLimit, TActivatorData, TStyle>
        (
            this IRegistrationBuilder<TLimit, TActivatorData, TStyle> builder,
            params string[] interceptorServiceNames)
        {
            if ((interceptorServiceNames == null) || interceptorServiceNames.Any(n => n == null))
                throw new ArgumentNullException(nameof(interceptorServiceNames));

            return InterceptedBy(builder,
                interceptorServiceNames.Select(n => new KeyedService(n, typeof(IInterceptor))).ToArray());
        }

        /// <summary>
        ///     Allows a list of interceptor services to be assigned to the registration.
        /// </summary>
        /// <typeparam name="TLimit">Registration limit type.</typeparam>
        /// <typeparam name="TActivatorData">Activator data type.</typeparam>
        /// <typeparam name="TStyle">Registration style.</typeparam>
        /// <param name="builder">Registration to apply interception to.</param>
        /// <param name="interceptorServiceTypes">The types of the interceptor services.</param>
        /// <returns>Registration builder allowing the registration to be configured.</returns>
        /// <exception cref="System.ArgumentNullException">builder or interceptorServices</exception>
        public static IRegistrationBuilder<TLimit, TActivatorData, TStyle> InterceptedBy<TLimit, TActivatorData, TStyle>
        (
            this IRegistrationBuilder<TLimit, TActivatorData, TStyle> builder,
            params Type[] interceptorServiceTypes)
        {
            if ((interceptorServiceTypes == null) || interceptorServiceTypes.Any(t => t == null))
                throw new ArgumentNullException(nameof(interceptorServiceTypes));

            return InterceptedBy(builder, interceptorServiceTypes.Select(t => new TypedService(t)).ToArray());
        }

        private static IEnumerable<Service> GetInterceptorServices(IComponentRegistration registration, Type implType)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            if (implType == null)
            {
                throw new ArgumentNullException(nameof(implType));
            }

            var result = _emptyServices;

            object services;
            if (registration.Metadata.TryGetValue(INTERCEPTORS_PROPERTY_NAME, out services))
            {
                result = result.Concat((IEnumerable<Service>)services);
            }

            return result.ToArray();
        }

        private static void EnsureInterfaceInterceptionApplies(IComponentRegistration componentRegistration)
        {
            if (componentRegistration.Services
                .OfType<IServiceWithType>()
                .Any(swt =>
                {
                    var info = swt.ServiceType.GetTypeInfo();
                    return !info.IsInterface || (!info.Assembly.IsInternalToDynamicProxy() && !info.IsVisible);
                }))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "The component {0} cannot use interface interception as it provides services that are not publicly visible interfaces. Check your registration of the component to ensure you're not enabling interception and registering it as an internal/private interface type.",
                    componentRegistration));
            }
        }

     

        private static Type[] GetProxiedInterfaces(Type type)
        {
            return type.GetInterfaces()
                .Where(i =>
                {
                    var info = i.GetTypeInfo();
                    return info.IsVisible || info.Assembly.IsInternalToDynamicProxy();
                }).ToArray();
        }
    }
}