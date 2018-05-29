using Microsoft.Extensions.DependencyInjection;
using Qim.Ioc;

namespace Qim.AspNetCore.DependencyInjection
{
    internal class ServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IIocScopeResolverFactory _factory;
        public ServiceScopeFactory(IIocScopeResolverFactory factory)
        {
            _factory = factory;
        }

        public IServiceScope CreateScope()
        {
            return new ServiceScope(_factory.CreateScopeResolver());
        }
    }
}