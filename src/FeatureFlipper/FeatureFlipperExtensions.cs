namespace FeatureFlipper
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;

    public static class FeatureFlipperExtensions
    {
        private static readonly FeatureNameProvider NameProvider = new FeatureNameProvider();

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design")]
        public static bool IsOn<TFeature>(this IFeatureFlipper flipper)
        {
            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            return flipper.IsOn(typeof(TFeature).FullName);
        }

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

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design")]
        public static void RegisterConfigurationFeature<TFeature>(this IFeatureFlipper flipper)
        {
            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            string feature = NameProvider.GetFeatureName(typeof(TFeature));
            flipper.RegisterConfigurationFeature(feature, feature);
        }

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

        public static void RegisterConfigurationFeature(this IFeatureFlipper flipper, string feature)
        {
            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            flipper.RegisterConfigurationFeature(feature, feature);
        }

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

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design")]
        public static void RegisterPerRoleFeature<TFeature>(this IFeatureFlipper flipper, string role)
        {
            RegisterPerRoleFeature(flipper, typeof(TFeature), role);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design")]
        public static void RegisterPerRoleFeature<TFeature>(this IFeatureFlipper flipper, string role, bool denied)
        {
            RegisterPerRoleFeature(flipper, typeof(TFeature), role, denied);
        }

        public static void RegisterPerRoleFeature(this IFeatureFlipper flipper, Type featureType, string role)
        {
            string feature = NameProvider.GetFeatureName(featureType);
            RegisterPerRoleFeature(flipper, feature, role);
        }

        public static void RegisterPerRoleFeature(this IFeatureFlipper flipper, Type featureType, string role, bool denied)
        {
            string feature = NameProvider.GetFeatureName(featureType);
            RegisterPerRoleFeature(flipper, feature, role, denied);
        }

        public static void RegisterPerRoleFeature(this IFeatureFlipper flipper, string feature, string role)
        {
            RegisterPerRoleFeature(flipper, feature, role, denied: false);
        }

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
