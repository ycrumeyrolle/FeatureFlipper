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
        private static readonly ICollection<IFeatureStateParser> featureStateParsers = new Collection<IFeatureStateParser> 
        { 
            new BooleanFeatureStateParser(),
            new DateFeatureStateParser(new SystemClock()),
            new VersionStateParser()
        };

        private static readonly Lazy<ICollection<IFeatureProvider>> providers = new Lazy<ICollection<IFeatureProvider>>(InitializeProviders);

        private static IConfigurationReader configurationReader = new DefaultConfigurationReader();

        private static IFeatureFlipper flipperInstance;

        private static readonly Lazy<IFeatureFlipper> flipper = new Lazy<IFeatureFlipper>(InitializeFlipper);

        /// <summary>
        /// Gets or sets the current <see cref="IFeatureFlipper"/>.
        /// </summary>
        public static IFeatureFlipper Flipper
        {
            get
            {
                return Features.flipperInstance ?? Features.flipper.Value;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                Features.flipperInstance = value;
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
                return Features.providers.Value;
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

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                Features.configurationReader = value;
            }
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
