using Qim.Configuration;
using Qim.Ioc;
using Qim.Ioc.DryIoc;

namespace Qim.Framework.Tests
{
    public abstract class TestBase
    {
        protected TestBase()
        {
            var config = Bootstrapper.Start(app => app.UseDryIoc().UseEmptyLogger());
            Registrar = config.Registrar;
            Resolver = config.Resolver;
        }

        protected IIocRegistrar Registrar { get; set; }

        protected IIocResolver Resolver { get; set; }
    }
}