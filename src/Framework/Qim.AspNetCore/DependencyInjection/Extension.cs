using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Qim.Ioc;

namespace Qim.AspNetCore.DependencyInjection
{
    public static class Extension
    {
        public static void Populate(this IIocRegistrar registrar,
            IEnumerable<ServiceDescriptor> descriptors)
        {
            Ensure.NotNull(registrar, nameof(registrar));

            if (descriptors != null)
            {
                foreach (var descriptor in descriptors)
                {
                    var lifetime = GetLifetimeType(descriptor.Lifetime);
                    if (descriptor.ImplementationType != null)
                    {
                        registrar.RegisterType(descriptor.ServiceType, descriptor.ImplementationType, lifetime);
                    }
                    else if (descriptor.ImplementationFactory != null)
                    {
                        registrar.RegisterDelegate(descriptor.ServiceType, r => descriptor.ImplementationFactory(r.GetService<IServiceProvider>()), lifetime);
                    }
                    else
                    {
                        registrar.RegisterInstance(descriptor.ServiceType, descriptor.ImplementationInstance);
                    }
                }
            }

        }

        private static LifetimeType GetLifetimeType(ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    return LifetimeType.Singleton;
                case ServiceLifetime.Scoped:
                    return LifetimeType.Scoped;
                case ServiceLifetime.Transient:
                    return LifetimeType.Transient;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, "Not supported lifetime");
            }
        }
    }
}