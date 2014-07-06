namespace FeatureFlipper.Tests
{
    using System;
    using Moq;
    using Xunit;

    public class FeatureMetatadaFixture
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FeatureMetadata(null, null, this.GetType(), "*", null));
            Assert.Throws<ArgumentNullException>(() => new FeatureMetadata("X", null, null, "*", null));
        }

        [Theory]
        [InlineData("X", null, "X")]
        [InlineData("X", "V1", "X¤V1")]
        public void Ctor(string name, string version, string expectedKey)
        {
            // Arrange
            FeatureMetadata featureMetadata = new FeatureMetadata(name, version, this.GetType(), string.Empty, null);

            // Act & assert
            Assert.Equal(name, featureMetadata.Name);
            Assert.Equal(version, featureMetadata.Version);
            Assert.Equal(expectedKey, featureMetadata.Key);
        }

        [Theory]
        [InlineData("A, B, C", new[] { "A", "B", "C" })]
        [InlineData(",,,,A, B, C, , , ", new[] { "A", "B", "C" })]
        [InlineData("A", new[] { "A" })]
        [InlineData("", new string[0])]
        public void GetRoles(string roles, string[] expectedRoles)
        {
            // Arrange
            FeatureMetadata featureMetadata = new FeatureMetadata("X", null, this.GetType(), roles, null);

            // Act
            var result = featureMetadata.GetRoles();

            // Assert
            Assert.Equal(expectedRoles, result);
        }
    }
}
