using Qim.Ioc.Interception;

namespace Qim.Ioc.Tests.Interception
{
    public class PlusOneAttribute : InterceptorAttribute
    {
        public override IMethodInterceptor GetInterceptor(IIocResolver resolver)
        {
            var plusOne = resolver.GetService<PlusOneInterceptor>();
            plusOne.Order = Order;
            return plusOne;
        }
    }

    public class EmptyAttribute : InterceptorAttribute
    {
        public override IMethodInterceptor GetInterceptor(IIocResolver resolver)
        {
            var empty = resolver.GetService<EmptyInterceptor>();
            empty.Order = Order;
            return empty;
        }
    }

    public class EmptyInterceptor : IMethodInterceptor
    {
        private readonly Tracer _tracer;

        public EmptyInterceptor(Tracer tracer)
        {
            _tracer = tracer;
        }

        public int Order { get; set; }

        public void Intercept(IMethodInvocation invocation)
        {
            _tracer.Trace(GetType());
            invocation.Proceed();
        }
    }

    public class PlusOneInterceptor : IMethodInterceptor
    {
        private readonly Tracer _tracer;

        public PlusOneInterceptor(Tracer tracer)
        {
            _tracer = tracer;
        }

        public int Order { get; set; } = 100;

        public void Intercept(IMethodInvocation invocation)
        {
            _tracer.Trace(GetType());
            invocation.Proceed();
            if (invocation.MethodInvocationTarget.ReturnType == typeof(int))
            {
                invocation.ReturnValue = (int) invocation.ReturnValue + 1;
            }
        }
    }
}