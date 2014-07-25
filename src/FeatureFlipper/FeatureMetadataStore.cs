namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using FeatureFlipper.CycleDetection;
    using FeatureFlipper.Properties;

    /// <summary>
    /// Provides a metadata storage.
    /// </summary>
    public class FeatureMetadataStore : IFeatureMetadataStore
    {
        private readonly ITypeResolver typeResolver;

        private readonly ICycleDetector cycleDetector;

        private readonly Lazy<IDictionary<string, Dictionary<string, FeatureMetadata>>> cache;
      
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureMetadataStore"/> class.
        /// </summary>
        /// <param name="typeResolver">The <see cref="ITypeResolver"/>.</param>
        /// <param name="cycleDetector">The <see cref="ICycleDetector"/>.</param>
        public FeatureMetadataStore(ITypeResolver typeResolver, ICycleDetector cycleDetector)
        {
            if (typeResolver == null)
            {
                throw new ArgumentNullException("typeResolver");
            }

            if (cycleDetector == null)
            {
                throw new ArgumentNullException("cycleDetector");
            }

            this.typeResolver = typeResolver;
            this.cycleDetector = cycleDetector;
            this.cache = new Lazy<IDictionary<string, Dictionary<string, FeatureMetadata>>>(InitializeMetatada);
        }

        /// <inheritsdoc />
        public IDictionary<string, Dictionary<string, FeatureMetadata>> Value
        {
            get
            {
                return this.cache.Value;
            }
        }

        private IDictionary<string, Dictionary<string, FeatureMetadata>> InitializeMetatada()
        {
            var featuresType = this.typeResolver.GetTypes();
            if (featuresType == null)
            {
                return new Dictionary<string, Dictionary<string, FeatureMetadata>>();
            }

            var features = new List<FeatureMetadata>();
            foreach (Type type in featuresType)
            {
                FeatureAttribute attribute = type.GetCustomAttribute<FeatureAttribute>(true);
                features.Add(new FeatureMetadata(attribute.Name, attribute.Version, type, attribute.Roles, attribute.DependsOn));
            }

            var groups = features.GroupBy(f => f.Key);
            if (groups.Any(f => f.Count() > 1))
            {
                throw CreateException(groups);
            }

            var cycles = this.DetectCycles(features);
            if (cycles.Length > 0)
            {
                throw CreateDependencyException(cycles);
            }

            return features.GroupBy(f => f.Name).ToDictionary(g => g.Key, g => g.ToDictionary(f => f.Version ?? string.Empty));
        }

        private string[] DetectCycles(IEnumerable<FeatureMetadata> features)
        {
            var hasCycles = this.cycleDetector.DetectCycles(features);

            return hasCycles.ToArray();
        }

        private static Exception CreateException(IEnumerable<IGrouping<string, FeatureMetadata>> groups)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Resources.Feature_Duplicate);
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

        private static Exception CreateDependencyException(IEnumerable<string> dependencies)
        {
            string message = dependencies.Aggregate(new StringBuilder(Resources.Feature_CyclicDependencies).AppendLine(), (sb, d) => sb.Append(" - ").AppendLine(d), sb => sb.ToString());
            return new InvalidOperationException(message);
        }
    }
}
