using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;

namespace Qim.Ioc.Castle
{
    internal class WindsorServiceScopeFactory : IServiceScopeFactory, IIocResolverScopeFactory
    {
        private readonly IWindsorContainer _container;

        public WindsorServiceScopeFactory(IWindsorContainer container)
        {
            _container = container;
        }

        public IIocResolverScope CreateScopeResolver()
        {
            return new WindsorServiceScope(_container);
        }

        public IServiceScope CreateScope()
        {
            return new WindsorServiceScope(_container);
        }
    }
}