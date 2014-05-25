namespace FeatureFlipper
{
    using System;

    /// <summary>
    /// This implementation of <see cref="IFeatureStateParser"/> tries to parse a value into a boolean.
    /// </summary>
    public sealed class VersionStateParser : IFeatureStateParser
    {
        /// <summary>
        /// Tries to parse the value of the feature. It must be a valid representation of a <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="value">The value of the feature.</param>
        /// <param name="version">The version of the feature.</param>
        /// <param name="isOn">
        ///  When this method returns, if the feature is parsed, contains true if the version of the feature is <c>On</c>
        ///  or false if the feature is <c>Off</c>.
        ///  If the feature or the vers is not parsed, contains false. 
        /// </param>
        /// <returns><c>true</c> if the feature is parsed; otherwise, <c>false</c>.</returns>   
        public bool TryParse(string value, string version, out bool isOn)
        {
            if (value == null || version == null)
            {
                isOn = false;
                return false;
            }

            isOn = string.Equals(value, version, StringComparison.Ordinal);
            return true;
        }
    }
}
