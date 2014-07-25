namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Represents a description of feature.
    /// </summary>
    public class FeatureDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureDescriptor"/> class.
        /// </summary>
        /// <param name="name">The name of the feature.</param>
        /// <param name="features">The features of <see cref="FeatureMetadata"/>.</param>
        public FeatureDescriptor(string name, IDictionary<string, FeatureMetadata> features)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (features == null)
            {
                throw new ArgumentNullException("features");
            }

            this.Name = name;
            this.Versions = CreateVersions(features);
        }

        /// <summary>
        /// Gets the feature name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the feature versions.
        /// </summary>
        public IReadOnlyCollection<VersionDescriptor> Versions { get; private set; }

        private static ReadOnlyCollection<VersionDescriptor> CreateVersions(IDictionary<string, FeatureMetadata> dictionary)
        {
            return new ReadOnlyCollection<VersionDescriptor>(dictionary.Select(v => new VersionDescriptor(v.Key, v.Value.FeatureType, v.Value.GetDependsOn(), v.Value.GetRoles())).ToList());
        }
    }
}
