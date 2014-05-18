namespace FeatureFlipper.Unity
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ObjectBuilder2;

    /// <summary>
    /// A <see cref="BuilderStrategy"/> that handles feature flipping.
    /// It verifies the state of the feature and build a NullObject if the feature is <c>Off</c>. Do nothing otherwise.
    /// </summary>
    public sealed class FlippingBuilderStrategy : BuilderStrategy
    {
        private readonly IDictionary<Type, object> nullObjectCache = new Dictionary<Type, object>();

        private readonly NullObjectGenerator generator = new NullObjectGenerator();

        private readonly IFeatureFlipper flipper;

        private readonly FeatureNameProvider nameProvider = new FeatureNameProvider();

        /// <summary>
        /// Initializes a new instance of the <see cref="FlippingBuilderStrategy"/> class.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        public FlippingBuilderStrategy(IFeatureFlipper flipper)
        {
            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            this.flipper = flipper;
        }

        /// <inheritsdoc/>
        public override void PreBuildUp(IBuilderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            Type fromType = context.OriginalBuildKey.Type;
            if (fromType.IsInterface)
            {
                string featureName = this.nameProvider.GetFeatureName(context.BuildKey.Type);
                bool isOn;
                if (context.Existing == null && this.flipper.TryIsOn(featureName, out isOn) && !isOn)
                {
                    object nullObject;
                    if (!this.nullObjectCache.TryGetValue(fromType, out nullObject))
                    {
                        Type nullType = this.generator.CreateNullObject(fromType);

                        context.BuildKey = new NamedTypeBuildKey(nullType, context.BuildKey.Name);
                        nullObject = Activator.CreateInstance(nullType);
                        this.nullObjectCache.Add(fromType, nullObject);
                    }

                    context.Existing = nullObject;
                    context.BuildComplete = true;
                }
            }

            base.PreBuildUp(context);
        }
    }
}
