using System;
using System.Threading.Tasks;
using Qim.Ioc.Tests.CaseServices;
using Xunit;

namespace Qim.Ioc.Tests
{
    public class LifetimeTests: TestBase
    {
        private readonly IIocScopeResolverFactory _scopeFactory;
        public LifetimeTests()
        {
            Reset();
            Registrar.Register<ISingletonBar, Bar>(LifetimeType.Singleton);
            Registrar.Register<IScopedBar, Bar>(LifetimeType.Scoped);
            Registrar.Register<ITransientBar, Bar>();
            Registrar.Register<ScopedTest>();
            _scopeFactory = Resolver.GetService<IIocScopeResolverFactory>();
        }

        private async Task DoInNewScopeAsync(Action<IIocScopeResolver> action)
        {
            using (var scope = _scopeFactory.CreateScopeResolver())
            {
                await Task.Delay(100);
                action(scope);
            }
        }

        [Fact]
        public void Singleton_Lifetime_Test()
        {
            ISingletonBar bar1 = Resolver.GetService<ISingletonBar>();
            ISingletonBar bar2 = null;
            ISingletonBar bar3 = null;
            Task.WaitAll(
                Task.Run(() => DoInNewScopeAsync(scope => bar2 = scope.Resolver.GetService<ISingletonBar>())),
                Task.Run(() => DoInNewScopeAsync(a => bar3 = Resolver.GetService<ISingletonBar>()))
            );
            Assert.Equal(bar1.Id, bar2.Id);
            Assert.Equal(bar2.Id, bar3.Id);
        }

        [Fact]
        public void Transient_Lifetime_Test()
        {
            ITransientBar bar1 = Resolver.GetService<ITransientBar>();
            ITransientBar bar2 = null;
            ITransientBar bar3 = null;
            Task.WaitAll(
                Task.Run(() => DoInNewScopeAsync(scope => bar2 = scope.Resolver.GetService<ITransientBar>())),
                Task.Run(() => DoInNewScopeAsync(a => bar3 = Resolver.GetService<ITransientBar>()))
            );
            Assert.NotEqual(bar1.Id, bar2.Id);
            Assert.NotEqual(bar2.Id, bar3.Id);
            Assert.NotEqual(bar1.Id, bar3.Id);
        }

        [Fact]
        public void Scoped_Lifetime_Test()
        {
            IScopedBar bar1 = Resolver.GetService<IScopedBar>();
            IScopedBar bar2 = null;
            IScopedBar bar3 = null;
            IScopedBar bar4 = null;
            IScopedBar bar5 = null;
            Task.WaitAll(
                Task.Run(() => DoInNewScopeAsync(scope =>
                    {
                        bar2 = scope.Resolver.GetService<IScopedBar>();
                        var resolver = scope.Resolver.GetService<IIocResolver>();
                        bar3 = resolver.GetService<ScopedTest>().Bar;
                        bar4 = scope.Resolver.GetService<IScopedBar>();
                    }
                )),
                Task.Run(()=>DoInNewScopeAsync(a =>
                {
                    bar5 = Resolver.GetService<IScopedBar>();
                   
                }))
            );

            Assert.NotEqual(bar1.Id, bar2.Id);

            Assert.Equal(bar2.Id, bar3.Id);
            Assert.Equal(bar2.Id, bar4.Id);

            Assert.Equal(bar1.Id, bar5.Id);



        }


        public class ScopedTest
        {
            
            public ScopedTest(IScopedBar bar)
            {
                Bar = bar;
            }

            public IScopedBar Bar { get; }
        }
    }
}