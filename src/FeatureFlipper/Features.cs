namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Provides access to features flipping.
    /// </summary>
    public static class Features
    {
        private static ICollection<IFeatureStateParser> featureStateParsers = new Collection<IFeatureStateParser> 
        { 
            new BooleanFeatureStateParser(),
            new DateFeatureStateParser(new SystemClock())
        };

        private static Lazy<ICollection<IFeatureProvider>> providers = new Lazy<ICollection<IFeatureProvider>>(InitializeProviders);

        private static ICollection<IFeatureProvider> providersInstance;

        private static IConfigurationReader configurationReader = new DefaultConfigurationReader();

        private static IFeatureFlipper flipperInstance;

        private static Lazy<IFeatureFlipper> flipper = new Lazy<IFeatureFlipper>(InitializeFlipper);

        /// <summary>
        /// Gets the current <see cref="IFeatureFlipper"/>.
        /// </summary>
        public static IFeatureFlipper Flipper
        {
            get
            {
                return Features.flipperInstance ?? (Features.flipperInstance = Features.flipper.Value);
            }
        }

        /// <summary>
        /// Gets the list of currents <see cref="IFeatureStateParser"/>.
        /// </summary>
        public static ICollection<IFeatureStateParser> FeatureStateParsers
        {
            get
            {
                return Features.featureStateParsers;
            }
        }

        /// <summary>
        /// Gets the list of currents <see cref="IFeatureProvider"/>.
        /// </summary>
        public static ICollection<IFeatureProvider> Providers
        {
            get
            {
                return Features.providersInstance ?? (Features.providersInstance = Features.providers.Value);
            }
        }

        /// <summary>
        /// Gets the current <see cref="IConfigurationReader"/>.
        /// </summary>
        public static IConfigurationReader ConfigurationReader
        {
            get
            {
                return Features.configurationReader;
            }
        }

        /// <summary>
        /// Sets the current <see cref="IFeatureFlipper"/>.
        /// </summary>
        /// <param name="flipper">The <see cref="IFeatureFlipper"/> to set.</param>
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

        private static ICollection<IFeatureProvider> InitializeProviders()
        {
            return new Collection<IFeatureProvider> 
            { 
                new ConfigurationFeatureProvider(Features.ConfigurationReader, Features.featureStateParsers),
                new RoleFeatureProvider(new DefaultRoleMatrixProvider(new DataAnnotationMetadataProvider(new FeatureTypeResolver(new DefaultAssembliesResolver()))), new DefaultPrincipalProvider())
            };
        }
    }
}
