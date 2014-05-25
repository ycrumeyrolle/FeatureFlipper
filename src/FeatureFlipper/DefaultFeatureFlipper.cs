namespace FeatureFlipper
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Default implementation of the <see cref="IFeatureFlipper"/>.
    /// </summary>
    public sealed class DefaultFeatureFlipper : IFeatureFlipper
    {
        private readonly IList<IFeatureProvider> providers = new List<IFeatureProvider>();

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
        public bool TryIsOn(string feature, string version, out bool isOn)
        {
            if (feature == null)
            {
                throw new ArgumentNullException("feature");
            }

            isOn = true;
            bool partialIsOn;
            bool found = false;
            for (int i = 0; i < this.providers.Count; i++)
            {
                var provider = this.providers[i];
                if (provider.TryIsOn(feature, version, out partialIsOn))
                {
                    isOn &= partialIsOn;
                    found = true;
                }
            }

            return found;
        }
    }
}
