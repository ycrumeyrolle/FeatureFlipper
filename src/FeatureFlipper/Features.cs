namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static class Features
    {
        private static ICollection<IFeatureStateParser> featureStateParsers = new Collection<IFeatureStateParser> 
        { 
            new BooleanFeatureStateParser(),
            new DateFeatureStateParser(new SystemClock())
        };

        private static ICollection<IFeatureProvider> providers = new Collection<IFeatureProvider> 
        { 
            new ConfigurationFeatureProvider(new DefaultConfigurationReader(), Features.featureStateParsers),
            new PerRoleFeatureProvider()
        };

        private static IFeatureFlipper flipperInstance;

        private static Lazy<IFeatureFlipper> flipper = new Lazy<IFeatureFlipper>(InitializeFlipper);

        public static IFeatureFlipper Flipper
        {
            get
            {
                return Features.flipperInstance ?? (Features.flipperInstance = Features.flipper.Value);
            }
        }

        public static ICollection<IFeatureStateParser> FeatureStateParsers
        {
            get
            {
                return Features.featureStateParsers;
            }
        }

        public static ICollection<IFeatureProvider> Providers
        {
            get
            {
                return Features.providers;
            }
        }

        public static void SetFlipper(IFeatureFlipper flipper)
        {
            if (flipper == null)
            {
                throw new ArgumentNullException("flipper");
            }

            Features.flipperInstance = flipper;
        }

        private static IFeatureFlipper InitializeFlipper()
        {
            return new DefaultFeatureFlipper(Features.Providers);
        }
    }
}
