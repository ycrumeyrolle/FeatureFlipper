namespace FeatureFlipper.Tests.CycleDetection
{
    using System;
    using FeatureFlipper.CycleDetection;
    using Xunit;

    public class VertexTests
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new Vertex("test", null));
            Assert.DoesNotThrow(() => new Vertex(null));
            Assert.DoesNotThrow(() => new Vertex(null, new Vertex[0]));
        }

        [Fact]
        public void Ctor1()
        {
            // Act
            Vertex vertex = new Vertex("test");

            // Assert
            Assert.Equal("test", vertex.Value);
            Assert.Equal(0, vertex.Dependencies.Count);
        }

        [Fact]
        public void Ctor2()
        {
            // Act
            Vertex vertex = new Vertex("test", new[] { new Vertex("1"), new Vertex("2") });

            // Assert
            Assert.Equal("test", vertex.Value);
            Assert.Equal(2, vertex.Dependencies.Count);
        }
    }
}
