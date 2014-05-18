namespace FeatureFlipper
{
    using System;

    /// <summary>
    /// Allows to give a name to a feature.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class FeatureAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureAttribute"/> class.
        /// </summary>
        /// <param name="name">The name to give to the feature.</param>
        public FeatureAttribute(string name)
        {
            this.Name = name;
        }
        
        /// <summary>
        /// Gets the name of the feature.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the roles required for the feature.
        /// </summary>
        public string Roles { get; set; }
    }
}
