using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Qim.Ioc.Interception
{
    public class PipelineManager
    {
        private static readonly HandlerPipeline _emptyPipeline = new HandlerPipeline();

        public static readonly PipelineManager Instance = new PipelineManager();

        private readonly Dictionary<HandlerPipelineKey, HandlerPipeline> _pipelines =
            new Dictionary<HandlerPipelineKey, HandlerPipeline>();

        private PipelineManager()
        {
        }


        public HandlerPipeline GetPipeline(MethodInfo method)
        {
            HandlerPipelineKey key = HandlerPipelineKey.ForMethod(method);
            HandlerPipeline pipeline = _emptyPipeline;
            if (_pipelines.ContainsKey(key))
            {
                pipeline = _pipelines[key];
            }
            return pipeline;
        }

        public void InterceptByVirtualMethod(Type implementationType)
        {
            Ensure.NotNull(implementationType, nameof(implementationType));

            var classAttributes = GetInterceptorAttributes(implementationType.GetTypeInfo());

            //处理Method上的拦截器,继承自object上的方法和属性不会拦截
            foreach (
                var methodInfo in
                implementationType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod)
                    .Where(a => a.IsVirtual && a.MemberType == MemberTypes.Method && a.DeclaringType != typeof(object)))
            {
                CreateHandlerPipeline(methodInfo, classAttributes);

            }
        }

        public void InterceptByInterface(Type implementationType)
        {
            Ensure.NotNull(implementationType, nameof(implementationType));

            var proxiedInterfaces = implementationType.GetInterfaces()
                .Where(i => i.GetTypeInfo().IsVisible).ToArray(); //|| info.Assembly.IsInternalToDynamicProxy();暂只拦截公开接口

            if (proxiedInterfaces.Length <= 0) return;

            var typeInfo = implementationType.GetTypeInfo();
            var classAttributes = GetInterceptorAttributes(typeInfo);

            foreach (var proxiedInterface in proxiedInterfaces)
            {
                var map = typeInfo.GetRuntimeInterfaceMap(proxiedInterface);
                for (int i = 0; i < map.InterfaceMethods.Length; i++)
                {
                    CreateHandlerPipeline(map.TargetMethods[i], classAttributes);
                }
            }
        }

        private void CreateHandlerPipeline(MethodInfo method, InterceptorAttribute[] classAttributes)
        {
            var customAttributes = GetInterceptorAttributes(method);
            if (classAttributes.Length > 0 || customAttributes.Length > 0)
            {
                var key = HandlerPipelineKey.ForMethod(method);
                if (!_pipelines.ContainsKey(key))
                {
                    var pipeline = new HandlerPipeline(customAttributes);
                    if (classAttributes.Length > 0)
                    {
                        pipeline.Append(classAttributes);
                    }
                    _pipelines[key] = pipeline;
                }
            }
        }

        private InterceptorAttribute[] GetInterceptorAttributes(MemberInfo info)
        {
            return info.GetCustomAttributes(typeof(InterceptorAttribute), true)
                         .Cast<InterceptorAttribute>().ToArray();
        }
    }
}