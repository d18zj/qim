using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using Qim.Infrastructure;
using Qim.Infrastructure.Autofac;
//using Qim.Infrastructure.Castle;
using QimErp.Web.Mvc.Controllers;


namespace QimErp.Web.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
           var config =  AppConfiguration.Create().UseAutofacForAspNet()
                //.UseCastleWindsorForAspNet()
                .Register(register =>
            {
                register.Register<IChildScoped, Scoped>(LifetimeType.Scoped);
                register.Register<ISingleton, Singleton>(LifetimeType.Singleton);
                register.Register<ITransient, Transient>();
                register.Register<TempClass, TempClass>();
                register.Register<HomeController,HomeController>();
            });

            //var container = ((IocRegistrar) config.Registrar).Container;

            ControllerBuilder.Current.SetControllerFactory(new IocControllerFactory(config.Resolver));
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(config.GetContainer()));

        }
    }
}
