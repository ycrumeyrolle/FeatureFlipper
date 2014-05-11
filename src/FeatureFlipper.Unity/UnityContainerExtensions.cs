namespace FeatureFlipper.Unity
{
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Provides extensions methods to the <see cref="IUnityContainer"/>.
    /// </summary>
    public static class UnityContainerExtensions
    {
        /// <summary>
        /// Add the feature flipping behavior to the unity container.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/>.</param>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        public static void AddFeatureFlippingExtension(this IUnityContainer container, IFeatureFlipper flipper)
        {
            container.RegisterInstance(typeof(IFeatureFlipper), flipper);
            container.AddNewExtension<FeatureFlipperExtension>();
        }

        /// <summary>
        /// Add the feature flipping behavior to the unity container.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/>.</param>
        public static void AddFeatureFlippingExtension(this IUnityContainer container)
        {
            container.RegisterType(typeof(IFeatureFlipper), new InjectionFactory(c => Features.Flipper));
            container.AddNewExtension<FeatureFlipperExtension>();
        }
    }
}
