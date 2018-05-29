using System;
using Qim.Ioc.Tests.Interception;
using Xunit;

namespace Qim.Ioc.Tests
{
    public class InterceptorTest : TestBase
    {
        public InterceptorTest()
        {
            Reset();
            Registrar.Register<ICalculator, Calculator>("simple");
            Registrar.Register<NumberPy>();
            Registrar.Register<Tracer>(lifetime: LifetimeType.Singleton);
            Registrar.Register<EmptyInterceptor>();
            Registrar.Register<PlusOneInterceptor>();
        }

        [Fact]
        public void Interface_Intercept_Test()
        {
            var calculator = Resolver.GetService<ICalculator>("simple");
            var tracer = (Tracer)Resolver.GetService(typeof(Tracer));

            var result = calculator.Add(2, 3);
            Assert.Equal(2 + 3 + 1, result);
            Assert.True(tracer.ValidTraceLog(new Type[] { typeof(EmptyInterceptor), typeof(PlusOneInterceptor) }));

            tracer.ResetTrace();
            result = calculator.Subtract(9, 5);
            Assert.Equal(9 - 5 + 1, result);
            Assert.True(tracer.ValidTraceLog(new Type[] { typeof(PlusOneInterceptor), typeof(EmptyInterceptor) }));

        }

        [Fact]
        public void VirtualMethod_Intercept_Test()
        {
            var numpy = Resolver.GetService<NumberPy>();
            Assert.Equal(numpy.Divide(6, 2), (6 / 2) + 1);
        }
    }
}