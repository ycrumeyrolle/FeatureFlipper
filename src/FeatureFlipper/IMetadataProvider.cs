namespace FeatureFlipper
{
    /// <summary>
    /// Provides metadata for a feature.
    /// </summary>
    public interface IMetadataProvider
    {
        /// <summary>
        /// Gets the metadata of a feature.
        /// </summary>
        /// <param name="feature">The name of the feature.</param>
        /// <returns>The <see cref="FeatureMetadata"/> of the feature.</returns>
        FeatureMetadata GetMetadata(string feature, string version);
    }
}
