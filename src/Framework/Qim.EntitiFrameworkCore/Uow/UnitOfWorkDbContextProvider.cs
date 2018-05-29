using System;
using Qim.Domain.Uow;
using Qim.MultiTenancy;

namespace Qim.EntitiFrameworkCore.Uow
{
    internal class UnitOfWorkDbContextProvider : IDbContextProvider
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
        public UnitOfWorkDbContextProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        public EfCoreDbContext GetDbContext(MultiTenancySides? multiTenancySides = null)
        {
            var efUnitOfWork = _currentUnitOfWorkProvider.Current as EfCoreUnitOfWork;
            if (efUnitOfWork == null)
            {
                throw new InvalidOperationException($"unitOfWork is not type of { typeof(EfCoreUnitOfWork).FullName } unitOfWork or is null.");
            }

            return efUnitOfWork.GetOrCreateDbContext(multiTenancySides);
        }
    }
}