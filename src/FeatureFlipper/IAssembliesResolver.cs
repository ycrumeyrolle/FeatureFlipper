namespace FeatureFlipper
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Provides an abstraction for managing the assemblies of an application. 
    /// </summary>
    public interface IAssembliesResolver
    {
        /// <summary>
        /// Returns a list of assemblies available for the processor. 
        /// </summary>
        /// <returns>A <see cref="ICollection{T}"/> of assemblies.</returns>
        Assembly[] GetAssemblies();
    }
}
