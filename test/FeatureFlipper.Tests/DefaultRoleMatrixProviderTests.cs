namespace FeatureFlipper.Tests
{
    using Xunit;

    public class DefaultRoleMatrixProviderTests
    {
        [Fact]
        public void GetRoleMatrix_GuardClause()
        {
            // Arrange
            DefaultRoleMatrixProvider roleMatrixProvider = new DefaultRoleMatrixProvider();

            // Act
            var result = roleMatrixProvider.GetRoleMatrix(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetRoleMatrix_UnknowFeature_ReturnsEmptyArray()
        {
            // Arrange
            DefaultRoleMatrixProvider roleMatrixProvider = new DefaultRoleMatrixProvider();
            FeatureMetadata metadata = new FeatureMetadata("X", null, this.GetType(), null, null);

            // Act
            var result = roleMatrixProvider.GetRoleMatrix(metadata);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("A, B, C", new[] { "A", "B", "C" })]
        [InlineData("A, B, C, , , ", new[] { "A", "B", "C" })]
        [InlineData("A", new[] { "A" })]
        [InlineData("", new string[0])]
        public void GetRoleMatrix_KnowFeature_ReturnsRoles(string roles, string[] expectedRoles)
        {
            // Arrange
            DefaultRoleMatrixProvider roleMatrixProvider = new DefaultRoleMatrixProvider();
            FeatureMetadata metadata = new FeatureMetadata("X", null, this.GetType(), roles, null);

            // Act
            var result = roleMatrixProvider.GetRoleMatrix(metadata);

            // Assert
            Assert.Equal(expectedRoles, result);
        }
    }
}
