namespace FeatureFlipper.Unity
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Practices.ObjectBuilder2;

    /// <summary>
    /// A <see cref="BuilderStrategy"/> that handles feature versions toggle.
    /// </summary>
    public sealed class VersionSelectionStrategy : BuilderStrategy
    {
        private readonly IFeatureFlipper flipper;

        private readonly FeatureNameProvider nameProvider = new FeatureNameProvider();

        private readonly IDictionary<Type, IList<TypeMapping>> featureVersionMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlippingBuilderStrategy"/> class.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        /// <param name="featureVersionMapping">The from-to-verion type mapping.</param>
        public VersionSelectionStrategy(IFeatureFlipper flipper, IDictionary<Type, IList<TypeMapping>> featureVersionMapping)
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

            if (context.Existing == null)
            {
                IList<TypeMapping> mapping;
                if (this.featureVersionMapping.TryGetValue(fromType, out mapping))
                {
                    for (int i = 0; i < mapping.Count; i++)
                    {
                        var map = mapping[i];
                        bool isOn;
                        string featureName = this.nameProvider.GetFeatureName(map.Type);
                        if (this.flipper.TryIsOn(featureName, map.Name, out isOn) && !isOn)
                        {
                            context.BuildKey = new NamedTypeBuildKey(map.Type, map.Name);
                            break;
                        }
                    }
                }
            }

            base.PreBuildUp(context);
        }
    }
}
