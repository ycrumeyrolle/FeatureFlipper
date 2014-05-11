namespace FeatureFlipper.Tests
{
    using System;
    using System.Linq;
    using Moq;
    using Xunit;

    public class DefaultFeatureFlipperFixture
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DefaultFeatureFlipper(null));
        }

        [Fact]
        public void Ctor()
        {
            // Arrange
            Mock<IFeatureProvider> provider = new Mock<IFeatureProvider>();
            var providers = new[] { provider.Object }.ToList();

            // Act 
            var flipper = new DefaultFeatureFlipper(providers);

            // Assert
            Assert.NotNull(flipper.Providers);
            Assert.Collection(flipper.Providers, item => providers.Contains(item));
        }

        [Fact]
        public void IsOn_GuardClause()
        {
            // Arrange
            var providers = new IFeatureProvider[0];
            var flipper = new DefaultFeatureFlipper(providers);
            bool isOn;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => flipper.TryIsOn(null, out isOn));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsOn(bool providerIsOn)
        {
            // Arrange
            Mock<IFeatureProvider> provider = new Mock<IFeatureProvider>();
            provider
                .Setup(p => p.TryIsOn(It.IsAny<string>(), out providerIsOn))
                .Returns(true);

            var providers = new[] { provider.Object };
            var flipper = new DefaultFeatureFlipper(providers);

            // Act 
            var isOn = flipper.IsOn("X");

            // Assert
            Assert.Equal(providerIsOn, isOn);
        }

        [Fact]
        public void IsOn_NoProvider_ThrowsException()
        {
            // Arrange
            bool providerIsOn;
            Mock<IFeatureProvider> provider = new Mock<IFeatureProvider>();
            provider
                .Setup(p => p.TryIsOn(It.IsAny<string>(), out providerIsOn))
                .Returns(false);

            var providers = new[] { provider.Object };
            var flipper = new DefaultFeatureFlipper(providers);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => flipper.IsOn("X"));
        }

        [Fact]
        public void IsOn_FastPath()
        {
            // Arrange
            const string FeatureName = "X";
            bool isOnValue = true;

            Mock<IFeatureProvider> firstFeatureProvider = new Mock<IFeatureProvider>(MockBehavior.Strict);
            firstFeatureProvider
                .Setup(p => p.TryIsOn(FeatureName, out isOnValue))
                .Returns(false);

            Mock<IFeatureProvider> secondFeatureProvider = new Mock<IFeatureProvider>(MockBehavior.Strict);
            secondFeatureProvider
                .Setup(p => p.TryIsOn(FeatureName, out isOnValue))
                .Callback(() => isOnValue = true)
                .Returns(true);

            Mock<IFeatureProvider> thirdFeatureProvider = new Mock<IFeatureProvider>(MockBehavior.Strict);
            thirdFeatureProvider
                .Setup(p => p.TryIsOn(FeatureName, out isOnValue))
                .Returns(true);

            var flipper = new DefaultFeatureFlipper(new[] { firstFeatureProvider.Object, secondFeatureProvider.Object, thirdFeatureProvider.Object });

            // Act
            bool isOn = flipper.IsOn(FeatureName);
            isOn = flipper.IsOn(FeatureName);

            Assert.Equal(true, isOn);

            firstFeatureProvider.Verify(p => p.TryIsOn(FeatureName, out isOnValue), Times.Once());
            secondFeatureProvider.Verify(p => p.TryIsOn(FeatureName, out isOnValue), Times.Exactly(2));
            thirdFeatureProvider.Verify(p => p.TryIsOn(FeatureName, out isOnValue), Times.Never());
        }
    }
}
