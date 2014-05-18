namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Default implementation of <see cref="IMetadataProvider"/>.
    /// Provides metadata from data annotations.
    /// See <see cref="FeatureAttribute"/>.
    /// </summary>
    public sealed class DataAnnotationMetadataProvider : IMetadataProvider
    {
        private readonly ITypeResolver typeResolver;

        private readonly Lazy<IDictionary<string, FeatureMetadata>> cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAnnotationMetadataProvider"/> class.
        /// </summary>
        /// <param name="typeResolver">The <see cref="ITypeResolver"/>.</param>
        public DataAnnotationMetadataProvider(ITypeResolver typeResolver)
        {
            if (typeResolver == null)
            {
                throw new ArgumentNullException("typeResolver");
            }

            this.typeResolver = typeResolver;
            this.cache = new Lazy<IDictionary<string, FeatureMetadata>>(this.InitializeMetatada);
        }

        /// <inheritsdoc />
        public FeatureMetadata GetMetadata(string feature)
        {
            var metadataCache = this.cache.Value;
            FeatureMetadata featureMetatada;
            if (metadataCache.TryGetValue(feature, out featureMetatada))
            {
                return featureMetatada;
            }

            return null;
        }

        private IDictionary<string, FeatureMetadata> InitializeMetatada()
        {
            var featuresType = this.typeResolver.GetTypes();
            if (featuresType == null)
            {
                return new Dictionary<string, FeatureMetadata>();
            }

            var features = new List<FeatureMetadata>();
            foreach (Type type in featuresType)
            {
                FeatureAttribute attribute = type.GetCustomAttribute<FeatureAttribute>(true);
                features.Add(new FeatureMetadata(attribute.Name, type, attribute.Roles ?? "*"));
            }

            var groups = features.GroupBy(f => f.Name);
            if (groups.Any(f => f.Count() > 1))
            {
                throw CreateException(groups);
            }

            return features.ToDictionary(f => f.Name);
        }

        private static Exception CreateException(IEnumerable<IGrouping<string, FeatureMetadata>> groups)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("More than one feature have the same feature name : ");
            foreach (var group in groups.Where(f => f.Count() > 1))
            {
                sb.Append(" - ").AppendLine(group.Key);
                foreach (var item in group)
                {
                    sb.Append("   + ").AppendLine(item.FeatureType.FullName);
                }
            }

            return new InvalidOperationException(sb.ToString());
        }
    }
}
