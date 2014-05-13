namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a resolver of types.
    /// </summary>
    public interface ITypeResolver
    {
        /// <summary>
        /// Returns a list of types available for the application.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection{Type}"/>.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "The method performs a time-consuming operation.")]
        ICollection<Type> GetTypes();
    }
}
