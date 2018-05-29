using System;
using Microsoft.Extensions.DependencyInjection;
using Qim.Ioc;

namespace Qim.AspNetCore.DependencyInjection
{
    internal class ServiceProvider : DisposableObject, IServiceProvider, ISupportRequiredService
    {
        private readonly IIocResolver _resolver;

        public ServiceProvider(IIocResolver resolver)
        {
            _resolver = resolver;
        }


        public object GetRequiredService(Type serviceType)
        {
            return _resolver.GetService(serviceType);
        }

        public object GetService(Type serviceType)
        {
            return _resolver.GetOptionalService(serviceType);
        }

        protected override void Dispose(bool disposing)
        {
            _resolver.Dispose();
        }
    }
}