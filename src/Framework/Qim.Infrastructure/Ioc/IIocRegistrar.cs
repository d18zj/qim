using System;

namespace Qim.Ioc
{
    /// <summary>
    ///     IocRegistrar interface
    /// </summary>
    public interface IIocRegistrar
    {
        /// <summary>
        ///     Register a implementer type as a service implementation.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="constructorArgsAsAnonymousType">The implementation type constructor params.</param>
        void RegisterType(Type serviceType, Type implementationType, object constructorArgsAsAnonymousType = null);


        /// <summary>
        ///     Register a implementer type as a service implementation.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="lifetime">The life cycle of the implementer type.</param>
        /// <param name="constructorArgsAsAnonymousType">The implementation type constructor params.</param>
        void RegisterType(Type serviceType, Type implementationType, LifetimeType lifetime, object constructorArgsAsAnonymousType = null);


        /// <summary>
        ///     Register a implementer type as a service implementation.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="name">Name to use for registration, null if a default registration.</param>
        /// <param name="lifetime">The life cycle of the implementer type.</param>
        /// <param name="constructorArgsAsAnonymousType">The implementation type constructor params.</param>
        void RegisterType(Type serviceType, Type implementationType, string name, LifetimeType lifetime = LifetimeType.Transient, object constructorArgsAsAnonymousType = null);


        /// <summary>
        ///     Register a maybe implementation service type
        /// </summary>
        /// <param name="implementationType"></param>
        /// <param name="name">Name to use for registration, null if a default registration.</param>
        /// <param name="lifetime"></param>
        /// <param name="constructorArgsAsAnonymousType"></param>
        void RegisterType(Type implementationType, string name = null, LifetimeType lifetime = LifetimeType.Transient, object constructorArgsAsAnonymousType = null);

        /// <summary>
        ///     Register a implementer type as a service implementation.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <typeparam name="TImplementer">The implementer type.</typeparam>
        /// <param name="constructorArgsAsAnonymousType">The implementation type constructor params.</param>
        void Register<TService, TImplementer>(object constructorArgsAsAnonymousType = null)
            where TService : class
            where TImplementer : class, TService;

        /// <summary>
        ///     Register a implementer type as a service implementation.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <typeparam name="TImplementer">The implementer type.</typeparam>
        /// <param name="lifetime">The life cycle of the implementer type.</param>
        /// <param name="constructorArgsAsAnonymousType">The implementation type constructor params.</param>
        void Register<TService, TImplementer>(LifetimeType lifetime, object constructorArgsAsAnonymousType = null)
            where TService : class
            where TImplementer : class, TService;


        /// <summary>
        ///     Register a implementer type as a service implementation.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <typeparam name="TImplementer">The implementer type.</typeparam>
        /// <param name="name">Name to use for registration, null if a default registration</param>
        /// <param name="lifetime">The life cycle of the implementer type.</param>
        /// <param name="constructorArgsAsAnonymousType">The implementation type constructor params.</param>
        void Register<TService, TImplementer>(string name, LifetimeType lifetime = LifetimeType.Transient, object constructorArgsAsAnonymousType = null)
            where TService : class
            where TImplementer : class, TService;


        /// <summary>
        ///     Register a service type  may be implementation.
        /// </summary>
        /// <typeparam name="TImplementer">The service type may be implementation.</typeparam>
        /// <param name="name">Name to use for registration, null if a default registration</param>
        /// <param name="lifetime">The life cycle of the implementer type.</param>
        /// <param name="constructorArgsAsAnonymousType">The implementation type constructor params.</param>
        void Register<TImplementer>(string name = null, LifetimeType lifetime = LifetimeType.Transient, object constructorArgsAsAnonymousType = null)
            where TImplementer : class;

        /// <summary>
        ///     Register a implementer type instance as a service implementation.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="instance">The implementer type instance.</param>
        void RegisterInstance<TService>(TService instance) where TService : class;

        /// <summary>
        ///     Register a implementer type instance as a service implementation
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="instance">The implementer type instance.</param>
        void RegisterInstance(Type serviceType, object instance);

        /// <summary>
        ///     Registers a factory delegate for creating an instance of serviceType.
        /// </summary>
        /// <param name="serviceType">The service type to register.</param>
        /// <param name="factoryDelegate">The delegate used to create a instance of serviceType.</param>
        /// <param name="lifetime">The life cycle of the serviceType.</param>
        /// <param name="name">Name to use for registration, null if a default registration</param>
        void RegisterDelegate(Type serviceType, Func<IIocResolver, object> factoryDelegate,
            LifetimeType lifetime = LifetimeType.Transient, string name = null);

        /// <summary>
        ///  Replacing a registered service
        /// </summary>
        /// <param name="serviceType">The registered service</param>
        /// <param name="implementationType">要替换的服务 </param>
        /// <param name="name"></param>
        /// <param name="lifetime"></param>
        /// <param name="constructorArgsAsAnonymousType"></param>
        void Replace(Type serviceType, Type implementationType, string name = null, LifetimeType lifetime = LifetimeType.Transient, object constructorArgsAsAnonymousType = null);

    }
}