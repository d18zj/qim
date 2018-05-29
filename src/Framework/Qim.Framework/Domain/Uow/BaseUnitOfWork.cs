using System;
using System.Threading.Tasks;
using Qim.WorkContext;

namespace Qim.Domain.Uow
{
    public abstract class BaseUnitOfWork : IUnitOfWork
    {
        private int? _tenantId;
        private bool _isBeginCalledBefore;
        private bool _isCompleteCalledBefore;

        protected BaseUnitOfWork(IQimSession session)
        {
            Id = Guid.NewGuid().ToString("N");
            QimSession = session;
        }


        /// <summary>
        ///     Reference to current ABP session.
        /// </summary>
        public IQimSession QimSession {  get; protected set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            //if (!_succeed)
            //{
            //    OnFailed(_exception);
            //}

            DisposeUow();
            OnDisposed();
        }


        public string Id { get; }
     
        public event EventHandler Disposed;

        public void Begin()
        {
            PreventMultipleBegin();
            SetTenantId(QimSession.TenantId);

            BeginUow();
        }

        public bool IsDisposed { get; private set; }

        /// <inheritdoc />
        public abstract void SaveChanges();

        /// <inheritdoc />
        public abstract Task SaveChangesAsync();

        public IDisposable SetTenantId(int? tenantId)
        {
            var oldTenantId = _tenantId;
            _tenantId = tenantId;
            return new DisposeAction(() => { _tenantId = oldTenantId; });
        }

        public int? GetTenantId()
        {
            return _tenantId;
        }

        public IUnitOfWork Outer { get; set; }

        public  void Commit()
        {
            PreventMultipleComplete();
            CommitUow();
        }


        public async Task CommitAsync()
        {
            PreventMultipleComplete();
            await CommitUowAsync();
        }


        /// <summary>
        /// Can be implemented by derived classes to start UOW.
        /// </summary>
        protected virtual void BeginUow()
        {

        }

        /// <summary>
        /// Should be implemented by derived classes to commit UOW.
        /// </summary>
        protected abstract void CommitUow();

        /// <summary>
        /// Should be implemented by derived classes to commit UOW.
        /// </summary>
        protected abstract Task CommitUowAsync();

        /// <summary>
        /// Should be implemented by derived classes to dispose UOW.
        /// </summary>
        protected abstract void DisposeUow();

        private void PreventMultipleBegin()
        {
            if (_isBeginCalledBefore)
            {
                throw new AppException("This unit of work has started before. Can not call Start method more than once.");
            }

            _isBeginCalledBefore = true;
        }

        private void PreventMultipleComplete()
        {
            if (_isCompleteCalledBefore)
            {
                throw new AppException("Complete is called before!");
            }

            _isCompleteCalledBefore = true;
        }

        protected virtual void OnDisposed()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}