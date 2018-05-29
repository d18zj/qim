using System;
using System.Collections.Generic;
using LightInject;

namespace Qim.Ioc.LightInject
{
    internal class IocResolver : DisposableObject, IIocResolver
    {
        private readonly Lazy<Scope> _lazy;
        public IocResolver(Lazy<Scope> lazy)
        {
            _lazy = lazy;
        }

        protected IServiceFactory ServiceFactory => _lazy.Value;

        protected override void Dispose(bool disposing)
        {
            if (_lazy.IsValueCreated)
            {
                _lazy.Value.Dispose();
            }
        }

        #region IIocResolver


        public TService GetService<TService>(object constructorArgsAsAnonymousType = null) where TService : class
        {
            return (TService)GetService(typeof(TService), string.Empty);
        }

        public TService GetService<TService>(string name, object constructorArgsAsAnonymousType = null) where TService : class
        {
            return (TService)GetService(typeof(TService), string.Empty, constructorArgsAsAnonymousType);
        }

        public object GetService(Type serviceType, object constructorArgsAsAnonymousType = null)
        {
            return GetService(serviceType, string.Empty, constructorArgsAsAnonymousType);
        }

        public object GetService(Type serviceType, string name, object constructorArgsAsAnonymousType = null)
        {
            name = name ?? string.Empty;
            var args = ConstructorArgsHelper.GetConstructorArgs(serviceType, name, constructorArgsAsAnonymousType);
            if (args == null)
            {
                return ServiceFactory.GetInstance(serviceType, name);
            }
            return ServiceFactory.GetInstance(serviceType, name, args);
        }

        public IEnumerable<TService> GetAllServices<TService>() where TService : class
        {
            return (IEnumerable<TService>)GetService(typeof(IEnumerable<TService>), string.Empty);
        }

        #endregion


    }
}