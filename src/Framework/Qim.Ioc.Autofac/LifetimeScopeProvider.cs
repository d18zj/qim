using Autofac;

namespace Qim.Ioc.Autofac
{
    internal interface ILifetimeScopeProvider
    {
        ILifetimeScope LifetimeScope { get; }
    }

    internal class LifetimeScopeProvider : ILifetimeScopeProvider
    {
        private readonly IocRegistrar _registrar;
        private ILifetimeScope _scope;

        public LifetimeScopeProvider(IocRegistrar registrar, ILifetimeScope scope)
        {
            _registrar = registrar;
            _scope = scope;
        }

        public ILifetimeScope LifetimeScope
        {
            get
            {
                var scope = _registrar.UpdateContainer();
                return _scope ?? (_scope = scope);
            }
        }
    }
}