namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Default implementation of <see cref="IMetadataProvider"/>.
    /// Provides metadata from data annotations.
    /// See <see cref="FeatureAttribute"/>.
    /// </summary>
    public sealed class DataAnnotationMetadataProvider : IMetadataProvider
    {
        private readonly IFeatureMetadataStore store;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAnnotationMetadataProvider"/> class.
        /// </summary>
        /// <param name="store">The <see cref="IFeatureMetadataStore"/>.</param>
        public DataAnnotationMetadataProvider(IFeatureMetadataStore store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            this.store = store;
        }

        /// <inheritsdoc />
        public FeatureMetadata GetMetadata(string feature, string version)
        {
            Dictionary<string, FeatureMetadata> lookup;
            if (this.store.Value.TryGetValue(feature, out lookup))
            {
                FeatureMetadata featureMetatada;
                if (lookup.TryGetValue(version ?? string.Empty, out featureMetatada))
                {
                    return featureMetatada;
                }
            }

            return null;
        }
    }
}
