using System;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Qim.Ioc.Castle
{
    internal class IocManager : BaseIocResolver, IIocRegistrar
    {
        #region Fields

        private readonly bool _classicWeb;

        #endregion

        public IocManager(bool classicWeb = false)
            : base(new WindsorContainer())
        {
            _classicWeb = classicWeb;
        }

        public void RegisterType(Type serviceType, Type implementationType, object constructorArgsAsAnonymousType = null)
        {
            RegisterType(serviceType, implementationType, null, LifetimeType.Transient, constructorArgsAsAnonymousType);
        }

        public void RegisterType(Type serviceType, Type implementationType, LifetimeType lifetime,
            object constructorArgsAsAnonymousType = null)
        {
            RegisterType(serviceType, implementationType, null, lifetime, constructorArgsAsAnonymousType);
        }

        public void RegisterType(Type serviceType, Type implementationType, string name, LifetimeType lifetime,
            object constructorArgsAsAnonymousType = null)
        {
            var reg = Component.For(serviceType).ImplementedBy(implementationType).ApplyLifestyle(lifetime, _classicWeb);
            if (!string.IsNullOrEmpty(name))
                reg.Named(name);

            if (constructorArgsAsAnonymousType != null)
            {
                var argsType = constructorArgsAsAnonymousType.GetType();
                foreach (
                    var propertyInfo in argsType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    reg.DependsOn(Dependency.OnValue(propertyInfo.PropertyType,
                        propertyInfo.GetValue(constructorArgsAsAnonymousType)));
                }
            }

            if (IocContainer.Kernel.HasComponent(implementationType) && string.IsNullOrEmpty(name))
            {
                reg.Named(Guid.NewGuid().ToString()).IsDefault();
            }
            IocContainer.Register(reg);
        }

        public void Register<TService, TImplementer>(object constructorArgsAsAnonymousType = null)
            where TService : class
            where TImplementer : class, TService
        {
            Register<TService, TImplementer>(LifetimeType.Transient, constructorArgsAsAnonymousType);
        }

        public void Register<TService, TImplementer>(LifetimeType lifetime, object constructorArgsAsAnonymousType = null)
            where TService : class where TImplementer : class, TService
        {
            Register<TService, TImplementer>(null, lifetime, constructorArgsAsAnonymousType);
        }


        public void Register<TService, TImplementer>(string name, LifetimeType lifetime,
            object constructorArgsAsAnonymousType = null) where TService : class where TImplementer : class, TService
        {
            RegisterType(typeof(TService), typeof(TImplementer), name, lifetime, constructorArgsAsAnonymousType);
        }

        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            RegisterInstance(typeof(TService), instance);
        }

        public void RegisterInstance(Type serviceType, object instance)
        {
            IocContainer.Register(Component.For(serviceType).Instance(instance).IsDefault());
        }
    }
}