using System;
using System.Collections.Generic;
using Autofac;

namespace Qim.Ioc.Autofac
{
    internal class IocResolver : DisposableObject, IIocResolver
    {
        private readonly ILifetimeScopeProvider _provider;
        public IocResolver(ILifetimeScopeProvider provider)
        {
            _provider = provider;
        }

        #region protected

        protected virtual ILifetimeScope Scope => _provider.LifetimeScope;

        protected override void Dispose(bool disposing)
        {
            Scope?.Dispose();
        }

        #endregion

        #region IIocResolver

        public TService GetService<TService>(object constructorArgsAsAnonymousType = null) where TService : class
        {
            return GetService<TService>(null, constructorArgsAsAnonymousType);
        }

        public TService GetService<TService>(string name, object constructorArgsAsAnonymousType = null)
            where TService : class
        {
            var @params = Extensions.GeTypedParameters(constructorArgsAsAnonymousType);
            return !string.IsNullOrEmpty(name) ? Scope.ResolveNamed<TService>(name, @params) : Scope.Resolve<TService>(@params);
        }

        public object GetService(Type serviceType, object constructorArgsAsAnonymousType = null)
        {
            return GetService(serviceType, null, constructorArgsAsAnonymousType);
        }

        public object GetService(Type serviceType, string name, object constructorArgsAsAnonymousType = null)
        {
            var @params = Extensions.GeTypedParameters(constructorArgsAsAnonymousType);
            if (!string.IsNullOrEmpty(name))
            {
                return Scope.ResolveNamed(name, serviceType, @params);
            }
            return Scope.Resolve(serviceType, @params);
        }

        public IEnumerable<TService> GetAllServices<TService>()
            where TService : class
        {
            return
                Scope.ResolveOptional<IEnumerable<TService>>();
        }

        public object GetOptionalService(Type serviceType, string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Scope.ResolveOptional(serviceType);
            }
            object result;
            Scope.TryResolveNamed(name, serviceType, out result);
            return result;
        }

        public TService GetOptionalService<TService>(string name = null)
        {
            return (TService)GetOptionalService(typeof(TService), name);
        }

        #endregion
    }
}