namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Default implementation of the <see cref="IFeatureFlipper"/>.
    /// </summary>
    public sealed class DefaultFeatureFlipper : IFeatureFlipper
    {
        private readonly IList<IFeatureProvider> providers = new List<IFeatureProvider>();

        private readonly IDictionary<string, IFeatureProvider[]> fastCache = new Dictionary<string, IFeatureProvider[]>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFeatureFlipper"/> class.
        /// </summary>
        /// <param name="providers">The feature providers used to get the features.</param>
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

        /// <inheritsdoc />
        public ICollection<IFeatureProvider> Providers
        {
            get { return this.providers; }
        }

        /// <inheritsdoc />
        public bool TryIsOn(string feature, out bool isOn)
        {
            if (feature == null)
            {
                throw new ArgumentNullException("feature");
            }

            IFeatureProvider[] providerCache;
            bool partialIsOn = false;
            isOn = true;
            if (this.fastCache.TryGetValue(feature, out providerCache))
            {
                for (int i = 0;i < providerCache.Length;i++)
                {
                    if (providerCache[i].TryIsOn(feature, out partialIsOn))
                    {
                        isOn &= partialIsOn;
                    }
                }

                isOn = partialIsOn;
                return true;
            }

            return this.TryIsOnCore(feature, out isOn);
        }

        private bool TryIsOnCore(string feature, out bool isOn)
        {
            isOn = true;
            bool partialIsOn;
            bool found = false;
            List<IFeatureProvider> providerCache = new List<IFeatureProvider>();
            for (int i = 0;i < this.providers.Count;i++)
            {
                var provider = this.providers[i];
                if (provider.TryIsOn(feature, out partialIsOn))
                {
                    providerCache.Add(provider);
                    isOn &= partialIsOn;
                    found = true;
                }
            }

            this.fastCache.Add(feature, providerCache.ToArray());
            return found;
        }
    }
}
