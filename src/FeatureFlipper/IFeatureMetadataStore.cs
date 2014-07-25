namespace FeatureFlipper
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a store for metadata/
    /// </summary>
    public interface IFeatureMetadataStore
    {
        /// <summary>
        /// Gets the store value.
        /// </summary>
        IDictionary<string, Dictionary<string, FeatureMetadata>> Value { get; }
    }
}