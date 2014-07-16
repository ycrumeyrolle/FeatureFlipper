namespace FeatureFlipper
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using FeatureFlipper.Properties;

    /// <summary>
    /// Represents the container of services commonly used by FeatureFlipper
    /// </summary>
    public class ServiceContainer
    {
        private readonly ConcurrentDictionary<Type, object> table = new ConcurrentDictionary<Type, object>();
        private readonly ConcurrentDictionary<Type, Func<object>> factories = new ConcurrentDictionary<Type, Func<object>>();

        private readonly ConcurrentDictionary<Type, object[]> tableMulti = new ConcurrentDictionary<Type, object[]>();
        private readonly ConcurrentDictionary<Type, Func<IEnumerable<object>>> factoriesMulti = new ConcurrentDictionary<Type, Func<IEnumerable<object>>>();
  
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceContainer"/> class.
        /// </summary>
        public ServiceContainer()
        {
            this.factories[typeof(ISystemClock)] = () => new SystemClock();
            this.factories[typeof(IConfigurationReader)] = () => new DefaultConfigurationReader();
            this.factories[typeof(IPrincipalProvider)] = () => new DefaultPrincipalProvider();

            this.factoriesMulti[typeof(IFeatureStateParser)] = () => new List<IFeatureStateParser> 
            { 
                new BooleanFeatureStateParser(),
                new DateFeatureStateParser(this.GetService<ISystemClock>()),
                new VersionStateParser()
            };

            this.factories[typeof(IAssembliesResolver)] = () => new DefaultAssembliesResolver();
            this.factories[typeof(IMetadataProvider)] = () => new DataAnnotationMetadataProvider(this.GetService<ITypeResolver>());
            this.factories[typeof(ITypeResolver)] = () => new FeatureTypeResolver(this.GetService<IAssembliesResolver>());
            this.factories[typeof(IRoleMatrixProvider)] = () => new DefaultRoleMatrixProvider();
            this.factoriesMulti[typeof(IFeatureProvider)] = () => new List<IFeatureProvider> 
            { 
                new ConfigurationFeatureProvider(this.GetService<IConfigurationReader>(), this.GetServices<IFeatureStateParser>()),
                new RoleFeatureProvider(this.GetService<IRoleMatrixProvider>(), this.GetService<IPrincipalProvider>())
            };

            this.factories[typeof(IFeatureFlipper)] = () => new DefaultFeatureFlipper(this.GetServices<IFeatureProvider>(), this.GetService<IMetadataProvider>());
        }
        
        /// <summary>
        /// Provides a service instance given a service type.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <returns>The service instance.</returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            object service;
            if (this.table.TryGetValue(serviceType, out service))
            {
                return service;
            }

            Func<object> factory;
            if (this.factories.TryGetValue(serviceType, out factory))
            {
                service = factory();
                this.table.TryAdd(serviceType, service);
                return service;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Service_Unsupported, serviceType.Name));
        }

        /// <summary>
        /// Provides services instances given a service type.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <returns>The list of the service instances.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            object[] services;
            if (this.tableMulti.TryGetValue(serviceType, out services))
            {
                return services;
            }

            Func<IEnumerable<object>> factory;
            if (this.factoriesMulti.TryGetValue(serviceType, out factory))
            {
                var newServices = factory();
                if (newServices == null)
                {
                    newServices = new object[0];
                }

                services = newServices.ToArray();
                this.tableMulti.TryAdd(serviceType, services);
                return services;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Service_Unsupported, serviceType.Name));
        }

        /// <summary>
        /// Replaces a service by an other.
        /// </summary>
        /// <param name="serviceType">The type of the service to replace.</param>
        /// <param name="serviceFactory">The <see cref="Func{T}"/> factory creating the service.</param>
        public void Replace(Type serviceType, Func<object> serviceFactory)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            if (serviceFactory == null)
            {
                throw new ArgumentNullException("serviceFactory");
            }

            object service;
            if (!this.table.TryRemove(serviceType, out service) && !this.factories.ContainsKey(serviceType))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Service_Invalid, serviceType.Name), "serviceType");
            }

            this.factories[serviceType] = serviceFactory;
        }

        /// <summary>
        /// Replaces a service collection by an other.
        /// </summary>
        /// <param name="serviceType">The type of the service to replace.</param>
        /// <param name="serviceFactories">The <see cref="Func{T}"/> factory creating the services.</param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Func<> pattern.")]
        public void Replace(Type serviceType, Func<IEnumerable<object>> serviceFactories)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            if (serviceFactories == null)
            {
                throw new ArgumentNullException("serviceFactories");
            }

            if (!this.factoriesMulti.ContainsKey(serviceType))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Service_Invalid, serviceType.Name), "serviceType");
            }

            object[] services;
            this.tableMulti.TryRemove(serviceType, out services);
            this.factoriesMulti[serviceType] = serviceFactories;
        }
        
        /// <summary>
        /// Removes all services collection of a given type.
        /// </summary>
        /// <param name="serviceType">The type of the service to remove.</param>
        public void RemoveAll(Type serviceType)
        {
            this.Replace(serviceType, () => new object[0]);
        }        
    }
}