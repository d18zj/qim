using System;
using Microsoft.Extensions.DependencyInjection;
using Qim.Ioc;

namespace Qim.AspNetCore.DependencyInjection
{
    internal class ServiceScope : IServiceScope
    {
        private readonly IIocScopeResolver _resolver;
        public ServiceScope(IIocScopeResolver resolver)
        {
            _resolver = resolver;
            ServiceProvider = new ServiceProvider(_resolver.Resolver);
        }


        public IServiceProvider ServiceProvider { get; }

        public void Dispose()
        {
            _resolver.Dispose();
        }
    }
}