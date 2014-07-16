namespace FeatureFlipper.CycleDetection
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides method to detect cycles dependencies in a list of features.
    /// </summary>
    public interface ICycleDetector
    {
        /// <summary>
        /// Detects whether a feature has dependencies cycles.
        /// </summary>
        /// <returns>A list of cycles, represented as a string.</returns>
        IEnumerable<string> DetectCycles(IEnumerable<FeatureMetadata> features);
    }
}
