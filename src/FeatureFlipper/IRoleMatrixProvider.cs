namespace FeatureFlipper
{
    /// <summary>
    /// Provides methods to retrieve user roles.
    /// </summary>
    public interface IRoleMatrixProvider
    {
        /// <summary>
        /// Gets the matrix of roles.
        /// </summary>
        /// <param name="metadata">The metadata of the feature.</param>
        /// <returns>An array of <see cref="string"/>.</returns>
        string[] GetRoleMatrix(FeatureMetadata metadata);
    }
}
