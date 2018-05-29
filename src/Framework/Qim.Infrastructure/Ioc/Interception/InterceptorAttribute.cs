using System;

namespace Qim.Ioc.Interception
{
    /// <summary>
    ///   方法拦截器抽象基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public abstract class InterceptorAttribute : Attribute
    {
        /// <summary>
        ///  拦截顺序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        ///     返回拦截器
        /// </summary>
        /// <param name="resolver">ioc resolver</param>
        /// <returns></returns>
        public abstract IMethodInterceptor GetInterceptor(IIocResolver resolver);
    }
}