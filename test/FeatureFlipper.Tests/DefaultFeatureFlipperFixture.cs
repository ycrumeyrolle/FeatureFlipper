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
            Assert.Throws<ArgumentNullException>(() => new DefaultFeatureFlipper(null, new Mock<IMetadataProvider>().Object));
            Assert.Throws<ArgumentNullException>(() => new DefaultFeatureFlipper(new IFeatureProvider[0], null));
        }

        [Fact]
        public void Ctor()
        {
            // Arrange
            Mock<IFeatureProvider> provider = new Mock<IFeatureProvider>();
            var providers = new[] { provider.Object }.ToList();

            // Act 
            var flipper = new DefaultFeatureFlipper(providers, new Mock<IMetadataProvider>().Object);

            // Assert
            Assert.NotNull(flipper.Providers);
            Assert.Collection(flipper.Providers, item => providers.Contains(item));
        }

        [Fact]
        public void IsOn_GuardClause()
        {
            // Arrange
            var providers = new IFeatureProvider[0];
            var flipper = new DefaultFeatureFlipper(providers, new Mock<IMetadataProvider>().Object);
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
                .Setup(p => p.TryIsOn(It.IsAny<FeatureMetadata>(), out providerIsOn))
                .Returns(true);

            var providers = new[] { provider.Object };
            Mock<IMetadataProvider> metadataProvider = new Mock<IMetadataProvider>();
            metadataProvider
                .Setup(p => p.GetMetadata(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new FeatureMetadata("X", null, this.GetType(), null, null));
            var flipper = new DefaultFeatureFlipper(providers, metadataProvider.Object);

            // Act 
            var isOn = flipper.IsOn("X");

            // Assert
            Assert.Equal(providerIsOn, isOn);
        }

        [Fact]
        public void IsOn_UnknowMetadata_UseBaseMetadata()
        {
            // Arrange
            bool providerIsOn = true;
            Mock<IFeatureProvider> provider = new Mock<IFeatureProvider>();
            provider
                .Setup(p => p.TryIsOn(It.Is<FeatureMetadata>(m => m.Name == "name" && m.Version == "version"), out providerIsOn))
                .Returns(true)
                .Verifiable();

            var providers = new[] { provider.Object };
            Mock<IMetadataProvider> metadataProvider = new Mock<IMetadataProvider>();
            metadataProvider
                .Setup(p => p.GetMetadata(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((FeatureMetadata)null);
            var flipper = new DefaultFeatureFlipper(providers, metadataProvider.Object);
            bool isOn;

            // Act 
            var result = flipper.TryIsOn("name", "version", out isOn);

            // Assert
            Assert.Equal(providerIsOn, isOn);
            provider.Verify();
        }

        [Fact]
        public void IsOn_NoProvider_ThrowsException()
        {
            // Arrange
            bool providerIsOn;
            Mock<IFeatureProvider> provider = new Mock<IFeatureProvider>();
            provider
                .Setup(p => p.TryIsOn(It.IsAny<FeatureMetadata>(), out providerIsOn))
                .Returns(false);

            Mock<IMetadataProvider> metadataProvider = new Mock<IMetadataProvider>();
            metadataProvider
                .Setup(p => p.GetMetadata(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new FeatureMetadata("X", null, this.GetType(), null, null));

            var providers = new[] { provider.Object };
            var flipper = new DefaultFeatureFlipper(providers, metadataProvider.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => flipper.IsOn("X"));
        }
        
        [Fact]
        public void IsOn_WithDependencies()
        {
            // Arrange
            FeatureMetadata featureX = new FeatureMetadata("X", null, this.GetType(), null, "Y, Z");
            FeatureMetadata featureY = new FeatureMetadata("Y", null, this.GetType(), null, "Z");
            FeatureMetadata featureZ = new FeatureMetadata("Z", null, this.GetType(), null, null);
            Mock<IFeatureProvider> provider = new Mock<IFeatureProvider>();
            bool providerIsOn = true;
            provider
                .Setup(p => p.TryIsOn(featureX, out providerIsOn))
                .Returns(true);
            provider
                .Setup(p => p.TryIsOn(featureY, out providerIsOn))
                .Returns(true);
            provider
                .Setup(p => p.TryIsOn(featureZ, out providerIsOn))
                .Returns(true);

            var providers = new[] { provider.Object };
            Mock<IMetadataProvider> metadataProvider = new Mock<IMetadataProvider>();
            metadataProvider
                .Setup(p => p.GetMetadata("X", It.IsAny<string>()))
                .Returns(featureX);
            metadataProvider
                .Setup(p => p.GetMetadata("Y", It.IsAny<string>()))
                .Returns(featureY);
            metadataProvider
                .Setup(p => p.GetMetadata("Z", It.IsAny<string>()))
                .Returns(featureZ);
            var flipper = new DefaultFeatureFlipper(providers, metadataProvider.Object);

            // Act 
            var isOn = flipper.IsOn("X");

            // Assert
            Assert.True(isOn);
        }

        [Fact]
        public void IsOn_WithUnknowDependencies_ReturnsFalseFalse()
        {
            // Arrange
            FeatureMetadata featureX = new FeatureMetadata("X", null, this.GetType(), null, "Y, Z");
            FeatureMetadata featureY = new FeatureMetadata("Y", null, this.GetType(), null, "Z");
            FeatureMetadata featureZ = new FeatureMetadata("Z", null, this.GetType(), null, null);
            Mock<IFeatureProvider> provider = new Mock<IFeatureProvider>();
            bool isOnX = true;
            bool isOnY = true;
            bool isOnZ = false;
            provider
                .Setup(p => p.TryIsOn(featureX, out isOnX))
                .Returns(true);
            provider
                .Setup(p => p.TryIsOn(featureY, out isOnY))
                .Returns(true);
            provider
                .Setup(p => p.TryIsOn(featureZ, out isOnZ))
                .Returns(false);

            var providers = new[] { provider.Object };
            Mock<IMetadataProvider> metadataProvider = new Mock<IMetadataProvider>();
            metadataProvider
                .Setup(p => p.GetMetadata("X", It.IsAny<string>()))
                .Returns(featureX);
            metadataProvider
                .Setup(p => p.GetMetadata("Y", It.IsAny<string>()))
                .Returns(featureY);
            metadataProvider
                .Setup(p => p.GetMetadata("Z", It.IsAny<string>()))
                .Returns(featureZ);
            var flipper = new DefaultFeatureFlipper(providers, metadataProvider.Object);
            bool isOn;

            // Act 
            var result = flipper.TryIsOn("X", null, out isOn);

            // Assert
            Assert.False(result);
            Assert.False(isOn);
        }

        [Fact]
        public void IsOn_WithDependenciesOff_ReturnsTrueFalse()
        {
            // Arrange
            FeatureMetadata featureX = new FeatureMetadata("X", null, this.GetType(), null, "Y, Z");
            FeatureMetadata featureY = new FeatureMetadata("Y", null, this.GetType(), null, "Z");
            FeatureMetadata featureZ = new FeatureMetadata("Z", null, this.GetType(), null, null);
            Mock<IFeatureProvider> provider = new Mock<IFeatureProvider>();
            bool isOnX = true;
            bool isOnY = true;
            bool isOnZ = false;
            provider
                .Setup(p => p.TryIsOn(featureX, out isOnX))
                .Returns(true);
            provider
                .Setup(p => p.TryIsOn(featureY, out isOnY))
                .Returns(true);
            provider
                .Setup(p => p.TryIsOn(featureZ, out isOnZ))
                .Returns(true);

            var providers = new[] { provider.Object };
            Mock<IMetadataProvider> metadataProvider = new Mock<IMetadataProvider>();
            metadataProvider
                .Setup(p => p.GetMetadata("X", It.IsAny<string>()))
                .Returns(featureX);
            metadataProvider
                .Setup(p => p.GetMetadata("Y", It.IsAny<string>()))
                .Returns(featureY);
            metadataProvider
                .Setup(p => p.GetMetadata("Z", It.IsAny<string>()))
                .Returns(featureZ);
            var flipper = new DefaultFeatureFlipper(providers, metadataProvider.Object);
            bool isOn;

            // Act 
            var result = flipper.TryIsOn("X", null, out isOn);

            // Assert
            Assert.True(result);
            Assert.False(isOn);
        }
    }
}
