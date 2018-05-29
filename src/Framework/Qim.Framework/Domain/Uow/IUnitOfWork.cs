using System;
using System.Threading.Tasks;

namespace Qim.Domain.Uow
{
    public interface IUnitOfWork : IUnitOfWorkCompleteHandle
    {
        /// <summary>
        ///  Unique id 
        /// </summary>
        string Id { get; }


        /// <summary>
        /// This event is raised when this UOW is disposed.
        /// </summary>
        event EventHandler Disposed;

        /// <summary>
        /// Begins the unit of work with given options.
        /// </summary>
        void Begin();

        /// <summary>
        /// Is this UOW disposed?
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Saves all changes until now in this unit of work.
        /// This method may be called to apply changes whenever needed.
        /// Note that if this unit of work is transactional, saved changes are also rolled back if transaction is rolled back.
        /// No explicit call is needed to SaveChanges generally, 
        /// since all changes saved at end of a unit of work automatically.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Saves all changes until now in this unit of work.
        /// This method may be called to apply changes whenever needed.
        /// Note that if this unit of work is transactional, saved changes are also rolled back if transaction is rolled back.
        /// No explicit call is needed to SaveChanges generally, 
        /// since all changes saved at end of a unit of work automatically.
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        /// Sets/Changes Tenant's Id for this UOW.
        /// </summary>
        /// <param name="tenantId">The tenant id.</param>
        IDisposable SetTenantId(int? tenantId);

        /// <summary>
        /// Gets Tenant Id for this UOW.
        /// </summary>
        /// <returns></returns>
        int? GetTenantId();


        /// <summary>
        /// Reference to the outer UOW if exists.
        /// </summary>
        IUnitOfWork Outer { get; set; }
    }
}