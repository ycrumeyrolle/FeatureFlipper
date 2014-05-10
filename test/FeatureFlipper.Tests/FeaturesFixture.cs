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
            Assert.Equal(1, flipper.Providers.OfType<PerRoleFeatureProvider>().Count());

            Assert.NotNull(Features.FeatureStateParsers);
            Assert.Equal(2, Features.FeatureStateParsers.Count);
            Assert.Equal(1, Features.FeatureStateParsers.OfType<BooleanFeatureStateParser>().Count());
            Assert.Equal(1, Features.FeatureStateParsers.OfType<DateFeatureStateParser>().Count());
   
            Assert.NotNull(Features.Providers);
            Assert.Equal(2, Features.Providers.Count);
            Assert.Equal(1, Features.Providers.OfType<ConfigurationFeatureProvider>().Count());
            Assert.Equal(1, Features.Providers.OfType<PerRoleFeatureProvider>().Count());
        }

        [Fact]
        public void SetFlipper()
        {
            // Arrange
            var flipper = new Mock<IFeatureFlipper>();
            var originalFlipper = Features.Flipper;

            // Act
            Features.SetFlipper(flipper.Object);

            // Assert
            Assert.NotNull(Features.Flipper);
            Assert.Equal(flipper.Object, Features.Flipper);

            // Teardown
            Features.SetFlipper(originalFlipper);
        }

        [Fact]
        public void SetFlipper_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Features.SetFlipper(null));
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
