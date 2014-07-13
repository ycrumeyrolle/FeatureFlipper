namespace FeatureFlipper.Unity
{
    using System;
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
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            FeatureFlippingExtension extension = new FeatureFlippingExtension(flipper);
            container.AddExtension(extension);
        }

        /// <summary>
        /// Add the feature flipping behavior to the unity container.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/>.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "ContainerControlledLifetimeManager does not need to be disposed without value.")]
        public static void AddFeatureFlippingExtension(this IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            FeatureFlippingExtension extension = new FeatureFlippingExtension(Features.Flipper);
            container.AddExtension(extension);
        }
        
        /// <summary>
        /// Add the feature versioning behavior to the unity container.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/>.</param>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        public static void AddFeatureVersioningExtension(this IUnityContainer container, IFeatureFlipper flipper)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            FeatureVersionExtension extension = new FeatureVersionExtension(flipper);
            container.AddExtension(extension);
        }

        /// <summary>
        /// Add the feature versioning behavior to the unity container.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/>.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "ContainerControlledLifetimeManager does not need to be disposed without value.")]
        public static void AddFeatureVersioningExtension(this IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            FeatureVersionExtension extension = new FeatureVersionExtension(Features.Flipper);
            container.AddExtension(extension);
        }
    }
}
