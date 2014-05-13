namespace FeatureFlipper
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Provides an implementation of <see cref="IAssembliesResolver"/>.
    /// </summary>
    public class DefaultAssembliesResolver : IAssembliesResolver
    {
        /// <summary>
        /// Returns a list of assemblies available for the application.
        /// </summary>
        /// <returns>An array of <see cref="Assembly"/>.</returns>
        public Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}
