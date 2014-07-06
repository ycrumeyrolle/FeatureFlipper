namespace FeatureFlipper.CycleDetection
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents a collection of <see cref="Vertex"/>.
    /// </summary>
    public class VertexCollection : Collection<Vertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexCollection"/> class.
        /// </summary>
        public VertexCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexCollection"/> class.
        /// </summary>
        /// <param name="list">The list of vertices.</param>
        public VertexCollection(IList<Vertex> list)
            : base(list)
        {
        }
    }
}
