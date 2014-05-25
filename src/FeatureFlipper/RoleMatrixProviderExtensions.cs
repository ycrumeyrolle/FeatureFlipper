namespace FeatureFlipper
{
    using System;

    /// <summary>
    /// Provides extensions methods to the <see cref="IRoleMatrixProvider"/>.
    /// </summary>
    public static class RoleMatrixProviderExtensions
    {
        /// <summary>
        /// Gets the matrix of roles.
        /// </summary>
        /// <param name="roleMatrixProvider">The <see cref="IRoleMatrixProvider"/>.</param>
        /// <param name="feature">The name of the feature.</param>
        /// <returns>An array of <see cref="string"/>.</returns>
        public static string[] GetRoleMatrix(this IRoleMatrixProvider roleMatrixProvider, string feature)
        {
            if (roleMatrixProvider == null)
            {
                throw new ArgumentNullException("roleMatrixProvider");
            }
            
            return roleMatrixProvider.GetRoleMatrix(feature, null);
        }
    }
}
