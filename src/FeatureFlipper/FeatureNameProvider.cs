namespace FeatureFlipper
{
    using System;
    using System.Collections.Concurrent;

    public class FeatureNameProvider
    {
        private readonly ConcurrentDictionary<Type, string> featureCache = new ConcurrentDictionary<Type, string>();

        public string GetFeatureName(Type featureType)
        {
            if (featureType == null)
            {
                throw new ArgumentNullException("featureType");
            }

            string name;
            if (!this.featureCache.TryGetValue(featureType, out name))
            {
                var attributes = featureType.GetCustomAttributes(typeof(FeatureAttribute), true);
                if (attributes.Length != 0)
                {
                    name = ((FeatureAttribute)attributes[0]).Name;
                }
                else
                {
                    name = featureType.FullName;
                }

                this.featureCache.TryAdd(featureType, name);
            }

            return name;
        }
    }
}
