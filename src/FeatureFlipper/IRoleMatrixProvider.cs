namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Provides methods to retrieve user roles.
    /// </summary>
    public interface IRoleMatrixProvider
    {
        /// <summary>
        /// Gets the matrix of roles.
        /// </summary>
        /// <param name="feature">The feature.</param>
        /// <returns>An array of <see cref="string"/>.</returns>
        string[] GetRoleMatrix(string feature);
    }
}
