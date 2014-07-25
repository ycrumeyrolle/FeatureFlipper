namespace FeatureFlipper
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides a store for metadata.
    /// </summary>
    public interface IFeatureMetadataStore
    {
        /// <summary>
        /// Gets the store value.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "By design")]
        Dictionary<string, Dictionary<string, FeatureMetadata>> Value { get; }
    }
}