namespace FeatureFlipper.CycleDetection
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Tarjan algorithm to detect cycles in a graph. 
    /// http://en.wikipedia.org/wiki/Tarjan%27s_strongly_connected_components_algorithm
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Tarjan", Justification = "This is the name of the algorithm.")]
    public class Tarjan
    {
        private Collection<VertexCollection> stronglyConnectedComponents;
        private Stack<Vertex> stack;
        private int index;

        /// <summary>
        /// Calculates the sets of strongly connected vertices.
        /// </summary>
        /// <param name="graph">Graph to detect cycles within.</param>
        /// <returns>Set of strongly connected components (sets of vertices)</returns>
        public Collection<VertexCollection> DetectCycle(IEnumerable<Vertex> graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }

            this.stronglyConnectedComponents = new Collection<VertexCollection>();
            this.index = 0;
            this.stack = new Stack<Vertex>();
            foreach (var vertex in graph)
            {
                if (vertex.Index < 0)
                {
                    this.StrongConnect(vertex);
                }
            }

            return this.stronglyConnectedComponents;
        }

        private void StrongConnect(Vertex vertex)
        {
            vertex.Index = this.index;
            vertex.LowLink = this.index;
            this.index++;
            this.stack.Push(vertex);

            foreach (Vertex current in vertex.Dependencies)
            {
                if (current.Index < 0)
                {
                    this.StrongConnect(current);
                    vertex.LowLink = Math.Min(vertex.LowLink, current.LowLink);
                }
                else if (this.stack.Contains(current))
                {
                    vertex.LowLink = Math.Min(vertex.LowLink, current.Index);
                }
            }

            if (vertex.LowLink == vertex.Index)
            {
                var vertices = new VertexCollection();
                Vertex current;
                do
                {
                    current = this.stack.Pop();
                    vertices.Add(current);
                }
                while (current != vertex);

                this.stronglyConnectedComponents.Add(vertices);
            }
        }
    }
}
