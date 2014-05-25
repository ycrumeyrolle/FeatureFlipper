namespace FeatureFlipper
{
    using System;

    /// <summary>
    /// Provides extensions methods to the <see cref="IFeatureStateParser"/>.
    /// </summary>
    public static class FeatureStateParserExtensions
    {
        /// <summary>
        /// Tries to parse the value of the feature.
        /// </summary>
        /// <param name="featureStateParser">The <see cref="IFeatureStateParser"/>.</param>
        /// <param name="value">The value of the feature.</param>
        /// <param name="isOn">
        ///  When this method returns, if the feature is parsed, contains true if the feature is <c>On</c>
        ///  or false if the feature is <c>Off</c>.
        ///  If the feature is not parsed, contains false. 
        /// </param>
        /// <returns><c>true</c> if the feature is parsed; otherwise, <c>false</c>.</returns>   
        public static bool TryParse(this IFeatureStateParser featureStateParser, string value, out bool isOn)
        {
            if (featureStateParser == null)
            {
                throw new ArgumentNullException("featureStateParser");
            }

            return featureStateParser.TryParse(value, null, out isOn);
        }
    }
}
