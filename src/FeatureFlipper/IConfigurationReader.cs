namespace FeatureFlipper
{
    /// <summary>
    /// Represents a configuration reader. 
    /// Its role is to provide a value from a configuration repository, like a configuration file, a database or a web service.
    /// </summary>
    public interface IConfigurationReader
    {
        /// <summary>
        /// Gets a value from a given key.
        /// </summary>
        /// <param name="key">The key of a feature.</param>
        /// <returns>The value of the feature. <c>null</c> if the key is unknown.</returns>
        string GetValue(string key);
    }
}
