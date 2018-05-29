using System;
using LightInject;

namespace Qim.Ioc.LightInject
{
    internal class IocResolverScope : IIocResolverScope
    {
        private readonly IocResolver _resolver;
        public IocResolverScope(Scope scope)
        {
            _resolver = new IocResolver(new Lazy<Scope>(() => scope));
        }

        public IIocResolver Resolver => _resolver;

        public void Dispose()
        {
            _resolver.Dispose();
        }
    }
}