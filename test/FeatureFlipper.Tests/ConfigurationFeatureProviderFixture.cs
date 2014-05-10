namespace FeatureFlipper.Tests
{
    using System;
    using Moq;
    using Xunit;

    public class ConfigurationFeatureProviderFixture
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            Assert.Throws<ArgumentNullException>(() => new ConfigurationFeatureProvider(null, null));
            Assert.Throws<ArgumentNullException>(() => new ConfigurationFeatureProvider(new Mock<IConfigurationReader>().Object, null));
        }

        [Theory]
        [InlineData("f1", null, false)]
        [InlineData("f1", "X", true)]
        [InlineData("f1", "X", false)]
        public void TryIsOn(string feature, string featureValue, bool expectedResult)
        {
            // Arrange
            bool isOn;
            Mock<IConfigurationReader> reader = new Mock<IConfigurationReader>(MockBehavior.Strict);
            reader
                .Setup(r => r.GetValue(feature))
                .Returns(featureValue);

            Mock<IFeatureStateParser> parser = new Mock<IFeatureStateParser>(MockBehavior.Strict);
            parser
                .Setup(p => p.TryParse(featureValue, out isOn))
                .Returns(expectedResult);

            ConfigurationFeatureProvider provider = new ConfigurationFeatureProvider(reader.Object, new[] { parser.Object });

            // Act
            var result = provider.TryIsOn(feature, out isOn);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("f1_name", "f1_key", null, false)]
        [InlineData("f1_name", "f1_key", "X", true)]
        [InlineData("f1_name", "f1_key", "X", false)]
        public void TryIsOn_RegisteredFeature(string featureName, string featureKey, string featureValue, bool expectedResult)
        {
            // Arrange
            bool isOn;
            Mock<IConfigurationReader> reader = new Mock<IConfigurationReader>(MockBehavior.Strict);
            reader
                .Setup(r => r.GetValue(featureKey))
                .Returns(featureValue);

            Mock<IFeatureStateParser> parser = new Mock<IFeatureStateParser>(MockBehavior.Strict);
            parser
                .Setup(p => p.TryParse(featureValue, out isOn))
                .Returns(expectedResult);

            ConfigurationFeatureProvider provider = new ConfigurationFeatureProvider(reader.Object, new[] { parser.Object });
            provider.RegisterFeature(featureName, featureKey);

            // Act
            var resultByName = provider.TryIsOn(featureName, out isOn);

            // Assert
            Assert.Equal(expectedResult, resultByName);
        }

        [Fact]
        public void TryIsOn_GuardClause()
        {
            // Arrange
            bool isOn;
            ConfigurationFeatureProvider provider = new ConfigurationFeatureProvider();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => provider.TryIsOn(null, out isOn));
        }

        [Fact]
        public void RegisterFeature_GuardClause()
        {
            // Arrange
            Mock<IFeatureStateParser> parser = new Mock<IFeatureStateParser>(MockBehavior.Strict);
            var provider = new ConfigurationFeatureProvider(new Mock<IConfigurationReader>().Object, new[] { parser.Object });

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => provider.RegisterFeature(null, "key"));
            Assert.Throws<ArgumentNullException>(() => provider.RegisterFeature("name", null));
        }

        [Fact]
        public void RegisterFeature_Doublon_ThrowsExceptioon()
        {
            // Arrange
            Mock<IFeatureStateParser> parser = new Mock<IFeatureStateParser>(MockBehavior.Strict);
            var provider = new ConfigurationFeatureProvider(new Mock<IConfigurationReader>().Object, new[] { parser.Object });

            // Act & Assert
            provider.RegisterFeature("name", "key");
            Assert.Throws<InvalidOperationException>(() => provider.RegisterFeature("name", "key"));
        }
    }
}
