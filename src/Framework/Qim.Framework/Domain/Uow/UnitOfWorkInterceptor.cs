using System.Reflection;
using System.Threading.Tasks;
using Qim.Ioc.Interception;
using Qim.Threading;

namespace Qim.Domain.Uow
{
    internal class UnitOfWorkInterceptor : IMethodInterceptor
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkInterceptor(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public bool RequiresNewTransaction { get; set; }

        public int Order { get; set; }

        public void Intercept(IMethodInvocation invocation)
        {
            if (
                invocation.MethodInvocationTarget.IsDefined(typeof(DisabledUnitOfWorkAttribute), false))
            {
                invocation.Proceed();
                return;
            }

            if (_unitOfWorkManager.Current != null && RequiresNewTransaction == false)
            {
                invocation.Proceed();
                return;
            }

            if (invocation.MethodInvocationTarget.IsAsyncMethod())
            {
                PerformAsyncUow(invocation);
            }
            else
            {
                PerformSyncUow(invocation);
            }
        }

        private void PerformSyncUow(IMethodInvocation invocation)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                invocation.Proceed();
                uow.Commit();
            }
        }

        private void PerformAsyncUow(IMethodInvocation invocation)
        {
            var uow = _unitOfWorkManager.Begin();

            invocation.Proceed();

            if (invocation.MethodInvocationTarget.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = AsyncHelper.AwaitTaskWithPostActionAndFinally(
                    (Task) invocation.ReturnValue,
                    async () => await uow.CommitAsync(),
                    exception => uow.Dispose()
                );
            }
            else //Task<TResult>
            {
                invocation.ReturnValue = AsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                    invocation.MethodInvocationTarget.ReturnType.GenericTypeArguments[0],
                    invocation.ReturnValue,
                    async () => await uow.CommitAsync(),
                    exception => uow.Dispose()
                );
            }
        }
    }
}