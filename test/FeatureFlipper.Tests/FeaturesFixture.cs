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

            Assert.NotNull(Features.FeatureStateParsers);
            Assert.Equal(3, Features.FeatureStateParsers.Count);
            Assert.Equal(1, Features.FeatureStateParsers.OfType<BooleanFeatureStateParser>().Count());
            Assert.Equal(1, Features.FeatureStateParsers.OfType<DateFeatureStateParser>().Count());
            Assert.Equal(1, Features.FeatureStateParsers.OfType<VersionStateParser>().Count());
   
            Assert.NotNull(Features.Providers);
            Assert.Equal(2, Features.Providers.Count);
            Assert.Equal(1, Features.Providers.OfType<ConfigurationFeatureProvider>().Count());
            Assert.Equal(1, Features.Providers.OfType<RoleFeatureProvider>().Count());

            Assert.NotNull(Features.ConfigurationReader);
            Assert.IsType<DefaultConfigurationReader>(Features.ConfigurationReader);
        }

        [Fact]
        public void SetFlipper()
        {
            // Arrange
            var flipper = new Mock<IFeatureFlipper>();
            var originalFlipper = Features.Flipper;

            // Act
            Features.Flipper = flipper.Object;

            // Assert
            Assert.NotNull(Features.Flipper);
            Assert.Equal(flipper.Object, Features.Flipper);

            // Teardown
            Features.Flipper = originalFlipper;
        }

        [Fact]
        public void SetConfigurationReader_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Features.ConfigurationReader = null);
        }

        [Fact]
        public void SetConfigurationReader()
        {
            // Arrange
            var reader = new Mock<IConfigurationReader>();
            var originnalReader = Features.ConfigurationReader;

            // Act
            Features.ConfigurationReader = reader.Object;

            // Assert
            Assert.NotNull(Features.ConfigurationReader);
            Assert.Equal(reader.Object, Features.ConfigurationReader);

            // Teardown
            Features.ConfigurationReader = originnalReader;
        }

        [Fact]
        public void SetFlipper_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Features.Flipper = null);
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
