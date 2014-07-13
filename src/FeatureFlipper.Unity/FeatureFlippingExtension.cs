namespace FeatureFlipper.Unity
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    /// <summary>
    /// This extension installs the <see cref="FlippingBuilderStrategy"/> into the container to implement feature flipping behavior.
    /// </summary>
    public sealed class FeatureFlippingExtension : UnityContainerExtension
    {
        private readonly IFeatureFlipper flipper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureFlippingExtension"/> class.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        public FeatureFlippingExtension(IFeatureFlipper flipper)
        {
            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            this.flipper = flipper;
        }

        /// <inheritsdoc/>
        protected override void Initialize()
        {
            FlippingBuilderStrategy flippingBuilderStrategy = new FlippingBuilderStrategy(this.flipper);

            this.Context.Strategies.Add(flippingBuilderStrategy, UnityBuildStage.TypeMapping);
        }
    }
}
