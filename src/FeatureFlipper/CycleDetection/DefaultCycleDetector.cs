namespace FeatureFlipper.CycleDetection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Default implementation of <see cref="ICycleDetector"/>. 
    /// Use Trajan algorithm.
    /// </summary>
    public class DefaultCycleDetector : ICycleDetector
    {
        /// <summary>
        /// Detects whether a feature has dependencies cycles.
        /// </summary>
        /// <returns>A list of cycles, represented as a string.</returns>
        public IEnumerable<string> DetectCycles(IEnumerable<FeatureMetadata> features)
        {
            if (features == null)
            {
                throw new ArgumentNullException("features");
            }

            var graph = PrepareGraph(features);
            var tarjan = new Tarjan();
            var components = tarjan.DetectCycle(graph);

            return components.Where(c => c.Count > 1).Select(FormatCycle);
        }

        private static string FormatCycle(VertexCollection vertices)
        {
            return vertices.Aggregate(new StringBuilder(), (sb, c) => sb.Append(c.Value).Append(" --> "), sb => sb.Append(vertices[0].Value).ToString());
        }

        private static VertexCollection PrepareGraph(IEnumerable<FeatureMetadata> features)
        {
            var vertices = features.Select(f => new Vertex(f.Name, f.GetDependsOn().Select(f2 => new Vertex(f2)))).ToArray();

            foreach (var vertexA in vertices)
            {
                foreach (var vertexB in vertices)
                {
                    for (int i = 0; i < vertexB.Dependencies.Count; i++)
                    {
                        if (vertexA.Value == vertexB.Dependencies[i].Value)
                        {
                            vertexB.Dependencies[i] = vertexA;
                        }
                    }
                }
            }

            return new VertexCollection(vertices);
        }
    }
}
