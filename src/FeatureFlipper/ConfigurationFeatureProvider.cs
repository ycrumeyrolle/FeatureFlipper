namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;

    public sealed class ConfigurationFeatureProvider : IFeatureProvider
    {
        private readonly IDictionary<string, ConfigurationFeatureContext> repository = new Dictionary<string, ConfigurationFeatureContext>();

        private readonly IConfigurationReader configurationReader;

        private readonly IList<IFeatureStateParser> featureStateProviders = new List<IFeatureStateParser>();

        public ConfigurationFeatureProvider(IConfigurationReader configurationReader, ICollection<IFeatureStateParser> featureStateProviders)
        {
            if (configurationReader == null)
            {
                throw new ArgumentNullException("configurationReader");
            }

            if (featureStateProviders == null)
            {
                throw new ArgumentNullException("featureStateProviders");
            }

            this.configurationReader = configurationReader;
            foreach (var provider in featureStateProviders)
            {
                this.featureStateProviders.Add(provider);
            }
        }

        public ConfigurationFeatureProvider(IConfigurationReader configurationReader)
            : this(configurationReader, Features.FeatureStateParsers)
        {
        }

        public ConfigurationFeatureProvider()
            : this(new DefaultConfigurationReader())
        {
        }

        public bool TryIsOn(string feature, out bool isOn)
        {
            if (feature == null)
            {
                throw new ArgumentNullException("feature");
            }

            ConfigurationFeatureContext context;
            string key;
            if (this.repository.TryGetValue(feature, out context))
            {
                key = context.Key;
            }
            else
            {
                key = feature;
            }

            return this.TryIsOnCore(key, out isOn);
        }

        private bool TryIsOnCore(string key, out bool isOn)
        {
            isOn = false;
            string value = this.configurationReader.GetValue(key);
            if (value == null)
            {
                isOn = false;
                return false;
            }

            for (int i = 0; i < this.featureStateProviders.Count; i++)
            {
                if (this.featureStateProviders[i].TryParse(value, out isOn))
                {
                    return true;
                }
            }

            return false;
        }

        public void RegisterFeature(string feature, string configurationKey)
        {
            if (feature == null)
            {
                throw new ArgumentNullException("feature");
            }

            if (configurationKey == null)
            {
                throw new ArgumentNullException("configurationKey");
            }

            if (this.repository.ContainsKey(feature))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The feature '{0}' is already registered.", feature));
            }

            ConfigurationFeatureContext context = new ConfigurationFeatureContext(configurationKey);

            this.repository.Add(feature, context);
        }
    }
}
