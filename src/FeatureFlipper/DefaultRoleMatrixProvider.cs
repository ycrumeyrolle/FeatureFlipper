namespace FeatureFlipper
{
    using System;

    /// <summary>
    /// Implementation of <see cref="IRoleMatrixProvider"/> that gets roles in data annotations.
    /// </summary>
    public sealed class DefaultRoleMatrixProvider : IRoleMatrixProvider
    {
        private readonly IMetadataProvider metataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRoleMatrixProvider"/> class.
        /// </summary>
        /// <param name="metadataProvider">The <see cref="IMetadataProvider"/>.</param>
        public DefaultRoleMatrixProvider(IMetadataProvider metadataProvider)
        {
            if (metadataProvider == null)
            {
                throw new ArgumentNullException("metadataProvider");
            }

            this.metataProvider = metadataProvider;
        }

        /// <inheritsdoc/>
        public string[] GetRoleMatrix(string feature)
        {
            if (feature == null)
            {
                throw new ArgumentNullException("feature");
            }

            FeatureMetadata featureMetatada = this.metataProvider.GetMetadata(feature);

            if (featureMetatada != null)
            {
                return featureMetatada.GetRoles();
            }

            return null;
        }
    }
}
