namespace FeatureFlipper.Unity
{
    using Microsoft.Practices.Unity;

    public static class UnityContainerExtensions
    {
        public static void AddFeatureFlippingExtension(this IUnityContainer container, IFeatureFlipper flipper)
        {
            container.RegisterInstance(typeof(IFeatureFlipper), flipper);
            container.AddNewExtension<FeatureFlipperExtension>();
        }

        public static void AddFeatureFlippingExtension(this IUnityContainer container)
        {
            container.RegisterType(typeof(IFeatureFlipper), new InjectionFactory(c => Features.Flipper));
            container.AddNewExtension<FeatureFlipperExtension>();
        }
    }
}
