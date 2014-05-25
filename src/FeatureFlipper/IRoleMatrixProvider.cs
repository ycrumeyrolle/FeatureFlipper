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
        /// <param name="feature">The feature.</param>
        /// <param name="version">THe version of the feature. Can be <c>null</c>.</param>
        /// <returns>An array of <see cref="string"/>.</returns>
        string[] GetRoleMatrix(string feature, string version);
    }
}
