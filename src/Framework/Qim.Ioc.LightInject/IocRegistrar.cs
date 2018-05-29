using System;
using LightInject;

namespace Qim.Ioc.LightInject
{
    internal class IocRegistrar : DisposableObject, IIocRegistrar
    {
        private readonly IServiceContainer _container;

        public IocRegistrar(IServiceContainer serviceContainer)
        {
            _container = serviceContainer;
        }

        #region protected

        protected override void Dispose(bool disposing)
        {
            _container.Dispose();
        }

        protected ILifetime GetLifetime(LifetimeType lifetime)
        {
            switch (lifetime)
            {
                case LifetimeType.Transient:
                    return new PerRequestLifeTime();
                case LifetimeType.Singleton:
                    return new PerContainerLifetime();
                case LifetimeType.Scoped:
                    return new PerScopeLifetime();
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
        }

        #endregion

        #region IIocRegistrar

        public void RegisterType(Type serviceType, Type implementationType, object constructorArgsAsAnonymousType = null)
        {
            RegisterType(serviceType, implementationType, string.Empty, LifetimeType.Transient,
                constructorArgsAsAnonymousType);
        }

        public void RegisterType(Type serviceType, Type implementationType, LifetimeType lifetime,
            object constructorArgsAsAnonymousType = null)
        {
            RegisterType(serviceType, implementationType, string.Empty, lifetime, constructorArgsAsAnonymousType);
        }

        public void RegisterType(Type serviceType, Type implementationType, string name, LifetimeType lifetime,
            object constructorArgsAsAnonymousType = null)
        {
            Ensure.NotNull(serviceType, nameof(serviceType));
            Ensure.NotNull(implementationType, nameof(implementationType));

            name = name ?? string.Empty;
            if (constructorArgsAsAnonymousType == null)
            {
                _container.Register(serviceType, implementationType, name, GetLifetime(lifetime));
            }
            else
            {
                _container.Register(new ServiceRegistration
                {
                    ServiceType = serviceType,
                    FactoryExpression =
                        ConstructorArgsHelper.GetFactoryDelegate(serviceType, implementationType, name,
                            constructorArgsAsAnonymousType),
                    ServiceName = name,
                    Lifetime = GetLifetime(lifetime)
                });
            }
        }

        public void RegisterType(Type implementationType, string name = null,
            LifetimeType lifetime = LifetimeType.Transient,
            object constructorArgsAsAnonymousType = null)
        {
            RegisterType(implementationType, implementationType, name, lifetime, constructorArgsAsAnonymousType);
        }

        public void Register<TService, TImplementer>(object constructorArgsAsAnonymousType = null)
            where TService : class where TImplementer : class, TService
        {
            RegisterType(typeof(TService), typeof(TImplementer), string.Empty, LifetimeType.Transient,
                constructorArgsAsAnonymousType);
        }

        public void Register<TService, TImplementer>(LifetimeType lifetime, object constructorArgsAsAnonymousType = null)
            where TService : class where TImplementer : class, TService
        {
            RegisterType(typeof(TService), typeof(TImplementer), string.Empty, lifetime,
                constructorArgsAsAnonymousType);
        }

        public void Register<TService, TImplementer>(string name, LifetimeType lifetime,
            object constructorArgsAsAnonymousType = null) where TService : class where TImplementer : class, TService
        {
            RegisterType(typeof(TService), typeof(TImplementer), name, lifetime, constructorArgsAsAnonymousType);
        }

        public void Register<TImplementer>(string name = null, LifetimeType lifetime = LifetimeType.Transient,
            object constructorArgsAsAnonymousType = null) where TImplementer : class
        {
            RegisterType(typeof(TImplementer), typeof(TImplementer), name, lifetime, constructorArgsAsAnonymousType);
        }

        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            RegisterInstance(typeof(TService), instance);
        }

        public void RegisterInstance(Type serviceType, object instance)
        {
            Ensure.NotNull(instance, nameof(instance));
            _container.RegisterInstance(serviceType, instance);
        }

        #endregion
    }
}