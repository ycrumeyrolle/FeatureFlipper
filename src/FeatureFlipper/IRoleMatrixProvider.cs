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
        /// <param name="feature">The name of the feature.</param>
        /// <param name="version">Optionnal. The version of the feature.</param>
        /// <returns>An array of <see cref="string"/>.</returns>
        string[] GetRoleMatrix(string feature, string version);
    }
}
