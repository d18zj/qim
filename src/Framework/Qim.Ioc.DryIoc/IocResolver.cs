using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using DryIoc;

namespace Qim.Ioc.DryIoc
{
    internal class IocResolver : DisposableObject, IIocResolver
    {
        private readonly IContainer _container;

        public IocResolver(IContainer container)
        {
            _container = container;
        }


        #region IIocResolver

        public TService GetService<TService>(object constructorArgsAsAnonymousType = null) where TService : class
        {
            return (TService)GetService(typeof(TService), null, constructorArgsAsAnonymousType);
        }

        public TService GetService<TService>(string name, object constructorArgsAsAnonymousType = null)
            where TService : class
        {
            return (TService)GetService(typeof(TService), name, constructorArgsAsAnonymousType);
        }

        public object GetService(Type serviceType, object constructorArgsAsAnonymousType = null)
        {
            return GetService(serviceType, null, constructorArgsAsAnonymousType);
        }

        public object GetService(Type serviceType, string name, object constructorArgsAsAnonymousType = null)
        {
            Ensure.NotNull(serviceType, nameof(serviceType));
            if (constructorArgsAsAnonymousType == null)
            {
                return _container.Resolve(serviceType, name);
            }

            var typeList = new List<Type>();
            var args = new List<object>();

            foreach (var p in constructorArgsAsAnonymousType.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                typeList.Add(p.PropertyType);
                args.Add(p.GetValue(constructorArgsAsAnonymousType));
            }

            typeList.Add(serviceType);
            var funcType = Expression.GetFuncType(typeList.ToArray());
            var func = (Delegate)_container.Resolve(funcType, name);
            return func.DynamicInvoke(args.ToArray());
        }

        public IEnumerable<TService> GetAllServices<TService>()
            where TService : class
        {
            return _container.ResolveMany<TService>();
        }

        public object GetOptionalService(Type serviceType, string name = null)
        {
            Ensure.NotNull(serviceType, nameof(serviceType));
            return _container.Resolve(serviceType, serviceKey: name, ifUnresolved: IfUnresolved.ReturnDefault);
        }

        public TService GetOptionalService<TService>(string name = null)
        {
            return (TService)GetOptionalService(typeof(TService), name);
        }

        #endregion

        #region protected

        //protected object InjectProperty(object instance)
        //{
        //    if (instance != null)
        //    {

        //        _container.InjectPropertiesAndFields(instance, PropertiesAndFields.All(withNonPublic: false, withPrimitive: false, withFields: false, withInfo:
        //            (member, request) =>
        //            {
        //                if (member.IsDefined(typeof(InjectAttribute), true))
        //                {
        //                    return PropertyOrFieldServiceInfo.Of(member)
        //                        .WithDetails(ServiceDetails.Of(ifUnresolved: IfUnresolved.ReturnDefault), request);
        //                }
        //                return null;
        //            }));
        //    }
        //    return instance;
        //}

        protected override void Dispose(bool disposing)
        {
            _container.Dispose();
        }




        #endregion

    }
}