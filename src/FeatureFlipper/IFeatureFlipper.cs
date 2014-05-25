namespace FeatureFlipper
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a feature flipper. Its role is to determine whether the feature is <c>On</c> or <c>Off</c>.
    /// </summary>
    public interface IFeatureFlipper
    {
        /// <summary>
        /// Gets the <see cref="IFeatureProvider"/> collection. 
        /// </summary>
        ICollection<IFeatureProvider> Providers { get; }

        /// <summary>
        /// Tries to get the state of the feature.
        /// </summary>
        /// <param name="feature">The name of the feature.</param>
        /// <param name="version">Optionnal. The version of the feature.</param>
        /// <param name="isOn">
        ///  When this method returns, if the feature is found, contains true if the feature is <c>On</c>
        ///  or false if the feature is <c>Off</c>.
        ///  If the feature is not found, contains false. 
        /// </param>
        /// <returns><c>true</c> if the feature is found; otherwise, <c>false</c>.</returns>   
        bool TryIsOn(string feature, string version, out bool isOn);
    }
}
