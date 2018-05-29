using Autofac;

namespace Qim.Ioc.Autofac
{
    /// <summary>
    /// </summary>
    internal class AutofacServiceScopeFactory : IIocScopeResolverFactory
    {
        private readonly ILifetimeScope _parentlifetimeScope;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AutofacServiceScopeFactory" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        public AutofacServiceScopeFactory(ILifetimeScope lifetimeScope)
        {
            _parentlifetimeScope = lifetimeScope;
        }

        public IIocScopeResolver CreateScopeResolver(string name = null)
        {
            return new AutofacServiceScope(_parentlifetimeScope.BeginLifetimeScope());
        }
    }
}