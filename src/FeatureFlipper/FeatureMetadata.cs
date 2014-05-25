namespace FeatureFlipper
{
    using System;
    using System.Linq;

    /// <summary>
    /// Represents the metadata of a feature. 
    /// </summary>
    public sealed class FeatureMetadata
    {
        private static readonly char[] separator = { ',' };

        private readonly string[] roles;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureMetadata"/> class.
        /// </summary>
        /// <param name="name">The name of the feature.</param>
        /// <param name="version">The version of the feature. Can be null.</param>
        /// <param name="type">The type of the feature.</param>
        /// <param name="roles">The roles associated to the feature.</param>
        public FeatureMetadata(string name, string version, Type type, string roles)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (roles == null)
            {
                throw new ArgumentNullException("roles");
            }

            this.Name = name;
            this.FeatureType = type;
            this.roles = roles.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(r => r.Trim()).Where(r => r.Length != 0).ToArray();

            if (version != null)
            {
                this.Version = version;
                this.Key = name + "¤" + version;
            }
            else
            {
                this.Key = name;
            }
        }

        /// <summary>
        /// Gets the name of the feature.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of the feature.
        /// </summary>
        public Type FeatureType { get; private set; }

        /// <summary>
        /// Gets the version of the feature.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets the key of the feature. 
        /// </summary>
        public string Key {get; private set;}

        /// <summary>
        /// Gets the roles associated to the feature.
        /// </summary>
        /// <returns>An array of <see cref="string"/> reprenting the roles associated to the feature.</returns>
        public string[] GetRoles()
        {
            return this.roles;
        }
    }
}
