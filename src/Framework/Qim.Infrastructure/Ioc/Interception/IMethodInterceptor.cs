namespace Qim.Ioc.Interception
{
    /// <summary>
    ///  拦截器
    /// </summary>
    public interface IMethodInterceptor
    {
        /// <summary>
        ///     拦截顺序
        /// </summary>
        int Order { get; set; }

        /// <summary>
        ///     执行拦截
        /// </summary>
        /// <param name="invocation"></param>
        void Intercept(IMethodInvocation invocation);
    }
}