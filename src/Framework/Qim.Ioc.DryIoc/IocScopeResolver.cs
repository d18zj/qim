using DryIoc;

namespace Qim.Ioc.DryIoc
{
    internal class IocScopeResolver : IIocScopeResolver
    {
        private readonly IocResolver _resolver;
        public IocScopeResolver(IContainer container)
        {
            _resolver = new IocResolver(container);
        }

        public void Dispose()
        {
            _resolver.Dispose();
        }

        public IIocResolver Resolver => _resolver;
    }
}