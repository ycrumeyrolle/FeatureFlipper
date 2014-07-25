namespace FeatureFlipper
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Provides all features.
    /// </summary>
    public interface IFeatureExplorer
    {
        /// <summary>
        /// Gets all features.
        /// </summary>
        /// <returns>The list of all <see cref="FeatureDescriptor"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Time consuming method.")]
        IReadOnlyCollection<FeatureDescriptor> GetFeatures();
    }
}