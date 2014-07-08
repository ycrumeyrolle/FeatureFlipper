namespace FeatureFlipper.Tests
{
    using System;
    using System.Linq;
    using Moq;
    using Xunit;

    public class FeaturesFixture
    {
        [Fact]
        public void PropertiesCheck()
        {
            // Act
            var flipper = Features.Flipper;

            // Assert
            Assert.NotNull(flipper);
            Assert.NotNull(flipper.Providers);
            Assert.Equal(2, flipper.Providers.Count);
            Assert.Equal(1, flipper.Providers.OfType<ConfigurationFeatureProvider>().Count());
            Assert.Equal(1, flipper.Providers.OfType<RoleFeatureProvider>().Count());

            Assert.NotNull(Features.Services);
        }
    }
}
