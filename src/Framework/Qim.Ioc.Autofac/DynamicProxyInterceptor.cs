using System.Linq;
using Castle.DynamicProxy;
using Qim.Ioc.Interception;

namespace Qim.Ioc.Autofac
{
    public class DynamicProxyInterceptor : IInterceptor
    {
        private readonly IIocResolver _resolver;

        public DynamicProxyInterceptor(IIocResolver resolver)
        {
            _resolver = resolver;
        }


        public void Intercept(IInvocation invocation)
        {
            var pipeline = PipelineManager.Instance.GetPipeline(invocation.MethodInvocationTarget);
            if (pipeline.Count > 0)
            {
                var interceptors =
                    pipeline.GetMethodInterceptors(_resolver);
                if (interceptors.Length > 0)
                {
                    var methodInterceptor = new MethodInvocation(invocation, interceptors);
                    methodInterceptor.Proceed();
                    return;
                }
            }
            invocation.Proceed();
        }
    }
}