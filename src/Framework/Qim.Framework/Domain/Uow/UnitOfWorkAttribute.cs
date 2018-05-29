using System;
using Qim.Ioc;
using Qim.Ioc.Interception;

namespace Qim.Domain.Uow
{
    public class UnitOfWorkAttribute :InterceptorAttribute
    {
        public UnitOfWorkAttribute() : this(false)
        {
            
        }

        public UnitOfWorkAttribute(bool nestedTransaction)
        {
            NestedTransaction = nestedTransaction;
            Order = -100;
        }

        /// <summary>
        ///  如果已在单元事务过程中时，是否采用嵌套事务
        /// </summary>
        public bool NestedTransaction { get; set; }

       
        public override IMethodInterceptor GetInterceptor(IIocResolver resolver)
        {
            var manager = resolver.GetService<IUnitOfWorkManager>();
            return new UnitOfWorkInterceptor(manager)
            {
                RequiresNewTransaction = NestedTransaction,
                Order = Order
            };
        }
    }
}