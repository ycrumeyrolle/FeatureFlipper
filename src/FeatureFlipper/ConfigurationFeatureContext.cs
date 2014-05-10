namespace FeatureFlipper
{
    using System;

    public sealed class ConfigurationFeatureContext
    {
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
