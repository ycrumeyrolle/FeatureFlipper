namespace FeatureFlipper.Unity.Tests
{
    using System;
    using Moq;
    using Xunit;

    public class FeatureFlippingExtensionTests
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FeatureFlippingExtension(null));
            Assert.DoesNotThrow(() => new FeatureFlippingExtension(new Mock<IFeatureFlipper>().Object));
        }
    }
}
