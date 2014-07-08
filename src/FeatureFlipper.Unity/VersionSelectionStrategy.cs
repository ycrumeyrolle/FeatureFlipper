namespace FeatureFlipper.Unity
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
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

        private readonly IDictionary<Type, TypeMappingCollection> featureVersionMapping;

        private readonly ConcurrentDictionary<Type, object> nullObjectCache = new ConcurrentDictionary<Type, object>();

        private readonly NullObjectGenerator generator = new NullObjectGenerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="FlippingBuilderStrategy"/> class.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        /// <param name="featureVersionMapping">The from-to-verion type mapping.</param>
        public VersionSelectionStrategy(IFeatureFlipper flipper, IDictionary<Type, TypeMappingCollection> featureVersionMapping)
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
                TypeMappingCollection mapping;
                if (this.featureVersionMapping.TryGetValue(fromType, out mapping))
                {
                    bool enabled = false;
                    bool found = false;
                    for (int i = 0; i < mapping.Count; i++)
                    {
                        var map = mapping[i];
                        bool isOn;
                        string featureName = this.nameProvider.GetFeatureName(map.FeatureType);
                        if (this.flipper.TryIsOn(featureName, map.FeatureName, out isOn))
                        {
                            found = true;
                            if (isOn)
                            {
                                context.BuildKey = new NamedTypeBuildKey(map.FeatureType, map.FeatureName);
                                enabled = true;
                                break;
                            }
                        }
                    }

                    if (!enabled && found)
                    {
                        object nullObject;
                        if (!this.nullObjectCache.TryGetValue(fromType, out nullObject))
                        {
                            Type nullType = this.generator.CreateNullObject(fromType);

                            context.BuildKey = new NamedTypeBuildKey(nullType, context.BuildKey.Name);
                            nullObject = Activator.CreateInstance(nullType);
                            this.nullObjectCache.TryAdd(fromType, nullObject);
                        }

                        context.Existing = nullObject;
                        context.BuildComplete = true;
                    }
                }
            }

            base.PreBuildUp(context);
        }
    }
}