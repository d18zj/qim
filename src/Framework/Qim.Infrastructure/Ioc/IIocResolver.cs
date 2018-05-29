using System;
using System.Collections.Generic;

namespace Qim.Ioc
{
    public interface IIocResolver : IDisposable
    {

        /// <summary>
        ///     Get a service.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="constructorArgsAsAnonymousType">The overrided arguments</param>
        /// <returns></returns>
        TService GetService<TService>(object constructorArgsAsAnonymousType = null) where TService : class;

        /// <summary>
        ///     Get a service.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="name">The registration name for service</param>
        /// <param name="constructorArgsAsAnonymousType">The overrided arguments</param>
        /// <returns></returns>
        TService GetService<TService>(string name, object constructorArgsAsAnonymousType = null) where TService : class;


        /// <summary>
        ///     Get a service.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="constructorArgsAsAnonymousType">The overrided arguments</param>
        /// <returns></returns>
        object GetService(Type serviceType, object constructorArgsAsAnonymousType = null);

        /// <summary>
        ///     Get a service.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="name">The registration name for service</param>
        /// <param name="constructorArgsAsAnonymousType">The overrided arguments</param>
        /// <returns></returns>
        object GetService(Type serviceType, string name, object constructorArgsAsAnonymousType = null);

        /// <summary>
        ///     Get all service.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <returns></returns>
        IEnumerable<TService> GetAllServices<TService>() where TService : class;

        /// <summary>
        /// Get a optional service.
        /// if the service does not exists retun null.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="name">The registration name for service</param>
        /// <returns></returns>
        object GetOptionalService(Type serviceType, string name = null);

        /// <summary>
        ///     Get a optional service.
        /// if the service does not exists retun null.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <param name="name">The registration name for service</param>
        /// <returns></returns>
        TService GetOptionalService<TService>(string name = null);
    }
}