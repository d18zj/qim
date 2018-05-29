using System;
using System.Reflection;
using Castle.DynamicProxy;
using Qim.Ioc.Interception;

namespace Qim.Ioc.Autofac
{
    public class InterceptMethodHook : AllMethodsHook
    {
        public override bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            if (!base.ShouldInterceptMethod(type, methodInfo)) return false;
            if (methodInfo.MemberType != MemberTypes.Method) return false;
            if (type.GetTypeInfo().IsInterface) return true;
            return PipelineManager.Instance.GetPipeline(methodInfo).Count > 0;
        }
    }
}