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
            Assert.Throws<ArgumentNullException>(() => flipper.TryIsOn(null, null, out isOn));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsOn(bool providerIsOn)
        {
            // Arrange
            Mock<IFeatureProvider> provider = new Mock<IFeatureProvider>();
            provider
                .Setup(p => p.TryIsOn(It.IsAny<string>(), It.IsAny<string>(), out providerIsOn))
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
                .Setup(p => p.TryIsOn(It.IsAny<string>(), It.IsAny<string>(), out providerIsOn))
                .Returns(false);

            var providers = new[] { provider.Object };
            var flipper = new DefaultFeatureFlipper(providers);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => flipper.IsOn("X"));
        }
    }
}
