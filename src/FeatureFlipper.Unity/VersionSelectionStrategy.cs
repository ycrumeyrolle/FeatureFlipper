namespace FeatureFlipper.Unity
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ObjectBuilder2;

    /// <summary>
    /// A <see cref="BuilderStrategy"/> that handles feature versions toggle.
    /// </summary>
    public sealed class VersionSelectionStrategy : BuilderStrategy
    {
        private readonly IFeatureFlipper flipper;

        private readonly FeatureNameProvider nameProvider = new FeatureNameProvider();

        private readonly IDictionary<Type, IDictionary<Type, string>> featureVersionMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlippingBuilderStrategy"/> class.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        /// <param name="featureVersionMapping">The from-to-verion type mapping.</param>
        public VersionSelectionStrategy(IFeatureFlipper flipper, IDictionary<Type, IDictionary<Type, string>> featureVersionMapping)
        {
            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            if (featureVersionMapping == null)
            {
                throw new ArgumentNullException("featureVersionMapping");
            }

            this.flipper = flipper;
            this.featureVersionMapping = featureVersionMapping;
        }

        /// <inheritsdoc/>
        public override void PreBuildUp(IBuilderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            Type fromType = context.OriginalBuildKey.Type;

            string featureName = this.nameProvider.GetFeatureName(context.BuildKey.Type);
            if (context.Existing == null)
            {
                IDictionary<Type, string> mapping;
                if (this.featureVersionMapping.TryGetValue(fromType, out mapping))
                {
                    foreach (var item in mapping)
                    {
                        bool isOn;
                        featureName = this.nameProvider.GetFeatureName(item.Key);
                        if (this.flipper.TryIsOn(featureName, item.Value, out isOn) && !isOn)
                        {
                            context.BuildKey = new NamedTypeBuildKey(item.Key, item.Value);
                            break;
                        }
                    }
                }
            }

            base.PreBuildUp(context);
        }
    }
}
