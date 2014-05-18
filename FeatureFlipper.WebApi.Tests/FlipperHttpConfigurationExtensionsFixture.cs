namespace FeatureFlipper.WebApi.Tests
{
    using System;
    using System.Web.Http;
    using Xunit;

    public class FlipperHttpConfigurationExtensionsFixture
    {
        [Fact]
        public void EnableFeatureFlipping_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => FlipperHttpConfigurationExtensions.EnableFeatureFlipping(null));
        }

        [Fact]
        public void EnableFeatureFlipping()
        {
            // Arrange
            HttpConfiguration config = new HttpConfiguration();

            // Act 
            FlipperHttpConfigurationExtensions.EnableFeatureFlipping(config);

            // Assert
            var actionSelector = config.Services.GetActionSelector();
            Assert.NotNull(actionSelector);
            Assert.IsType<FeatureActionSelector>(actionSelector);
        }
    }
}
