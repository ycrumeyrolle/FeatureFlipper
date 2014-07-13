namespace FeatureFlipper.Tests
{
    using System;
    using System.Linq;
    using Moq;
    using Xunit;

    public class FeaturesFixture : IDisposable
    {
        private readonly IFeatureFlipper flipper;

        public FeaturesFixture()
        {
            this.flipper = Features.Flipper;
        }

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

        [Fact]
        public void SetFlipperToNull_ThrowsArgumentNullException()
        {
            // Act
            Assert.Throws<ArgumentNullException>(() => Features.Flipper = null);
        }

        [Fact]
        public void SetFlipper()
        {
            // Arrange
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>();
            Features.Flipper = flipper.Object;

            // Act
            var result = Features.Flipper;

            // Act 
            Assert.Same(flipper.Object, result);
        }

        public void Dispose()
        {
            Features.Flipper = this.flipper;
        }
    }
}
