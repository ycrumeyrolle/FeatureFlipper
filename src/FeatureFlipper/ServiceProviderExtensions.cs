﻿namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Provides extension methods for <see cref="ServiceContainer"/>.
    /// </summary>
    public static class ServiceProviderExtensions
    {   
        /// <summary>
        /// Provides a service instance given a service type.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="serviceContainer">The <see cref="ServiceContainer"/></param>
        /// <returns>The service instance.</returns>
        public static TService GetService<TService>(this ServiceContainer serviceContainer)
        {
            if (serviceContainer == null)
            {
                throw new ArgumentNullException("serviceContainer");
            }

            Type serviceType = typeof(TService);
            return (TService)serviceContainer.GetService(serviceType);
        }

        /// <summary>
        /// Provides services instances instance given a service type.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="serviceContainer">The <see cref="ServiceContainer"/></param>
        /// <returns>The list of the service instances.</returns>
        public static IEnumerable<TService> GetServices<TService>(this ServiceContainer serviceContainer) where TService : class
        {
            if (serviceContainer == null)
            {
                throw new ArgumentNullException("serviceContainer");
            }

            Type serviceType = typeof(TService);
            return serviceContainer.GetServices(serviceType).Cast<TService>();
        }
      
        /// <summary>
        /// Replaces a service collection by an other.
        /// </summary>
        /// <param name="serviceContainer">The <see cref="ServiceContainer"/></param>
        /// <param name="serviceType">The type of the service to replace.</param>
        /// <param name="services">The services instances.</param>
        public static void Replace(this ServiceContainer serviceContainer, Type serviceType, IEnumerable<object> services)
        {
            if (serviceContainer == null)
            {
                throw new ArgumentNullException("serviceContainer");
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            if (services == null)
            {
                throw new ArgumentNullException("services");
            }

            serviceContainer.Replace(serviceType, services.ToArray);
        }

        /// <summary>
        /// Replaces a service by an other.
        /// </summary>
        /// <param name="serviceContainer">The <see cref="ServiceContainer"/></param>
        /// <param name="serviceType">The type of the service to replace.</param>
        /// <param name="service">The service instance.</param>
        public static void Replace(this ServiceContainer serviceContainer, Type serviceType, object service)
        {
            if (serviceContainer == null)
            {
                throw new ArgumentNullException("serviceContainer");
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            serviceContainer.Replace(serviceType, () => service);
        }

        /// <summary>
        /// Replaces a service by an other.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="serviceContainer">The <see cref="ServiceContainer"/></param>
        /// <param name="service">The service instance.</param>
        public static void Replace<TService>(this ServiceContainer serviceContainer, TService service) where TService : class
        {
            if (serviceContainer == null)
            {
                throw new ArgumentNullException("serviceContainer");
            }

            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            Type serviceType = typeof(TService);

            serviceContainer.Replace(serviceType, () => service);
        }
        
        /// <summary>
        /// Adds a service to a service collection.
        /// </summary>
        /// <param name="serviceContainer">The <see cref="ServiceContainer"/></param>
        /// <param name="serviceType">The type of the service to replace.</param>
        /// <param name="service">The service instance.</param>
        /// <remarks>
        /// <see cref="IFeatureStateParser"/> and <see cref="IFeatureProvider"/> are the only possible types.
        /// </remarks>
        public static void Add(this ServiceContainer serviceContainer, Type serviceType, object service)
        {
            if (serviceContainer == null)
            {
                throw new ArgumentNullException("serviceContainer");
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            var services = serviceContainer.GetServices(serviceType);

            serviceContainer.Replace(serviceType, () => services.Concat(new[] { service }));
        }

        /// <summary>
        /// Adds a service to a service collection.
        /// </summary>
        /// <param name="serviceContainer">The <see cref="ServiceContainer"/></param>
        /// <param name="service">The service instance.</param>
        /// <typeparam name="TService">The service type</typeparam>
        /// <remarks>
        /// <see cref="IFeatureStateParser"/> and <see cref="IFeatureProvider"/> are the only possible types.
        /// </remarks>
        public static void Add<TService>(this ServiceContainer serviceContainer, TService service) where TService : class
        {
            if (serviceContainer == null)
            {
                throw new ArgumentNullException("serviceContainer");
            }

            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            serviceContainer.Add(typeof(TService), service);
        }

        /// <summary>
        /// Adds a <see cref="IFeatureStateParser"/> to the service collection.
        /// </summary>
        /// <param name="serviceContainer">The <see cref="ServiceContainer"/></param>
        /// <param name="featureStateParser">The service instance.</param>
        public static void AddFeatureStateParser(this ServiceContainer serviceContainer, IFeatureStateParser featureStateParser)
        {
            serviceContainer.Add(featureStateParser);
        }

        /// <summary>
        /// Adds a <see cref="IFeatureProvider"/> to the service collection.
        /// </summary>
        /// <param name="serviceContainer">The <see cref="ServiceContainer"/></param>
        /// <param name="featureProvider">The service instance.</param>
        public static void AddFeatureProvider(this ServiceContainer serviceContainer, IFeatureProvider featureProvider)
        {
            serviceContainer.Add(featureProvider);
        }

        /// <summary>
        /// Removes all services collection of a given type.
        /// </summary>
        /// <param name="serviceContainer">The <see cref="ServiceContainer"/></param>
        /// <typeparam name="TService">The type of the service to remove.</typeparam>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design")]
        public static void RemoveAll<TService>(this ServiceContainer serviceContainer) where TService : class
        {
            if (serviceContainer == null)
            {
                throw new ArgumentNullException("serviceContainer");
            }

            serviceContainer.RemoveAll(typeof(TService));
        }
    }
}