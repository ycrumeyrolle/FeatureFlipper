namespace FeatureFlipper.Unity
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    /// <summary>
    /// This extension installs the <see cref="FlippingBuilderStrategy"/> into the container to implement feature flipping behavior.
    /// </summary>
    public sealed class FeatureFlipperExtension : UnityContainerExtension
    {
        private readonly IFeatureFlipper flipper;

        private readonly IDictionary<Type, TypeMappingCollection> featureVersionMapping = new Dictionary<Type, TypeMappingCollection>();

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
            FlippingBuilderStrategy flippingBuilderStrategy = new FlippingBuilderStrategy(this.flipper);
            this.Context.Registering += this.OnRegistering;

            this.Context.Strategies.Add(flippingBuilderStrategy, UnityBuildStage.PreCreation);

            VersionSelectionStrategy versionSelectionStrategy = new VersionSelectionStrategy(this.flipper, this.featureVersionMapping);
            this.Context.Strategies.Add(versionSelectionStrategy, UnityBuildStage.TypeMapping);
        }

        private void OnRegistering(object sender, RegisterEventArgs e)
        {
            TypeMappingCollection featureMapper;
            if (!this.featureVersionMapping.TryGetValue(e.TypeFrom, out featureMapper))
            {
                featureMapper = new TypeMappingCollection();
                this.featureVersionMapping.Add(e.TypeFrom, featureMapper);
            }

            featureMapper.Add(new TypeMapping { FeatureType = e.TypeTo, FeatureName = e.Name });
        }
    }
}
