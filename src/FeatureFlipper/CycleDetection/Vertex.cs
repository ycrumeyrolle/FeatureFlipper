namespace FeatureFlipper.CycleDetection
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a vertex in a graph.
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex"/> class.
        /// </summary>
        /// <param name="value">The value of the vertex.</param>
        public Vertex(string value) 
        {
            this.Value = value;
            this.Index = -1;
            this.Dependencies = new List<Vertex>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex"/> class.
        /// </summary>
        /// <param name="value">The value of the vertex.</param>
        /// <param name="dependencies">The dependencies of the vertex.</param>
        public Vertex(string value, IEnumerable<Vertex> dependencies)
        {
            this.Value = value;
            this.Index = -1;
            this.Dependencies = dependencies.ToList();
        }

        internal int Index { get; set; }

        internal int LowLink { get; set; }

        /// <summary>
        /// Gets the value of the vertex.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Gets the dependencies of the vertex.
        /// </summary>
        public IList<Vertex> Dependencies { get; private set; }     
    }
}
