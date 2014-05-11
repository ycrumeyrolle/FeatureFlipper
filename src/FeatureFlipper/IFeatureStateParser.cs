namespace FeatureFlipper
{    
    /// <summary>
    /// Represents a parser able to give the state of a feature given a value.
    /// </summary>
    public interface IFeatureStateParser
    {
        /// <summary>
        /// Tries to parse the value of the feature.
        /// </summary>
        /// <param name="value">The value of the feature.</param>
        /// <param name="isOn">
        ///  When this method returns, if the feature is parsed, contains true if the feature is <c>On</c>
        ///  or false if the feature is <c>Off</c>.
        ///  If the feature is not parsed, contains false. 
        /// </param>
        /// <returns><c>true</c> if the feature is parsed; otherwise, <c>false</c>.</returns>   
        bool TryParse(string value, out bool isOn);
    }
}
