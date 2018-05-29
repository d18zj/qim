using System;
using System.Threading.Tasks;

namespace Qim.Domain.Uow
{
    public interface IUnitOfWorkCompleteHandle : IDisposable
    {
        void Commit();

        Task CommitAsync();
    }
}