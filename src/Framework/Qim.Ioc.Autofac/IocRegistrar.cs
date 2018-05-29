using System;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Qim.Ioc.Interception;

namespace Qim.Ioc.Autofac
{
    internal class IocRegistrar : IIocRegistrar
    {
        #region Fields
        private readonly Type _interceptType;
        private ContainerBuilder _builder;
        private IContainer _container;
        private static readonly object _obj = new object();

        #endregion

        #region Ctor
        public IocRegistrar(ContainerBuilder builder)
        {
            _interceptType = typeof(IInterceptingTarget);
            _builder = builder;
        }
        #endregion


        //public void RegisterModule(IModule module)
        //{
        //    Ensure.NotNull(module, nameof(module));
        //    _builder.RegisterModule(module);
        //    EnsureNeedUpdate();
        //}

        #region IIocRegistrar

        public void RegisterType(Type serviceType, Type implementationType, object constructorArgsAsAnonymousType = null)
        {
            RegisterType(serviceType, implementationType, null, LifetimeType.Transient, constructorArgsAsAnonymousType);
        }

        public void RegisterType(Type serviceType, Type implementationType, LifetimeType lifetime,
            object constructorArgsAsAnonymousType = null)
        {
            RegisterType(serviceType, implementationType, null, lifetime, constructorArgsAsAnonymousType);
        }

        public void RegisterType(Type serviceType, Type implementationType, string name,
            LifetimeType lifetime = LifetimeType.Transient,
            object constructorArgsAsAnonymousType = null)
        {
            Ensure.NotNull(serviceType, nameof(serviceType));
            Ensure.NotNull(implementationType, nameof(implementationType));
            //var builder = _builder;

            var serviceTypeInfo = serviceType.GetTypeInfo();
            var registrationBuilder =
                serviceTypeInfo.IsGenericTypeDefinition
                    ? (IRegistrationBuilder<object, ReflectionActivatorData, dynamic>)
                    _builder.RegisterGeneric(implementationType)
                    : _builder.RegisterType(implementationType);

            if (serviceType != implementationType)
            {
                registrationBuilder.As(serviceType);
            }
            registrationBuilder.ConfigureLifecycle(lifetime);

            if (!string.IsNullOrEmpty(name))
            {
                registrationBuilder.Named(name, serviceType);
            }

            if (constructorArgsAsAnonymousType != null)
            {
                registrationBuilder.WithParameters(Extensions.GeTypedParameters(constructorArgsAsAnonymousType));
            }

            //接口拦截
            if (_interceptType.IsAssignableFrom(serviceType) && serviceType.GetTypeInfo().IsInterface)
            {
                registrationBuilder.EnableInterfaceInterceptors().InterceptedBy(typeof(DynamicProxyInterceptor));
            }
            else if (_interceptType.IsAssignableFrom(implementationType)) //虚方法拦截
            {
                registrationBuilder.EnableClassInterceptors().InterceptedBy(typeof(DynamicProxyInterceptor));
            }
            registrationBuilder.InjectProperties(implementationType);
            EnsureNeedUpdate();
            //builder.Update(Container);
            //builder.Build()
        }

        public void RegisterType(Type implementationType, string name = null,
            LifetimeType lifetime = LifetimeType.Transient, object constructorArgsAsAnonymousType = null)
        {
            RegisterType(implementationType, implementationType, name, lifetime, constructorArgsAsAnonymousType);
        }

        public void Register<TService, TImplementer>(object constructorArgsAsAnonymousType = null)
            where TService : class
            where TImplementer : class, TService
        {
            RegisterType(typeof(TService), typeof(TImplementer), string.Empty, LifetimeType.Transient,
                constructorArgsAsAnonymousType);
        }

        public void Register<TService, TImplementer>(LifetimeType lifetime, object constructorArgsAsAnonymousType = null)
            where TService : class where TImplementer : class, TService
        {
            RegisterType(typeof(TService), typeof(TImplementer), string.Empty, lifetime, constructorArgsAsAnonymousType);
        }


        public void Register<TService, TImplementer>(string name, LifetimeType lifetime = LifetimeType.Transient,
            object constructorArgsAsAnonymousType = null) where TService : class where TImplementer : class, TService
        {
            RegisterType(typeof(TService), typeof(TImplementer), name, lifetime, constructorArgsAsAnonymousType);
        }

        public void Register<TService>(string name = null, LifetimeType lifetime = LifetimeType.Transient,
            object constructorArgsAsAnonymousType = null) where TService : class
        {
            RegisterType(typeof(TService), typeof(TService), name, lifetime, constructorArgsAsAnonymousType);
        }

        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            RegisterInstance(typeof(TService), instance);
        }

        public void RegisterInstance(Type serviceType, object instance)
        {
            _builder.RegisterInstance(instance).As(serviceType).ExternallyOwned();
            EnsureNeedUpdate();
        }

        public void RegisterDelegate(Type serviceType, Func<IIocResolver, object> factoryDelegate,
            LifetimeType lifetime = LifetimeType.Transient,
            string name = null)
        {
            Ensure.NotNull(serviceType, nameof(serviceType));
            Ensure.NotNull(factoryDelegate, nameof(factoryDelegate));

            var registration =
                RegistrationBuilder.ForDelegate(serviceType,
                        (context, parameters) => factoryDelegate(context.Resolve<IIocResolver>()))
                    .ConfigureLifecycle(lifetime).CreateRegistration();
            _builder.RegisterComponent(registration);
        }

        public void Replace(Type serviceType, Type implementationType, string name,
            LifetimeType lifetime = LifetimeType.Transient, object constructorArgsAsAnonymousType = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Protectd

        internal ILifetimeScope UpdateContainer()
        {
            if (!NeedUpdate) return _container;
            lock (_obj)
            {
                if (!NeedUpdate) return _container;
                if (_container == null)
                {
                    _container = _builder.Build();
                }
                else
                {
                    //todo 
                    _builder.Update(_container);
                }
                NeedUpdate = false;
                _builder = new ContainerBuilder();
            }
            return _container;
        }

        protected void EnsureNeedUpdate()
        {
            if (!NeedUpdate)
            {
                lock (_obj)
                {
                    NeedUpdate = true;
                }
            }
        }

        protected bool NeedUpdate { get; set; } = true;

        #endregion
    }
}