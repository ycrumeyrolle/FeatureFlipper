namespace FeatureFlipper
{
    using System;

    /// <summary>
    /// Provides extensions methods to the <see cref="IFeatureProvider"/>.
    /// </summary>
    public static class FeatureProviderExtensions
    {
        /// <summary>
        /// Tries to get the state of the feature.
        /// </summary>
        /// <param name="featureProvider">The <see cref="IFeatureProvider"/>.</param>
        /// <param name="feature">The name of the feature.</param>
        /// <param name="isOn">
        ///  When this method returns, if the feature is found, contains true if the feature is <c>On</c>
        ///  or false if the feature is <c>Off</c>.
        ///  If the feature is not found, contains false. 
        /// </param>
        /// <returns><c>true</c> if the feature is found; otherwise, <c>false</c>.</returns>   
        public static bool TryIsOn(this IFeatureProvider featureProvider, string feature, out bool isOn)
        {
            if (featureProvider == null)
            {
                throw new ArgumentNullException("featureProvider");
            }

            return featureProvider.TryIsOn(feature, null, out isOn);
        }
    }
}
