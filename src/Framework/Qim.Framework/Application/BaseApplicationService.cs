using Qim.Domain.Uow;

namespace Qim.Application
{
    [UnitOfWork]
    public abstract class BaseApplicationService : DisposableObject, IApplicationService
    {
        private IUnitOfWorkManager _unitOfWorkManager;

        protected BaseApplicationService(IUnitOfWorkManager manager)
        {
            _unitOfWorkManager = manager;
        }

        /// <summary>
        ///     Reference to <see cref="IUnitOfWorkManager" />.
        /// </summary>
        public IUnitOfWorkManager UnitOfWorkManager
        {
            get
            {
                if (_unitOfWorkManager == null)
                {
                    throw new AppException("Must set UnitOfWorkManager before use it.");
                }

                return _unitOfWorkManager;
            }
            set { _unitOfWorkManager = value; }
        }


        protected virtual IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager.Current;


        protected override void Dispose(bool disposing)
        {
            CurrentUnitOfWork?.Dispose();
        }
    }
}