using System;
using Castle.Windsor;
using Microsoft.Extensions.DependencyInjection;

namespace Qim.Ioc.Castle
{
    internal class WindsorServiceProvider : BaseIocResolver, ISupportRequiredService
    {
        public WindsorServiceProvider(IWindsorContainer container) : base(container)
        {
        }


        public object GetRequiredService(Type serviceType)
        {
            return GetServiceInternal(serviceType, null, null, false);
        }
    }
}