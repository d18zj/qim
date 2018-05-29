using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Qim.Infrastructure;

namespace QimErp.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISingleton _singleton;
        private readonly IChildScoped _scoped;
        private readonly ITransient _transient;
        private readonly IIocResolver _container;
        private readonly IIocResolverScopeFactory _factory;
        private readonly IServiceProvider _provider;
        private readonly TempClass _temp6;
        public HomeController(ISingleton singleton, ITransient transient, IChildScoped scoped,
            IServiceProvider provider,
            TempClass temp,
            IIocResolverScopeFactory factory,
            IIocResolver container
            )
        {
            _singleton = singleton;
            _transient = transient;
            _scoped = scoped;
            _provider = provider;
            _container = container;
            _factory = factory;
            _temp6 = temp;
        }
        
        public ActionResult Index()
        {
            using (var ctx = _factory.CreateScopeResolver())
            {


                var singleton2 = _container.GetService<ISingleton>();
                var scoped2 = _container.GetService<IChildScoped>();


                var scoped4 = (IChildScoped)HttpContext.GetService(typeof(IChildScoped));
                var scoped3 = (IChildScoped)_provider.GetService(typeof(IChildScoped));
                var scoped5 = ctx.Resolver.GetService<IChildScoped>();
                //var scoped4 = (IChildScoped)HttpContext.RequestServices.GetService(typeof(IChildScoped));
                //var scoped5 = ctx.Resolver.GetService<IChildScoped>();
                //_temp.Scoped;//(IChildScoped) HttpContext.RequestServices.GetService(typeof(IChildScoped));
                var transient2 = _container.GetService<ITransient>();
                var list = new List<string>
                {
                    string.Format("Singleton 1:{0}   Singleton 2:{1}", _singleton.Id, singleton2.Id),
                    string.Format("Scoped 1:{0}   Scoped 2:{1}", _scoped.Id, scoped2.Id),
                    string.Format("Scoped 3:{0}   Scoped 4:{1}", scoped3.Id, scoped4?.Id),
                    string.Format("Scoped 5:{0}   Scoped 6:{1}", scoped5.Id, _temp6.Scoped.Id),
                    string.Format("Transient 1:{0}   Transient 2:{1}", _transient.Id, transient2.Id)
                };
                ViewData["testMsg"] = list;

                return View();
            }
        }
    }
}