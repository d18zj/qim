using System;
using System.Reflection;

namespace Qim.Ioc.Interception
{
    public interface IMethodInvocation
    {
        /// <summary>
		///   Gets the arguments that the <see cref = "MethodInfo" /> has been invoked with.
		/// </summary>
		/// <value>The arguments the method was invoked with.</value>
		object[] Arguments { get; }

        /// <summary>
        ///   Gets the generic arguments of the method.
        /// </summary>
        /// <value>The generic arguments, or null if not a generic method.</value>
        Type[] GenericArguments { get; }

        /// <summary>
        ///   Gets the object on which the invocation is performed. This is different from proxy object
        ///   because most of the time this will be the proxy target object.
        /// </summary>
        /// <value>The invocation target.</value>
        object InvocationTarget { get; }


        /// <summary>
        ///   For interface proxies, this will point to the <see cref = "MethodInfo" /> on the target class.
        /// </summary>
        /// <value>The method invocation target.</value>
        MethodInfo MethodInvocationTarget { get; }

        /// <summary>
        ///   Gets or sets the return value of the method.
        /// </summary>
        /// <value>The return value of the method.</value>
        object ReturnValue { get; set; }

        /// <summary>
        ///   Gets the type of the target object for the intercepted method.
        /// </summary>
        /// <value>The type of the target object.</value>
        Type TargetType { get; }

        /// <summary>
        ///   Gets the value of the argument at the specified <paramref name = "index" />.
        /// </summary>
        /// <param name = "index">The index.</param>
        /// <returns>The value of the argument at the specified <paramref name = "index" />.</returns>
        object GetArgumentValue(int index);


        /// <summary>
        ///   Proceeds the call to the next interceptor in line, and ultimately to the target method.
        /// </summary>
        /// <remarks>
        ///   Since interface proxies without a target don't have the target implementation to proceed to,
        ///   it is important, that the last interceptor does not call this method, otherwise a
        ///   <see cref = "NotImplementedException" /> will be thrown.
        /// </remarks>
        void Proceed();

        /// <summary>
        ///   Overrides the value of an argument at the given <paramref name = "index" /> with the
        ///   new <paramref name = "value" /> provided.
        /// </summary>
        /// <remarks>
        ///   This method accepts an <see cref = "object" />, however the value provided must be compatible
        ///   with the type of the argument defined on the method, otherwise an exception will be thrown.
        /// </remarks>
        /// <param name = "index">The index of the argument to override.</param>
        /// <param name = "value">The new value for the argument.</param>
        void SetArgumentValue(int index, object value);
    }
}