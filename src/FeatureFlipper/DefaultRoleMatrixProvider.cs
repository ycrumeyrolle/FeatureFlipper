namespace FeatureFlipper
{
    /// <summary>
    /// Implementation of <see cref="IRoleMatrixProvider"/> that gets roles in data annotations.
    /// </summary>
    public sealed class DefaultRoleMatrixProvider : IRoleMatrixProvider
    {
        /// <inheritsdoc/>
        public string[] GetRoleMatrix(FeatureMetadata metadata)
        {
            if (metadata != null)
            {
                return metadata.GetRoles();
            }

            return null;
        }
    }
}
