namespace FeatureFlipper.Unity
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ObjectBuilder2;

    public class FlippingBuilderStrategy : BuilderStrategy
    {
        private readonly IDictionary<Type, object> nullObjectCache = new Dictionary<Type, object>();

        private readonly ProxyGenerator generator = new ProxyGenerator();

        private readonly IFeatureFlipper flipper;

        private readonly FeatureNameProvider nameProvider = new FeatureNameProvider();

        public FlippingBuilderStrategy(IFeatureFlipper flipper)
        {
            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            this.flipper = flipper;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            string featureName = this.nameProvider.GetFeatureName(context.BuildKey.Type);
            bool isOn;
            if (context.Existing == null && this.flipper.TryIsOn(featureName, out isOn) && !isOn)
            {
                Type fromType = context.OriginalBuildKey.Type;
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

            base.PreBuildUp(context);
        }
    }
}
