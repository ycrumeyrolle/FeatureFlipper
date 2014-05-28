namespace FeatureFlipper.Unity
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents the mapping between a feature type and a feature name.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "Equals is not used.")]
    public struct TypeMapping
    {
        /// <summary>
        /// Gets or sets the feature name.
        /// </summary>
        public string FeatureName { get; set; }

        /// <summary>
        /// Gets or sets the feature type.
        /// </summary>
        public Type FeatureType { get; set; }
    }
}
