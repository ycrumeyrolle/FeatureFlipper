namespace FeatureFlipper.Tests.CycleDetection
{
    using System;
    using System.Linq;
    using FeatureFlipper.CycleDetection;
    using Xunit;

    public class TarjanTests
    {
        [Fact]
        public void GuardClause()
        {
            // Arrange
            var detector = new Tarjan();

            // Act 
            var exception = Record.Exception(() => detector.DetectCycle(null));            

            // Assert
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void EmptyGraph()
        {
            // Arrange
            var graph = new VertexCollection();
            var detector = new Tarjan();

            // Act
            var cycles = detector.DetectCycle(graph);

            // Assert
            Assert.Equal(0, cycles.Count);
        }

        // A
        [Fact]
        public void Single()
        {
            // Arrange
            var graph = new VertexCollection();
            graph.Add(new Vertex("A"));
            var detector = new Tarjan();

            // Act
            var components = detector.DetectCycle(graph);

            // Assert
            Assert.Equal(1, components.Count);
            Assert.Equal(1, components.First().Count);
        }
        
        // A→B
        [Fact]
        public void LinearWithTwoElements()
        {
            // Arrange
            var graph = new VertexCollection();
            var vA = new Vertex("A");
            var vB = new Vertex("B");
            vA.Dependencies.Add(vB);
            graph.Add(vA);
            graph.Add(vB);
            var detector = new Tarjan(); 

            // Act
            var components = detector.DetectCycle(graph);

            // Assert
            Assert.Equal(2, components.Count);
            Assert.True(components.All(c => c.Count == 1));
        }

        // A→B→C
        [Fact]
        public void LinearWithThreeElements()
        {
            // Arrange
            var graph = new VertexCollection();
            var vA = new Vertex("A");
            var vB = new Vertex("B");
            var vC = new Vertex("C");
            vA.Dependencies.Add(vB);
            vB.Dependencies.Add(vC);
            graph.Add(vA);
            graph.Add(vB);
            graph.Add(vC);
            var detector = new Tarjan();

            // Act
            var components = detector.DetectCycle(graph);

            // Assert
            Assert.Equal(3, components.Count);
            Assert.True(components.All(c => c.Count == 1));
        }

        // A↔B
        [Fact]
        public void CycleWithTwoElements()
        {
            // Arrange
            var graph = new VertexCollection();
            var vA = new Vertex("A");
            var vB = new Vertex("B");
            vA.Dependencies.Add(vB);
            vB.Dependencies.Add(vA);
            graph.Add(vA);
            graph.Add(vB);
            var detector = new Tarjan();

            // Act
            var components = detector.DetectCycle(graph);

            // Assert
            Assert.Equal(1, components.Count);
            Assert.True(components.All(c => c.Count == 2));
        }

        // A→B
        // ↑ ↓
        // └─C
        [Fact]
        public void CycleWithThreeElements()
        {
            // Arrange
            var graph = new VertexCollection();
            var vA = new Vertex("A");
            var vB = new Vertex("B");
            var vC = new Vertex("C");
            vA.Dependencies.Add(vB);
            vB.Dependencies.Add(vC);
            vC.Dependencies.Add(vA);
            graph.Add(vA);
            graph.Add(vB);
            graph.Add(vC);
            var detector = new Tarjan();

            // Act
            var components = detector.DetectCycle(graph);

            // Assert
            Assert.Equal(1, components.Count);
            Assert.Equal(3, components.First().Count);
        }

        // A→B   D→E
        // ↑ ↓   ↑ ↓
        // └─C   └─F
        [Fact]
        public void TwoIsolatedWithThreeElementsEach()
        {
            // Arrange
            var graph = new VertexCollection();
            var vA = new Vertex("A");
            var vB = new Vertex("B");
            var vC = new Vertex("C");
            vA.Dependencies.Add(vB);
            vB.Dependencies.Add(vC);
            vC.Dependencies.Add(vA);
            graph.Add(vA);
            graph.Add(vB);
            graph.Add(vC);

            var vD = new Vertex("D");
            var vE = new Vertex("E");
            var vF = new Vertex("F");
            vD.Dependencies.Add(vE);
            vE.Dependencies.Add(vF);
            vF.Dependencies.Add(vD);
            graph.Add(vD);
            graph.Add(vE);
            graph.Add(vF);
            
            var detector = new Tarjan();

            // Act
            var components = detector.DetectCycle(graph);

            // Assert
            Assert.Equal(2, components.Count);
            Assert.True(components.All(c => c.Count == 3));
        }
        
        // A→B
        // ↑ ↓
        // └─C-→D
        [Fact]
        public void CycleWithThreeElementsWithStub()
        {
            // Arrange
            var graph = new VertexCollection();
            var vA = new Vertex("A");
            var vB = new Vertex("B");
            var vC = new Vertex("C");
            var vD = new Vertex("D");
            vA.Dependencies.Add(vB);
            vB.Dependencies.Add(vC);
            vC.Dependencies.Add(vA);
            vC.Dependencies.Add(vD);
            graph.Add(vA);
            graph.Add(vB);
            graph.Add(vC);
            graph.Add(vD);
            var detector = new Tarjan();

            // Act
            var components = detector.DetectCycle(graph);

            // Assert
            Assert.Equal(2, components.Count);
            Assert.Equal(1, components.Count(c => c.Count == 3));
            Assert.Equal(1, components.Count(c => c.Count == 1));
            Assert.True(components.Single(c => c.Count == 1).Single() == vD);
        }
    }
}