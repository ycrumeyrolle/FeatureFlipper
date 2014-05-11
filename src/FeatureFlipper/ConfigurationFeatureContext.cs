namespace FeatureFlipper
{
    using System;

    /// <summary>
    /// Represents the context of the feature determination.
    /// </summary>
    public sealed class ConfigurationFeatureContext
    {  
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationFeatureContext"/> class.
        /// </summary>
        public ConfigurationFeatureContext(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            this.Key = key;
        }

        /// <summary>
        /// Gets the configuration key.
        /// </summary>
        public string Key { get; private set; }
    }
}
