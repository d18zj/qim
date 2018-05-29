using System.Collections.Concurrent;
#if NET451
using System.Runtime.Remoting.Messaging;
#else
using System.Threading;
#endif

using Qim.Logging;

namespace Qim.Domain.Uow
{
    public class CurrentUnitOfWorkProvider : ICurrentUnitOfWorkProvider
    {
        private const string CONTEXT_KEY = "_QimSoft.UnitOfWork.Current";
        private static readonly ConcurrentDictionary<string, IUnitOfWork> _unitOfWorkDictionary = new ConcurrentDictionary<string, IUnitOfWork>();

        private readonly ILogger _logger;
        public CurrentUnitOfWorkProvider(ILogger<CurrentUnitOfWorkProvider> logger)
        {
            _logger = logger;
        }


        private IUnitOfWork GetCurrentUow()
        {
            var unitOfWorkKey = GetCacheId();
            if (unitOfWorkKey == null)
            {
                return null;
            }

            IUnitOfWork unitOfWork;
            if (!_unitOfWorkDictionary.TryGetValue(unitOfWorkKey, out unitOfWork))
            {
                _logger.Warn(
                    $"There is a unitOfWorkKey in CallContext but not in UnitOfWorkDictionary (on GetCurrentUow)! UnitOfWork key: { unitOfWorkKey }");
                SetCacheId(null);
                return null;
            }

            if (unitOfWork.IsDisposed)
            {
                _logger.Warn("There is a unitOfWorkKey in CallContext but the UOW was disposed!");
                _unitOfWorkDictionary.TryRemove(unitOfWorkKey, out unitOfWork);
                SetCacheId(null);
                return null;
            }

            return unitOfWork;
        }

        private void SetCurrentUow(IUnitOfWork value)
        {
            if (value == null)
            {
                ExitFromCurrentUowScope();
                return;
            }

            var unitOfWorkKey = GetCacheId();
            if (unitOfWorkKey != null)
            {
                IUnitOfWork outer;
                if (_unitOfWorkDictionary.TryGetValue(unitOfWorkKey, out outer))
                {
                    if (outer == value)
                    {
                        _logger.Warn("Setting the same UOW to the CallContext, no need to set again!");
                        return;
                    }

                    value.Outer = outer;
                }
                else
                {
                    _logger.Warn("There is a unitOfWorkKey in CallContext but not in UnitOfWorkDictionary (on SetCurrentUow)! UnitOfWork key: " + unitOfWorkKey);
                }
            }

            unitOfWorkKey = value.Id;
            if (!_unitOfWorkDictionary.TryAdd(unitOfWorkKey, value))
            {
                throw new AppException("Can not set unit of work! UnitOfWorkDictionary.TryAdd returns false!");
            }

            SetCacheId(unitOfWorkKey);
        }

        private void ExitFromCurrentUowScope()
        {
            var unitOfWorkKey = GetCacheId();
            if (unitOfWorkKey == null)
            {
                _logger.Warn("There is no current UOW to exit!");
                return;
            }

            IUnitOfWork unitOfWork;
            if (!_unitOfWorkDictionary.TryGetValue(unitOfWorkKey, out unitOfWork))
            {
                _logger.Warn("There is a unitOfWorkKey in CallContext but not in UnitOfWorkDictionary (on ExitFromCurrentUowScope)! UnitOfWork key: " + unitOfWorkKey);
                SetCacheId(null);
                return;
            }

            _unitOfWorkDictionary.TryRemove(unitOfWorkKey, out unitOfWork);
            if (unitOfWork.Outer == null)
            {
                SetCacheId(null);
                return;
            }

            //Restore outer UOW
            var outerUnitOfWorkKey = unitOfWork.Outer.Id;
            if (!_unitOfWorkDictionary.TryGetValue(outerUnitOfWorkKey, out unitOfWork))
            {
                //No outer UOW
                _logger.Warn("Outer UOW key could not found in UnitOfWorkDictionary!");
                SetCacheId(null);
                return;
            }

            SetCacheId(outerUnitOfWorkKey);
        }

#if NET451
        private string GetCacheId()
        {
            return CallContext.LogicalGetData(CONTEXT_KEY) as string;
        }

        private void SetCacheId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                CallContext.FreeNamedDataSlot(CONTEXT_KEY);
            }
            else
            {
                CallContext.LogicalSetData(CONTEXT_KEY, id);
            }
            
        }
#else
        private static readonly AsyncLocal<string> _localId = new AsyncLocal<string>();
        private string GetCacheId()
        {
            return _localId.Value;
        }

        private void SetCacheId(string id)
        {
            _localId.Value = id;
        }
#endif

        public IUnitOfWork Current
        {
            get { return GetCurrentUow(); }
            set { SetCurrentUow(value); }
        }
    }
}