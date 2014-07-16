namespace FeatureFlipper.Tests.CycleDetection
{
    using System;
    using System.Linq;
    using FeatureFlipper.CycleDetection;
    using Xunit;

    public class DefaultCycleDetectorTests
    {
        [Fact]
        public void DetectCycles_GuardClause()
        {
            // Arrange
            DefaultCycleDetector detector = new DefaultCycleDetector();

            // Act & assert       
            Assert.Throws<ArgumentNullException>(() => detector.DetectCycles(null));
        }

        [Fact]
        public void DetectCycles_WithCycle_ReturnsCycleDescription()
        {
            // Arrange
            DefaultCycleDetector detector = new DefaultCycleDetector();
            var vertexX = new FeatureMetadata("X", null, this.GetType(), null, "Y, Z");
            var vertexY = new FeatureMetadata("Y", null, this.GetType(), null, "Z");
            var vertexZ = new FeatureMetadata("Z", null, this.GetType(), null, "X");
            var vertices = new[] { vertexX, vertexY, vertexZ };

            // Act            
            var result = detector.DetectCycles(vertices);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count());
            Assert.Equal("Z --> Y --> X --> Z", result.ElementAt(0));
        }

        [Fact]
        public void DetectCycles_WithCycles_ReturnsCyclesDescription()
        {
            // Arrange
            DefaultCycleDetector detector = new DefaultCycleDetector();
            var vertexX = new FeatureMetadata("X", null, this.GetType(), null, "Y");
            var vertexY = new FeatureMetadata("Y", null, this.GetType(), null, "Z");
            var vertexZ = new FeatureMetadata("Z", null, this.GetType(), null, "1");
            var vertex1 = new FeatureMetadata("1", null, this.GetType(), null, "4");
            var vertex2 = new FeatureMetadata("2", null, this.GetType(), null, "3");
            var vertex3 = new FeatureMetadata("3", null, this.GetType(), null, "2");
            var vertex4 = new FeatureMetadata("4", null, this.GetType(), null, "X");
            var vertices = new[] { vertexX, vertexY, vertexZ, vertex1, vertex2, vertex3, vertex4 };

            // Act            
            var result = detector.DetectCycles(vertices);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("4 --> 1 --> Z --> Y --> X --> 4", result.ElementAt(0));
            Assert.Equal("3 --> 2 --> 3", result.ElementAt(1));
        }

        [Fact]
        public void DetectCycles_WithoutCycle_ReturnsFalse()
        {
            // Arrange
            DefaultCycleDetector detector = new DefaultCycleDetector();
            var vertexX = new FeatureMetadata("X", null, this.GetType(), null, "Y, Z");
            var vertexY = new FeatureMetadata("Y", null, this.GetType(), null, "Z");
            var vertexZ = new FeatureMetadata("Z", null, this.GetType(), null, null);
            var vertices = new[] { vertexX, vertexY, vertexZ };

            // Act
            var result = detector.DetectCycles(vertices);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }
    }
}
