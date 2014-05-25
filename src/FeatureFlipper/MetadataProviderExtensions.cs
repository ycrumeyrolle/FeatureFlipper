namespace FeatureFlipper
{
    using System;

    /// <summary>
    /// Provides extensions methods to the <see cref="IMetadataProvider"/>.
    /// </summary>
    public static class MetadataProviderExtensions
    {
        /// <summary>
        /// Gets the metadata of a feature.
        /// </summary>
        /// <param name="metadataProvider">The <see cref="IMetadataProvider"/>.</param>
        /// <param name="feature">The name of the feature.</param>
        /// <returns>The <see cref="FeatureMetadata"/> of the feature.</returns>
        public static FeatureMetadata GetMetadata(this IMetadataProvider metadataProvider, string feature)
        {
            if (metadataProvider == null)
            {
                throw new ArgumentNullException("metadataProvider");
            }

            return metadataProvider.GetMetadata(feature, null);
        }
    }
}
