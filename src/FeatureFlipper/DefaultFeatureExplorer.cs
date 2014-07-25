namespace FeatureFlipper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Default implementation of the <see cref="IFeatureExplorer"/> interface.
    /// </summary>
    public class DefaultFeatureExplorer : IFeatureExplorer
    {
        private readonly IFeatureMetadataStore store;
  
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFeatureExplorer"/> class.
        /// </summary>
        /// <param name="store">The <see cref="IFeatureMetadataStore"/>.</param>
        public DefaultFeatureExplorer(IFeatureMetadataStore store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            this.store = store;
        }
        
        /// <inheritsdoc />
        public IReadOnlyCollection<FeatureDescriptor> GetFeatures()
        {
            return new ReadOnlyCollection<FeatureDescriptor>(this.store.Value.Select(m => new FeatureDescriptor(m.Key, m.Value)).ToList());
        }
    }
}
