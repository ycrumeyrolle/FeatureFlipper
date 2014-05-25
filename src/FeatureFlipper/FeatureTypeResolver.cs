namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represents a resolver of feature types.
    /// </summary>
    public sealed class FeatureTypeResolver : ITypeResolver
    {
        private readonly IAssembliesResolver assembliesResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureTypeResolver"/> class.
        /// </summary>
        /// <param name="assembliesResolver">The <see cref="IAssembliesResolver"/> that provides assemblies to discover.</param>
        public FeatureTypeResolver(IAssembliesResolver assembliesResolver)
        {
            if (assembliesResolver == null)
            {
                throw new ArgumentNullException("assembliesResolver");
            }

            this.assembliesResolver = assembliesResolver;
        }

        /// <summary>
        /// Returns a list of features available for the application.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection{Type}"/> of features.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "We deliberately ignore all exceptions when building the cache.")]
        public ICollection<Type> GetTypes()
        {
            List<Type> result = new List<Type>();

            // Go through all assemblies referenced by the application and search for types matching a predicate
            Assembly[] assemblies = this.assembliesResolver.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type[] exportedTypes;
                if (assembly == null || assembly.IsDynamic)
                {
                    // can't call GetExportedTypes on a dynamic assembly
                    continue;
                }

                try
                {
                    exportedTypes = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    exportedTypes = ex.Types;
                }
                catch
                {
                    // We deliberately ignore all exceptions when building the cache.
                    continue;
                }

                if (exportedTypes != null)
                {
                    result.AddRange(exportedTypes.Where(type => type != null && type.GetCustomAttributes(typeof(FeatureAttribute), true).Length > 0));
                }
            }

            return result;
        }
    }
}
