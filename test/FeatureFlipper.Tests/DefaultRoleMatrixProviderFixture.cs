namespace FeatureFlipper.Tests
{
    using System;
    using Moq;
    using Xunit;

    public class DefaultRoleMatrixProviderFixture
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DefaultRoleMatrixProvider(null));
        }

        [Fact]
        public void GetRoleMatrixCtor_GuardClause()
        {
            // Arrange
            var metatadataProvider = new Mock<IMetadataProvider>(MockBehavior.Strict);
            DefaultRoleMatrixProvider roleMatrixProvider = new DefaultRoleMatrixProvider(metatadataProvider.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => roleMatrixProvider.GetRoleMatrix(null));
        }

        [Fact]
        public void GetRoleMatrix_UnknowFeature_ReturnsNull()
        {
            // Arrange
            var metatadataProvider = new Mock<IMetadataProvider>(MockBehavior.Strict);
            metatadataProvider
               .Setup(p => p.GetMetadata(It.IsAny<string>()))
               .Returns((FeatureMetadata)null);
            DefaultRoleMatrixProvider roleMatrixProvider = new DefaultRoleMatrixProvider(metatadataProvider.Object);

            // Act
            var result = roleMatrixProvider.GetRoleMatrix("X");

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData("A, B, C", new[] { "A", "B", "C" })]
        [InlineData("A, B, C, , , ", new[] { "A", "B", "C" })]
        [InlineData("A", new[] { "A" })]
        [InlineData("", new string[0])]
        public void GetRoleMatrix_KnowFeature_ReturnsRoles(string roles, string[] expectedRoles)
        {
            // Arrange
            var metatadataProvider = new Mock<IMetadataProvider>(MockBehavior.Strict);
            metatadataProvider
                .Setup(p => p.GetMetadata(It.IsAny<string>()))
                .Returns(new FeatureMetadata("X", this.GetType(), roles));
            DefaultRoleMatrixProvider roleMatrixProvider = new DefaultRoleMatrixProvider(metatadataProvider.Object);

            // Act
            var result = roleMatrixProvider.GetRoleMatrix("X");

            // Assert
            Assert.Equal(expectedRoles, result);
        }
    }
}
