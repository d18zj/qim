using DryIoc;

namespace Qim.Ioc.DryIoc
{
    internal class IocScopeResolverFactory: IIocScopeResolverFactory
    {
        private readonly IContainer _container;
        public IocScopeResolverFactory(IContainer container)
        {
            _container = container;
        }

        public IIocScopeResolver CreateScopeResolver(string name = null)
        {
            var scope = _container.OpenScope(name);
            return new IocScopeResolver(scope.Resolve<IContainer>());
        }
    }
}