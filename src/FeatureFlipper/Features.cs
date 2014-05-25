namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;

    /// <summary>
    /// Provides access to features flipping.
    /// </summary>
    public static class Features
    {
        private static readonly ICollection<IFeatureStateParser> FeatureStateParserCollection = new Collection<IFeatureStateParser> 
        { 
            new BooleanFeatureStateParser(),
            new DateFeatureStateParser(new SystemClock()),
            new VersionStateParser()
        };

        private static readonly Lazy<ICollection<IFeatureProvider>> ProvidersInstance = new Lazy<ICollection<IFeatureProvider>>(InitializeProviders);

        private static IConfigurationReader configurationReader = new DefaultConfigurationReader();

        private static IFeatureFlipper flipperInstance;

        private static readonly Lazy<IFeatureFlipper> FlipperInner = new Lazy<IFeatureFlipper>(InitializeFlipper);

        /// <summary>
        /// Gets or sets the current <see cref="IFeatureFlipper"/>.
        /// </summary>
        public static IFeatureFlipper Flipper
        {
            get
            {
                return Features.flipperInstance ?? Features.FlipperInner.Value;
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
                return Features.FeatureStateParserCollection;
            }
        }

        /// <summary>
        /// Gets the list of currents <see cref="IFeatureProvider"/>.
        /// </summary>
        public static ICollection<IFeatureProvider> Providers
        {
            get
            {
                return Features.ProvidersInstance.Value;
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
                new ConfigurationFeatureProvider(Features.ConfigurationReader, Features.FeatureStateParserCollection),
                new RoleFeatureProvider(new DefaultRoleMatrixProvider(new DataAnnotationMetadataProvider(new FeatureTypeResolver(new DefaultAssembliesResolver()))), new DefaultPrincipalProvider())
            };
        }
    }
}
