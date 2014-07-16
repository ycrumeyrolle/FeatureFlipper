namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using FeatureFlipper.Properties;
     
    /// <summary>
    /// This implementation of the <see cref="IFeatureProvider"/> tries to get the state of the feature into a configuration repository.
    /// Then it tries to determinate state of the feature with the help of the <see cref="IFeatureStateParser"/>.
    /// </summary>
    public sealed class ConfigurationFeatureProvider : IFeatureProvider
    {
        private readonly IDictionary<string, ConfigurationFeatureContext> repository = new Dictionary<string, ConfigurationFeatureContext>();

        private readonly IConfigurationReader configurationReader;

        private readonly IList<IFeatureStateParser> featureStateProviders = new List<IFeatureStateParser>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationFeatureProvider"/> class.
        /// </summary>
        /// <param name="configurationReader">The <see cref="IConfigurationReader"/> used to read values.</param>
        /// <param name="featureStateProviders">The list of <see cref="IFeatureStateParser"/> used to parse the state of the features.</param>
        public ConfigurationFeatureProvider(IConfigurationReader configurationReader, IEnumerable<IFeatureStateParser> featureStateProviders)
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
        
        /// <inheritsdoc />
        public bool TryIsOn(FeatureMetadata metadata, out bool isOn)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            ConfigurationFeatureContext context;
            string key;
            if (this.repository.TryGetValue(metadata.Name, out context))
            {
                key = context.Key;
            }
            else
            {
                key = metadata.Name;
            }

            return this.TryIsOnCore(key, metadata.Version, out isOn);
        }

        private bool TryIsOnCore(string key, string version, out bool isOn)
        {
            isOn = false;
            string value = this.configurationReader.GetValue(key);
            if (value == null)
            {
                return false;
            }

            for (int i = 0; i < this.featureStateProviders.Count; i++)
            {
                if (this.featureStateProviders[i].TryParse(value, version, out isOn))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Registers a feature. It map a feature name with a configuration key.
        /// </summary>
        /// <param name="feature">The name of the feature.</param>
        /// <param name="configurationKey">The configuration key.</param>
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
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Feature_AlreadyRegistered, feature));
            }

            ConfigurationFeatureContext context = new ConfigurationFeatureContext(configurationKey);

            this.repository.Add(feature, context);
        }
    }
}
