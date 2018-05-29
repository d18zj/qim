using System;
using System.Collections.Generic;
using Castle.Windsor;

namespace Qim.Ioc.Castle
{
    internal class BaseIocResolver : IIocResolver
    {
        protected BaseIocResolver(IWindsorContainer container)
        {
            IocContainer = container;
        }

        public IWindsorContainer IocContainer { get; }


        public object GetService(Type serviceType)
        {
            return GetServiceInternal(serviceType);
        }

        public TService GetService<TService>() where TService : class
        {
            return GetService<TService>(null);
        }

        public TService GetService<TService>(object overridedArguments) where TService : class
        {
            return GetService<TService>(null);
        }

        public TService GetService<TService>(string name, object overridedArguments = null) where TService : class
        {
            return (TService) GetServiceInternal(typeof(TService), name, overridedArguments);
        }

        public object GetService(Type serviceType, string name, object overridedArguments = null)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<TService> GetAllServices<TService>(object overridedArguments = null) where TService : class
        {
            return IocContainer.ResolveAll<TService>(overridedArguments);
        }

        protected virtual object GetServiceInternal(Type serviceType, string name = null,
            object overridedArguments = null, bool isOptional = true)
        {
            //using (MsLifetimeScope.Using(CurrentScope))
            //{
            // MS uses GetService<IEnumerable<TDesiredType>>() to get a collection.
            // This must be resolved with IWindsorContainer.ResolveAll();

            if (serviceType.IsGenericType && (serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                var allObjects = IocContainer.ResolveAll(serviceType.GenericTypeArguments[0]);
                Array.Reverse(allObjects);
                return allObjects;
            }

            if (!isOptional)
                return ResolveService(serviceType, name, overridedArguments);

            // A single service was requested.

            // Microsoft.Extensions.DependencyInjection is built to handle optional registrations.
            // However Castle Windsor throws a ComponentNotFoundException when a type wasn't registered.
            // For this reason we have to manually check if the type exists in Windsor.

            if (IocContainer.Kernel.HasComponent(serviceType))
                return ResolveService(serviceType, name, overridedArguments);

            //Not found
            return null;
            //}
        }


        protected virtual object ResolveService(Type serviceType, string name = null, object overridedArguments = null)
        {
            if (string.IsNullOrEmpty(name))
                return overridedArguments != null
                    ? IocContainer.Resolve(serviceType, overridedArguments)
                    : IocContainer.Resolve(serviceType);
            return overridedArguments != null
                ? IocContainer.Resolve(name, serviceType, overridedArguments)
                : IocContainer.Resolve(name, serviceType);
        }
    }
}