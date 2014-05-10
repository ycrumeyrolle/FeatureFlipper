namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class DefaultFeatureFlipper : IFeatureFlipper
    {
        private readonly IList<IFeatureProvider> providers = new List<IFeatureProvider>();

        private IDictionary<string, IFeatureProvider> fastCache = new Dictionary<string, IFeatureProvider>();

        public DefaultFeatureFlipper(IEnumerable<IFeatureProvider> providers)
        {
            if (providers == null)
            {
                throw new ArgumentNullException("providers");
            }

            foreach (var provider in providers)
            {
                this.providers.Add(provider);
            }
        }

        public ICollection<IFeatureProvider> Providers
        {
            get { return this.providers; }
        }

        public bool TryIsOn(string feature, out bool isOn)
        {
            if (feature == null)
            {
                throw new ArgumentNullException("feature");
            }

            IFeatureProvider provider;
            if (this.fastCache.TryGetValue(feature, out provider))
            {
                if (provider.TryIsOn(feature, out isOn))
                {
                    return true;
                }
            }

            for (int i = 0; i < this.providers.Count; i++)
            {
                provider = this.providers[i];
                if (provider.TryIsOn(feature, out isOn))
                {
                    this.fastCache.Add(feature, provider);
                    return true;
                }
            }

            isOn = false;
            return false;
        }
    }
}
