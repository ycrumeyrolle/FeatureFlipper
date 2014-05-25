﻿namespace FeatureFlipper.Unity
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

        private readonly IDictionary<Type, IDictionary<Type, string>> featureVersionMapping = new Dictionary<Type, IDictionary<Type, string>>();

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
            this.Context.Registering += OnRegistering;

            this.Context.Strategies.Add(flippingBuilderStrategy, UnityBuildStage.PreCreation);

            VersionSelectionStrategy versionSelectionStrategy = new VersionSelectionStrategy(this.flipper, this.featureVersionMapping);
            this.Context.Strategies.Add(versionSelectionStrategy, UnityBuildStage.TypeMapping);
        }

        void OnRegistering(object sender, RegisterEventArgs e)
        {
            IDictionary<Type, string> featureMapper;
            if (!featureVersionMapping.TryGetValue(e.TypeFrom, out featureMapper))
            {
                featureMapper = new Dictionary<Type, string>();
                featureVersionMapping.Add(e.TypeFrom, featureMapper);
            }

            featureMapper.Add(e.TypeTo, e.Name);
        }
    }
}
