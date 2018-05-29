using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Qim.AspNetCore.DependencyInjection;
using Qim.AspNetCore.Mvc;
using Qim.AspNetCore.Mvc.FluentMetadata;
using Qim.AspNetCore.WorkContext;
using Qim.Configuration;
using Qim.Ioc;
using Qim.WorkContext;

namespace Qim.AspNetCore
{
    public static class ConfigurationExtensions
    {
        public static IIocAppConfiguration AddHttpAuthentication(this IIocAppConfiguration configuration)
        {
            Ensure.NotNull(configuration, nameof(configuration));
            var registrar = configuration.Registrar;
            registrar.Register<IHttpContextAccessor, HttpContextAccessor>(LifetimeType.Singleton);
            registrar.Register<IAuthenticationService, HttpAuthenticationService>(LifetimeType.Singleton);
            registrar.Register<IPrincipalAccessor, HttpPrincipalAccessor>(LifetimeType.Singleton);
            registrar.Register<IQimSession, ClaimsSession>(LifetimeType.Singleton);
            return configuration;
        }

        public static IServiceProvider BuildServiceProvider(this IIocAppConfiguration configuration,
            IEnumerable<ServiceDescriptor> descriptors)
        {
            Ensure.NotNull(configuration, nameof(configuration));
            var register = configuration.Registrar;
            register.Register<IServiceScopeFactory, ServiceScopeFactory>();
            register.Register<IServiceProvider, ServiceProvider>();
            register.Populate(descriptors);
            //register.Replace(typeof(IModelMetadataProvider), typeof(ExtendedModelMetadataProvider), lifetime: LifetimeType.Singleton);

            return configuration.Resolver.GetService<IServiceProvider>();
        }


        public static IMvcBuilder UseFluentMetadata(this IMvcBuilder builder,
            Func<IServiceProvider, IMetadataConfiguratorProvider> factory = null)
        {
            Ensure.NotNull(builder, nameof(builder));
            if (factory != null)
            {
                builder.Services.AddSingleton(factory);
            }
            else
            {
                builder.Services.AddSingleton<IMetadataConfiguratorProvider, MetadataConfiguratorProvider>();
            }
            builder.Services.AddSingleton<IConfigureOptions<MvcOptions>, MvcOptionsSetup>();
            return builder;
        }

        public static IApplicationBuilder UseHttpAuthentication(this IApplicationBuilder app,
            Action<CookieAuthenticationOptions> options = null)
        {
            Ensure.NotNull(app, nameof(app));

            var service = app.ApplicationServices.GetService<IAuthenticationService>() as HttpAuthenticationService;
            if (service == null)
            {
                throw new InvalidOperationException("The must be call AddHttpAuthentication first!");
            }
            
            //var cookieOpt = new CookieAuthenticationOptions
            //{
            //    AutomaticChallenge = true
            //};
            //options?.Invoke(cookieOpt);

            //return app.UseCookieAuthentication(cookieOpt);
            return app.UseAuthentication(); 
        }
    }
}