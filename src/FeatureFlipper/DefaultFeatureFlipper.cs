namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Default implementation of the <see cref="IFeatureFlipper"/>.
    /// </summary>
    public sealed class DefaultFeatureFlipper : IFeatureFlipper
    {
        private readonly IList<IFeatureProvider> providers = new List<IFeatureProvider>();

        private readonly IMetadataProvider metadataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFeatureFlipper"/> class.
        /// </summary>
        /// <param name="providers">The feature providers used to get the features.</param>
        /// <param name="metadataProvider">The <see cref="IMetadataProvider"/> to provides features metadata.</param>
        public DefaultFeatureFlipper(IEnumerable<IFeatureProvider> providers, IMetadataProvider metadataProvider)
        {
            if (providers == null)
            {
                throw new ArgumentNullException("providers");
            }

            if (metadataProvider == null)
            {
                throw new ArgumentNullException("metadataProvider");
            }

            foreach (var provider in providers)
            {
                this.providers.Add(provider);
            }

            this.metadataProvider = metadataProvider;
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

            FeatureContext context = new FeatureContext();
            FeatureMetadata metadata = this.metadataProvider.GetMetadata(feature, version);
            if (metadata == null)
            {
                metadata = new FeatureMetadata(feature, version, typeof(void), null, null);
            }

            context.Metadata = metadata;

            return this.TryIsOnCore(context, out isOn);
        }

        private bool TryIsOnCore(FeatureContext context, out bool isOn)
        {
            if (this.TryIsOnDependencies(context, out isOn))
            {
                if (!isOn)
                {
                    return true;
                }
            }
            else
            {
                isOn = false;
                return false;
            }

            return this.TryIsOnCore(context.Metadata, out isOn);
        }

        private bool TryIsOnDependencies(FeatureContext context, out bool isOn)
        {
            context.Visited.Add(context.Metadata.Name);
            var dependsOn = context.Metadata.GetDependsOn();
            isOn = true;
            for (int i = 0; i < dependsOn.Length; i++)
            {
                string featureName = dependsOn[i];
                if (context.Visited.Contains(featureName))
                {
                    continue;
                }
                               
                FeatureContext overrideContext = new FeatureContext
                {
                    Metadata = this.metadataProvider.GetMetadata(featureName, context.Metadata.Version),
                    Visited = context.Visited
                };
                if (!this.TryIsOnCore(overrideContext, out isOn))
                {
                    return false;
                }
            }

            return true;
        }

        private bool TryIsOnCore(FeatureMetadata metadata, out bool isOn)
        {
            bool found = false;
            isOn = true;
            for (int i = 0; i < this.providers.Count; i++)
            {
                var provider = this.providers[i];
                bool partialIsOn;
                if (provider.TryIsOn(metadata, out partialIsOn))
                {
                    isOn &= partialIsOn;
                    found = true;
                }
            }

            return found;
        }

        private class FeatureContext
        {
            public FeatureContext()
            {
                this.Visited = new HashSet<string>();
            }

            public HashSet<string> Visited { get; set; }

            public FeatureMetadata Metadata { get; set; }
        }
    }
}
