namespace FeatureFlipper
{
    using System.Collections.Concurrent;
    using System.Configuration;

    /// <summary>
    /// Default implementation of <see cref="IConfigurationReader"/>.
    /// Its read its configuration in the AppSettings configuration section.
    /// </summary>
    public sealed class DefaultConfigurationReader : IConfigurationReader
    {
        private readonly ConcurrentDictionary<string, string> cache = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Gets a value from a given key.
        /// </summary>
        /// <param name="key">The key of a feature.</param>
        /// <returns>The value of the feature. <c>null</c> if the key is unknown.</returns>
        public string GetValue(string key)
        {
            string value;
            if (!this.cache.TryGetValue(key, out value))
            {                
                value = ConfigurationManager.AppSettings[key];
                this.cache.TryAdd(key, value);
            }

            return value;
        }
    }
}
