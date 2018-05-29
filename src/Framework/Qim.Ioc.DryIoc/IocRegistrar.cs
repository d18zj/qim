using System;
using System.Reflection;
using DryIoc;
using Qim.Ioc.Interception;

namespace Qim.Ioc.DryIoc
{
    internal class IocRegistrar : DisposableObject, IIocRegistrar
    {
        private readonly IContainer _container;
        private readonly Type _interceptType;
        public IocRegistrar(IContainer container)
        {
            _container = container;
            _interceptType = typeof(IInterceptingTarget);
        }

        #region IIocRegistrar


        public void RegisterType(Type serviceType, Type implementationType, object constructorArgsAsAnonymousType = null)
        {
            RegisterType(serviceType, implementationType, null, LifetimeType.Transient,
                constructorArgsAsAnonymousType);
        }

        public void RegisterType(Type serviceType, Type implementationType, LifetimeType lifetime,
            object constructorArgsAsAnonymousType = null)
        {
            RegisterType(serviceType, implementationType, null, lifetime,
                constructorArgsAsAnonymousType);
        }

        public void RegisterType(Type serviceType, Type implementationType, string name, LifetimeType lifetime = LifetimeType.Transient,
            object constructorArgsAsAnonymousType = null)
        {
            RegisterService(serviceType, implementationType, name, lifetime, constructorArgsAsAnonymousType);
        }

        public void RegisterType(Type implementationType, string name = null, LifetimeType lifetime = LifetimeType.Transient, object constructorArgsAsAnonymousType = null)
        {
            Ensure.NotNull(implementationType, nameof(implementationType));
            Made made = null;
            if (constructorArgsAsAnonymousType != null)
            {
                made = GetParameterSelector(constructorArgsAsAnonymousType);
            }
            _container.Register(implementationType, ConvertLifetimeToReuse(lifetime), made: made, serviceKey: name);
            if (_interceptType.IsAssignableFrom(implementationType)) //虚方法拦截
            {
                PipelineManager.Instance.InterceptByVirtualMethod(implementationType);

                _container.Intercept<DynamicProxyInterceptor>(implementationType, name);
            }
        }

        public void Register<TService, TImplementer>(object constructorArgsAsAnonymousType = null)
            where TService : class where TImplementer : class, TService
        {
            RegisterType(typeof(TService), typeof(TImplementer), null, LifetimeType.Transient,
                constructorArgsAsAnonymousType);
        }

        public void Register<TService, TImplementer>(LifetimeType lifetime, object constructorArgsAsAnonymousType = null)
            where TService : class where TImplementer : class, TService
        {
            RegisterType(typeof(TService), typeof(TImplementer), null, lifetime,
                constructorArgsAsAnonymousType);
        }

        public void Register<TService, TImplementer>(string name, LifetimeType lifetime = LifetimeType.Transient,
            object constructorArgsAsAnonymousType = null) where TService : class where TImplementer : class, TService
        {
            RegisterType(typeof(TService), typeof(TImplementer), name, lifetime,
                constructorArgsAsAnonymousType);
        }

        public void Register<TImplementer>(string name = null, LifetimeType lifetime = LifetimeType.Transient,
            object constructorArgsAsAnonymousType = null) where TImplementer : class
        {
            RegisterType(typeof(TImplementer), name, lifetime, constructorArgsAsAnonymousType);
        }

        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            RegisterInstance(typeof(TService), instance);
        }

        public void RegisterInstance(Type serviceType, object instance)
        {
            Ensure.NotNull(serviceType, nameof(serviceType));
            Ensure.NotNull(instance, nameof(instance));
            _container.UseInstance(serviceType, instance);
        }

        public void RegisterDelegate(Type serviceType, Func<IIocResolver, object> factoryDelegate,
            LifetimeType lifetime = LifetimeType.Transient,
            string name = null)
        {
            Ensure.NotNull(serviceType, nameof(serviceType));
            Ensure.NotNull(factoryDelegate, nameof(factoryDelegate));
            _container.RegisterDelegate(serviceType, r => factoryDelegate(r.Resolve<IIocResolver>()),
                ConvertLifetimeToReuse(lifetime), serviceKey: name);
        }

        public void Replace(Type serviceType, Type implementationType, string name = null,
            LifetimeType lifetime = LifetimeType.Transient, object constructorArgsAsAnonymousType = null)
        {
            RegisterService(serviceType, implementationType, name, lifetime, constructorArgsAsAnonymousType, IfAlreadyRegistered.Replace);
        }

        #endregion

        #region protected & private

        protected IReuse ConvertLifetimeToReuse(LifetimeType lifetime)
        {
            switch (lifetime)
            {
                case LifetimeType.Transient:
                    return Reuse.Transient;
                case LifetimeType.Singleton:
                    return Reuse.InCurrentNamedScope(Container.NonAmbientRootScopeName);
                case LifetimeType.Scoped:
                    return Reuse.InCurrentScope;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _container.Dispose();
        }

        private void RegisterService(Type serviceType, Type implementationType, string name, LifetimeType lifetime = LifetimeType.Transient,
            object constructorArgsAsAnonymousType = null, IfAlreadyRegistered ifAlreadyRegistered = IfAlreadyRegistered.AppendNotKeyed)
        {
            Ensure.NotNull(serviceType, nameof(serviceType));
            Ensure.NotNull(implementationType, nameof(implementationType));
            Made made = null;
            if (constructorArgsAsAnonymousType != null)
            {
                made = GetParameterSelector(constructorArgsAsAnonymousType);
            }

            _container.Register(serviceType: serviceType, implementationType: implementationType,
                reuse: ConvertLifetimeToReuse(lifetime), made: made, serviceKey: name, ifAlreadyRegistered: ifAlreadyRegistered);

            //接口拦截
            if (_interceptType.IsAssignableFrom(serviceType) && serviceType.GetTypeInfo().IsInterface)
            {
                PipelineManager.Instance.InterceptByInterface(implementationType);
                _container.Intercept<DynamicProxyInterceptor>(serviceType, name);
            }
            else if (_interceptType.IsAssignableFrom(implementationType)) //虚方法拦截
            {
                PipelineManager.Instance.InterceptByVirtualMethod(implementationType);
                _container.Intercept<DynamicProxyInterceptor>(implementationType, name);
            }
        }
        private ParameterSelector GetParameterSelector(object constructorArgsAsAnonymousType)
        {
            var selector = Parameters.Of;
            foreach (
                var propertyInfo in
                constructorArgsAsAnonymousType.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                selector = selector.Name(propertyInfo.Name,
                    defaultValue: propertyInfo.GetValue(constructorArgsAsAnonymousType));
            }
            return selector;
        }



        #endregion
    }
}