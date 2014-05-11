namespace FeatureFlipper
{
    using System.Configuration;

    /// <summary>
    /// Default implementation of <see cref="IConfigurationReader"/>.
    /// Its read its configuration in the AppSettings configuration section.
    /// </summary>
    public class DefaultConfigurationReader : IConfigurationReader
    {
        /// <summary>
        /// Gets a value from a given key.
        /// </summary>
        /// <param name="key">The key of a feature.</param>
        /// <returns>The value of the feature. <c>null</c> if the key is unknown.</returns>
        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
