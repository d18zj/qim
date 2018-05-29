using LightInject;

namespace Qim.Ioc.LightInject
{
    public class IocResolverScopeFactory : IIocResolverScopeFactory
    {
        private readonly IServiceFactory _serviceFactory;
        public IocResolverScopeFactory(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public IIocResolverScope CreateScopeResolver()
        {
            return new IocResolverScope(_serviceFactory.BeginScope());
        }
    }
}