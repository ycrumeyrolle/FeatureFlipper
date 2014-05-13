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
            Assert.Throws<ArgumentNullException>(() => new FeatureMetadata(null, this.GetType(), "*"));
            Assert.Throws<ArgumentNullException>(() => new FeatureMetadata("X", null, "*"));
            Assert.Throws<ArgumentNullException>(() => new FeatureMetadata("X", this.GetType(), null));
        }

        [Theory]
        [InlineData("A, B, C", new[] { "A", "B", "C" })]
        [InlineData(",,,,A, B, C, , , ", new[] { "A", "B", "C" })]
        [InlineData("A", new[] { "A" })]
        [InlineData("", new string[0])]
        public void Ctor(string roles, string[] expectedRoles)
        {
            // Arrange
            FeatureMetadata featureMetadata = new FeatureMetadata("X", this.GetType(), roles);

            // Act
            var result = featureMetadata.GetRoles();

            // Assert
            Assert.Equal(expectedRoles, result);
        }
    }
}
