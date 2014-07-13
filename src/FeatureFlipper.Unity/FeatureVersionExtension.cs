namespace FeatureFlipper.Unity
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    /// <summary>
    /// This extension installs the <see cref="VersionSelectionStrategy"/> into the container to implement feature versioning behavior.
    /// </summary>
    public sealed class FeatureVersionExtension : UnityContainerExtension
    {
        private readonly IFeatureFlipper flipper;

        private readonly IDictionary<Type, TypeMappingCollection> featureVersionMapping = new Dictionary<Type, TypeMappingCollection>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureVersionExtension"/> class.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        public FeatureVersionExtension(IFeatureFlipper flipper)
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
            this.Context.Registering += this.OnRegistering;
            
            VersionSelectionStrategy versionSelectionStrategy = new VersionSelectionStrategy(this.flipper, this.featureVersionMapping);
            this.Context.Strategies.Add(versionSelectionStrategy, UnityBuildStage.TypeMapping);
        }

        private void OnRegistering(object sender, RegisterEventArgs e)
        {
            if (e.TypeFrom != null && e.Name != null)
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
}
