namespace FeatureFlipper.Tests
{
    using System;
    using Xunit;

    public class FeatureNameProviderTests
    {
        [Theory]
        [InlineData(typeof(FeatureWithoutAttribute), "FeatureFlipper.Tests.FeatureNameProviderTests+FeatureWithoutAttribute")]
        [InlineData(typeof(FeatureWithAttribute), "feature1")]
        public void GetFeatureName(Type featureType, string expectedName)
        {
            // Arrange
            var nameProvider = new FeatureNameProvider();

            // Act
            var name = nameProvider.GetFeatureName(featureType);

            // Assert
            Assert.Equal(expectedName, name);
        }

        [Fact]
        public void GetFeatureName_GuardClause()
        {
            // Arrange
            var nameProvider = new FeatureNameProvider();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => nameProvider.GetFeatureName(null));
        }

        private class FeatureWithoutAttribute
        {
        }

        [Feature("feature1")]
        private class FeatureWithAttribute
        {
        }
    }
}
