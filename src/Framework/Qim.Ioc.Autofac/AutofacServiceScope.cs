using Autofac;

namespace Qim.Ioc.Autofac
{
    /// <summary>
    ///     Autofac implementation of the ASP.NET Core
    /// </summary>
    internal class AutofacServiceScope : IIocScopeResolver
    {
        private readonly IIocResolver _resolver;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AutofacServiceScope" /> class.
        /// </summary>
        /// <param name="scope">
        ///     The lifetime scope from which services should be resolved for this service scope.
        /// </param>
        public AutofacServiceScope(ILifetimeScope scope)
        {
            _resolver = scope.Resolve<IIocResolver>();
        }


        /// <summary>
        ///     Disposes of the lifetime scope and resolved disposable services.
        /// </summary>
        public void Dispose()
        {
            _resolver.Dispose();
        }


        public IIocResolver Resolver => _resolver;
    }
}