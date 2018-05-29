using System;
using System.Reflection;
using Castle.DynamicProxy;
using Qim.Ioc.Interception;

namespace Qim.Ioc.Autofac
{
    internal class MethodInvocation : IMethodInvocation
    {
        private readonly IMethodInterceptor[] _interceptors;
        private readonly IInvocation _invocation;
        private int _currentIndex;

        public MethodInvocation(IInvocation invocation, IMethodInterceptor[] interceptors)
        {
            _invocation = invocation;
            _interceptors = interceptors;
            _currentIndex = -1;
        }

        public object[] Arguments => _invocation.Arguments;
        public Type[] GenericArguments => _invocation.GenericArguments;
        public object InvocationTarget => _invocation.InvocationTarget;
        public MethodInfo MethodInvocationTarget => _invocation.MethodInvocationTarget;

        public object ReturnValue
        {
            get => _invocation.ReturnValue;
            set => _invocation.ReturnValue = value;
        }

        public Type TargetType => _invocation.TargetType;

        public object GetArgumentValue(int index)
        {
            return Arguments[index];
        }

        public void Proceed()
        {
            _currentIndex++;

            try
            {
                if (_currentIndex < _interceptors.Length)
                {
                    _interceptors[_currentIndex].Intercept(this);
                }
                else if (_currentIndex == _interceptors.Length)
                {
                    _invocation.Proceed();
                }
                else
                {
                    throw new InvalidOperationException(
                        $"The invocation.Proceed() has been called more times than expected,calls invocation.Proceed() at most once.");
                }
            }
            finally
            {
                _currentIndex--;
            }
        }

        public void SetArgumentValue(int index, object value)
        {
            Arguments[index] = value;
        }
    }
}