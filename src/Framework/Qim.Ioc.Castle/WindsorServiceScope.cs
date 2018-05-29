using System;
using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;
using Castle.MicroKernel.Lifestyle;

namespace Qim.Ioc.Castle
{
    internal class WindsorServiceScope : IServiceScope, IIocResolverScope
    {
        private readonly IDisposable _scope;

        public WindsorServiceScope(IWindsorContainer container)
        {
            _scope = container.BeginScope();
            Resolver = container.Resolve<IIocResolver>();
            ServiceProvider = Resolver;
        }


        public IServiceProvider ServiceProvider { get; }

        public IIocResolver Resolver { get; }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}