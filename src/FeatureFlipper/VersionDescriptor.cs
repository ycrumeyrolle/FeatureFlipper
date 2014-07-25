namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the description of feature version.
    /// </summary>
    public class VersionDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionDescriptor"/> class.
        /// </summary>
        /// <param name="version">The version of the feature.</param>
        /// <param name="type">The type of the feature.</param>
        /// <param name="dependencies">The feature dependencies.</param>
        /// <param name="roles">The feature roles.</param>
        internal VersionDescriptor(string version, Type type, string[] dependencies, string[] roles)
        {
            this.Name = version;
            this.FeatureType = type;
            this.Dependencies = new ReadOnlyCollection<string>(dependencies);
            this.Roles = new ReadOnlyCollection<string>(roles);
        }

        /// <summary>
        /// Gets the feature version.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the feature type.
        /// </summary>
        public Type FeatureType { get; private set; }

        /// <summary>
        /// Gets the feature dependencies.
        /// </summary>
        public IReadOnlyCollection<string> Dependencies { get; private set; }

        /// <summary>
        /// Gets the feature roles.
        /// </summary>
        public IReadOnlyCollection<string> Roles { get; private set; }
    }
}