using Qim.Ioc;

namespace Qim.Domain.Uow
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly ICurrentUnitOfWorkProvider _provider;
        private readonly IIocResolver _resolver;
        public UnitOfWorkManager(ICurrentUnitOfWorkProvider provider,IIocResolver resolver)
        {
            _provider = provider;
            _resolver = resolver;
        }


        /// <inheritdoc />
        public IUnitOfWork Current => _provider.Current;

        public IUnitOfWorkCompleteHandle Begin()
        {
            var uow = _resolver.GetService<IUnitOfWork>();
            uow.Disposed += Uow_Disposed;
            uow.Begin();
            _provider.Current = uow;
            return uow;
        }

        private void Uow_Disposed(object sender, System.EventArgs e)
        {
            _provider.Current = null;
        }

       
    }
}