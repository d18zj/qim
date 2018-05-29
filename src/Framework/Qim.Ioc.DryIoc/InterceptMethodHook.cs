using System;
using System.Reflection;
using Castle.DynamicProxy;
using Qim.Ioc.Interception;

namespace Qim.Ioc.DryIoc
{
    internal class InterceptMethodHook : AllMethodsHook
    {
        public override bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            if (!base.ShouldInterceptMethod(type, methodInfo)) return false;
            if (methodInfo.MemberType != MemberTypes.Method) return false; //只拦截方法
            if (type.GetTypeInfo().IsInterface) return true; //如果是接口，拦截全部方法
            return PipelineManager.Instance.GetPipeline(methodInfo).Count > 0;
        }
    }
}