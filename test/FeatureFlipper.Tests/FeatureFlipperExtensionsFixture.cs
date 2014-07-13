namespace FeatureFlipper.Tests
{
    using System;
    using Moq;
    using Xunit;

    public class FeatureFlipperExtensionsFixture
    {
        [Fact]
        public void IsOn_TFeature_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => FeatureFlipperExtensions.IsOn<Feature1>(null));
            Assert.Throws<ArgumentNullException>(() => FeatureFlipperExtensions.IsOn<Feature1>(null, null));
        }

        [Fact]
        public void IsOn_TFeature()
        {
            // Arrange
            bool isOn = true;
            var flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), It.IsAny<string>(), out isOn))
                .Returns(true);

            // Act
            var result = FeatureFlipperExtensions.IsOn<Feature1>(flipper.Object);

            // Assert
            Assert.True(result);
            Assert.True(isOn);
            flipper.Verify(f => f.TryIsOn(typeof(Feature1).FullName, It.IsAny<string>(), out isOn), Times.Once());
        }

        [Fact]
        public void IsOn_TFeature_WithVersion()
        {
            // Arrange
            bool isOn = true;
            var flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), It.IsAny<string>(), out isOn))
                .Returns(true);

            // Act
            var result = FeatureFlipperExtensions.IsOn<Feature1>(flipper.Object, "version");

            // Assert
            Assert.True(result);
            Assert.True(isOn);
            flipper.Verify(f => f.TryIsOn(typeof(Feature1).FullName, It.IsAny<string>(), out isOn), Times.Once());
        }

        [Fact]
        public void IsOn_GuardClause()
        {
            // Arrange
            var flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            bool isOn;
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), It.IsAny<string>(), out isOn))
                .Returns(true);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => FeatureFlipperExtensions.IsOn(null, typeof(Feature1)));
            Assert.Throws<ArgumentNullException>(() => FeatureFlipperExtensions.IsOn(flipper.Object, (Type)null));
            Assert.Throws<ArgumentNullException>(() => FeatureFlipperExtensions.IsOn(null, (string)null));
            Assert.Throws<ArgumentNullException>(() => FeatureFlipperExtensions.IsOn(null, (Type)null, null));
            Assert.Throws<ArgumentNullException>(() => FeatureFlipperExtensions.IsOn(null, (string)null, null));
            Assert.Throws<ArgumentNullException>(() => FeatureFlipperExtensions.IsOn(flipper.Object, (Type)null, null));
            Assert.DoesNotThrow(() => FeatureFlipperExtensions.IsOn(flipper.Object, this.GetType()));
            Assert.DoesNotThrow(() => FeatureFlipperExtensions.IsOn<object>(flipper.Object));
        }

        [Fact]
        public void IsOn()
        {
            // Arrange
            bool isOn = true;
            var flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), It.IsAny<string>(), out isOn))
                .Returns(true);

            // Act
            var result = FeatureFlipperExtensions.IsOn(flipper.Object, typeof(Feature1), null);

            // Assert
            Assert.True(result);
            Assert.True(isOn);
            flipper.Verify(f => f.TryIsOn(typeof(Feature1).FullName, It.IsAny<string>(), out isOn), Times.Once());
        }
        
        [Fact]
        public void RegisterConfigurationFeature_GuardClause()
        {
            // Arrange
            var flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => FeatureFlipperExtensions.RegisterConfigurationFeature<Feature1>(null, "key"));
            Assert.Throws<ArgumentNullException>(() => FeatureFlipperExtensions.RegisterConfigurationFeature(null, "feature", "key"));
            Assert.Throws<ArgumentNullException>(() => FeatureFlipperExtensions.RegisterConfigurationFeature(flipper.Object, null, "key"));
            Assert.Throws<ArgumentNullException>(() => FeatureFlipperExtensions.RegisterConfigurationFeature(flipper.Object, "feature", null));
        }

        [Fact]
        public void RegisterConfigurationFeature()
        {
            // Arrange
            bool isOn; 
            bool providerIsOn = true;
            var reader = new Mock<IConfigurationReader>(MockBehavior.Strict);
            Mock<IFeatureStateParser> parser = new Mock<IFeatureStateParser>(MockBehavior.Strict);
            parser
                .Setup(p => p.TryParse("X", It.IsAny<string>(), out providerIsOn))
                .Returns(true);
            var provider = new ConfigurationFeatureProvider(reader.Object, new[] { parser.Object });

            var flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), It.IsAny<string>(), out isOn))
                .Returns(true);

            flipper
                .Setup(f => f.Providers)
                .Returns(new[] { provider });

            // Act
            FeatureFlipperExtensions.RegisterConfigurationFeature<Feature2>(flipper.Object, "key1");
            FeatureFlipperExtensions.RegisterConfigurationFeature(flipper.Object, "feature2", "key2");

            // Assert
            Assert.Throws<InvalidOperationException>(() => FeatureFlipperExtensions.RegisterConfigurationFeature<Feature2>(flipper.Object, "key1"));
            Assert.Throws<InvalidOperationException>(() => FeatureFlipperExtensions.RegisterConfigurationFeature(flipper.Object, "feature2", "key2"));
        }

        [Fact]
        public void RegisterConfigurationFeature_NoConfigurationProvider_ThrowException()
        {
            // Arrange
            var flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            flipper
                .Setup(f => f.Providers)
                .Returns(new IFeatureProvider[0]);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => FeatureFlipperExtensions.RegisterConfigurationFeature(flipper.Object, "feature", "key"));
        }        
    }
}
