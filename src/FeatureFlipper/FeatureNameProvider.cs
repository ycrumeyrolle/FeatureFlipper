namespace FeatureFlipper
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// Represents a provider that gives a name for a feature.
    /// </summary>
    public sealed class FeatureNameProvider
    {
        private readonly ConcurrentDictionary<Type, string> featureCache = new ConcurrentDictionary<Type, string>();

        /// <summary>
        /// Provides the name of a feature. 
        /// </summary>
        /// <param name="featureType">The <see cref="Type"/> of the feature.</param>
        /// <returns>The name of the feature.</returns>
        public string GetFeatureName(Type featureType)
        {
            if (featureType == null)
            {
                throw new ArgumentNullException("featureType");
            }

            string featureName;
            if (!this.TryGetFeatureName(featureType, out featureName))
            {
                featureName = featureType.FullName;
                this.featureCache.TryAdd(featureType, featureName);
            }

            return featureName;
        }

        private bool TryGetFeatureName(Type featureType, out string featureName)
        {
            if (!this.featureCache.TryGetValue(featureType, out featureName))
            {
                var attributes = featureType.GetCustomAttributes(typeof(FeatureAttribute), true);
                if (attributes.Length != 0)
                {
                    featureName = ((FeatureAttribute)attributes[0]).Name;
                    this.featureCache.TryAdd(featureType, featureName);
                    return true;
                }

                featureName = null;
                this.featureCache.TryAdd(featureType, null);
                return false;
            }

            return featureName != null;
        }
    }
}
