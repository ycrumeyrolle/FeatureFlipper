namespace FeatureFlipper
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Provides extensions methods to the <see cref="IFeatureFlipper"/>.
    /// </summary>
    public static class FeatureFlipperExtensions
    {
        private static readonly FeatureNameProvider NameProvider = new FeatureNameProvider();

        /// <summary>
        /// Gets the state of the feature.
        /// </summary>
        /// <typeparam name="TFeature">The type of the feature.</typeparam>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        /// <returns><c>true</c> if the feature is <c>On</c>; otherwise, <c>false</c>.</returns>   
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design")]
        public static bool IsOn<TFeature>(this IFeatureFlipper flipper)
        {
            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            return flipper.IsOn(typeof(TFeature).FullName);
        }

        /// <summary>
        /// Gets the state of the feature.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        /// <param name="featureType">The type of the feature.</param>
        /// <returns><c>true</c> if the feature is <c>On</c>; otherwise, <c>false</c>.</returns>   
        public static bool IsOn(this IFeatureFlipper flipper, Type featureType)
        {
            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            if (featureType == null)
            {
                throw new ArgumentNullException("featureType");
            }

            return flipper.IsOn(featureType.FullName);
        }

        /// <summary>
        /// Gets the state of the feature.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        /// <param name="feature">The name of the feature.</param>
         /// <returns><c>true</c> if the feature is <c>On</c>; otherwise, <c>false</c>.</returns>   
        public static bool IsOn(this IFeatureFlipper flipper, string feature)
        {
            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            bool isOn;
            if (flipper.TryIsOn(feature, out isOn))
            {
                return isOn;
            }

            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The feature '{0}' is unknown.", feature));
        }

        /// <summary>
        /// Registers a feature. It maps a feature type with a configuration key.
        /// </summary>
        /// <typeparam name="TFeature">The type of the feature.</typeparam>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        /// <param name="configurationKey">The configuration key.</param>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design")]
        public static void RegisterConfigurationFeature<TFeature>(this IFeatureFlipper flipper, string configurationKey)
        {
            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            string feature = NameProvider.GetFeatureName(typeof(TFeature));
            flipper.RegisterConfigurationFeature(feature, configurationKey);
        }

        /// <summary>
        /// Registers a feature. It maps a feature type with a configuration key.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        /// <param name="feature">The name of the feature.</param>
        /// <param name="configurationKey">The configuration key.</param>
        public static void RegisterConfigurationFeature(this IFeatureFlipper flipper, string feature, string configurationKey)
        {
            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            if (feature == null)
            {
                throw new ArgumentNullException("feature");
            }

            if (configurationKey == null)
            {
                throw new ArgumentNullException("configurationKey");
            }

            ConfigurationFeatureProvider provider = flipper.Providers.OfType<ConfigurationFeatureProvider>().FirstOrDefault();
            if (provider == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "No provider of type {0} were found in the Providers property. ", typeof(ConfigurationFeatureProvider).FullName));
            }

            provider.RegisterFeature(feature, configurationKey);
        }

        /// <summary>
        /// Registers a feature. It maps a feature name with a role.
        /// </summary>
        /// <typeparam name="TFeature">The type of the feature.</typeparam>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        /// <param name="role">The role that is allowed to access to the feature.</param>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design")]
        public static void RegisterPerRoleFeature<TFeature>(this IFeatureFlipper flipper, string role)
        {
            RegisterPerRoleFeature(flipper, typeof(TFeature), role);
        }

        /// <summary>
        /// Registers a feature. It maps a feature name with a role.
        /// </summary>
        /// <typeparam name="TFeature">The type of the feature.</typeparam>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        /// <param name="role">The role that is allowed or denied to access to the feature.</param>
        /// <param name="denied">Explicitly deny a feature to the <paramref name="role"/>.</param>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design")]
        public static void RegisterPerRoleFeature<TFeature>(this IFeatureFlipper flipper, string role, bool denied)
        {
            RegisterPerRoleFeature(flipper, typeof(TFeature), role, denied);
        }

        /// <summary>
        /// Registers a feature. It maps a feature name with a role.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        /// <param name="featureType">The type of the feature.</param>
        /// <param name="role">The role that is allowed to access to the feature.</param>
        public static void RegisterPerRoleFeature(this IFeatureFlipper flipper, Type featureType, string role)
        {
            string feature = NameProvider.GetFeatureName(featureType);
            RegisterPerRoleFeature(flipper, feature, role);
        }

        /// <summary>
        /// Registers a feature. It maps a feature name with a role.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        /// <param name="featureType">The type of the feature.</param>
        /// <param name="role">The role that is allowed or denied to access to the feature.</param>
        /// <param name="denied">Explicitly deny a feature to the <paramref name="role"/>.</param>
        public static void RegisterPerRoleFeature(this IFeatureFlipper flipper, Type featureType, string role, bool denied)
        {
            string feature = NameProvider.GetFeatureName(featureType);
            RegisterPerRoleFeature(flipper, feature, role, denied);
        }

        /// <summary>
        /// Registers a feature. It maps a feature name with a role.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        /// <param name="feature">The name of the feature.</param>
        /// <param name="role">The role that is allowed to access to the <paramref name="feature"/>.</param>
        public static void RegisterPerRoleFeature(this IFeatureFlipper flipper, string feature, string role)
        {
            RegisterPerRoleFeature(flipper, feature, role, denied: false);
        }

        /// <summary>
        /// Registers a feature. It maps a feature name with a role.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/>.</param>
        /// <param name="feature">The name of the feature.</param>
        /// <param name="role">The role that is allowed or denied to access to the <paramref name="feature"/>.</param>
        /// <param name="denied">Explicitly deny a feature to the <paramref name="role"/>.</param>
        public static void RegisterPerRoleFeature(this IFeatureFlipper flipper, string feature, string role, bool denied)
        {
            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            if (feature == null)
            {
                throw new ArgumentNullException("feature");
            }

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            PerRoleFeatureProvider provider = flipper.Providers.OfType<PerRoleFeatureProvider>().FirstOrDefault();
            if (provider == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "No provider of type {0} were found in the Providers property. ", typeof(PerRoleFeatureProvider).FullName));
            }

            provider.RegisterFeature(feature, role, denied);
        }
    }
}
