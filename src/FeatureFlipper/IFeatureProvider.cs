namespace FeatureFlipper
{ 
    /// <summary>
    /// Represents a feature provider. Its role is to try to find a feature and to determinate whether the feature is <c>On</c> or <c>Off</c>.
    /// </summary>
    public interface IFeatureProvider
    {
        /// <summary>
        /// Tries to get the state of the feature.
        /// </summary>
        /// <param name="feature">The name of the feature.</param>
        /// <param name="isOn">
        ///  When this method returns, if the feature is found, contains true if the feature is <c>On</c>
        ///  or false if the feature is <c>Off</c>.
        ///  If the feature is not found, contains false. 
        /// </param>
        /// <returns><c>true</c> if the feature is found; otherwise, <c>false</c>.</returns>   
        bool TryIsOn(string feature, string version, out bool isOn);
    }
}
