using System;
using System.Web.Mvc;
using System.Web.Routing;
using Qim.Infrastructure;

namespace QimErp.Web.Mvc
{
    public class IocControllerFactory: DefaultControllerFactory
    {
        private readonly IIocResolver _resolver;
        private IIocResolverScope _scope;
        public IocControllerFactory(IIocResolver resolver)
        {
            _resolver = resolver;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (null == controllerType)
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }
            return (IController)Scope.Resolver.GetService(controllerType);
        }

        public override void ReleaseController(IController controller)
        {
            base.ReleaseController(controller);
            Dispose();
        }

        protected IIocResolverScope Scope => _scope ?? (_scope = CreateScope());

        protected void Dispose()
        {
            _scope?.Dispose();
            _scope = null;
        }

        protected IIocResolverScope CreateScope()
        {
            var factory = _resolver.GetService<IIocResolverScopeFactory>();
            return factory.CreateScopeResolver();
        }
    }
}