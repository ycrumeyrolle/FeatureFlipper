namespace FeatureFlipper.Unity
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class FeatureFlipperExtension : UnityContainerExtension
    {
        private readonly IFeatureFlipper flipper;

        public FeatureFlipperExtension(IFeatureFlipper flipper)
        {
            this.flipper = flipper;
        }

        protected override void Initialize()
        {
            FlippingBuilderStrategy strategy = new FlippingBuilderStrategy(this.flipper);
            this.Context.Strategies.Add(strategy, UnityBuildStage.TypeMapping);
        }
    }
}
