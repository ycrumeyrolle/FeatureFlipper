namespace FeatureFlipper.Unity
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    /// <summary>
    /// This extension installs the <see cref="FlippingBuilderStrategy"/> into the container to implement feature flipping behavior.
    /// </summary>
    public sealed class FeatureFlipperExtension : UnityContainerExtension
    {
        private readonly IFeatureFlipper flipper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureFlipperExtension"/> class.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        public FeatureFlipperExtension(IFeatureFlipper flipper)
        {
            this.flipper = flipper;
        }

        /// <inheritsdoc/>
        protected override void Initialize()
        {
            FlippingBuilderStrategy strategy = new FlippingBuilderStrategy(this.flipper);
            this.Context.Strategies.Add(strategy, UnityBuildStage.TypeMapping);
        }
    }
}
