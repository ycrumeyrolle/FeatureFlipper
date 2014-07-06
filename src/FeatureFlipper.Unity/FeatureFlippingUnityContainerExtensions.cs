namespace FeatureFlipper.Unity
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Provides extensions methods to the <see cref="IUnityContainer"/>.
    /// </summary>
    public static class FeatureFlippingUnityContainerExtensions
    {
        /// <summary>
        /// Add the feature flipping behavior to the unity container.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/>.</param>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        public static void AddFeatureFlippingExtension(this IUnityContainer container, IFeatureFlipper flipper)
        {
            container.RegisterInstance(typeof(IFeatureFlipper), flipper);
            container.AddNewExtension<FeatureFlippingExtension>();
        }

        /// <summary>
        /// Add the feature flipping behavior to the unity container.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/>.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "ContainerControlledLifetimeManager does not need to be disposed without value.")]
        public static void AddFeatureFlippingExtension(this IUnityContainer container)
        {
            container.RegisterType(typeof(IFeatureFlipper), new ContainerControlledLifetimeManager(), new InjectionFactory(c => Features.Flipper));
            container.AddNewExtension<FeatureFlippingExtension>();
        }
        
        /// <summary>
        /// Add the feature versioning behavior to the unity container.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/>.</param>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        public static void AddFeatureVersioningExtension(this IUnityContainer container, IFeatureFlipper flipper)
        {
            container.RegisterInstance(typeof(IFeatureFlipper), flipper);
            container.AddNewExtension<FeatureVersionExtension>();
        }

        /// <summary>
        /// Add the feature versioning behavior to the unity container.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/>.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "ContainerControlledLifetimeManager does not need to be disposed without value.")]
        public static void AddFeatureVersioningExtension(this IUnityContainer container)
        {
            container.RegisterType(typeof(IFeatureFlipper), new ContainerControlledLifetimeManager(), new InjectionFactory(c => Features.Flipper));
            container.AddNewExtension<FeatureVersionExtension>();
        }
    }
}
