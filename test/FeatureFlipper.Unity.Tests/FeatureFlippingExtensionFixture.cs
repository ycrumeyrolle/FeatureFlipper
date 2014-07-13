namespace FeatureFlipper.Unity.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Moq;
    using Xunit;

    public class FeatureFlippingExtensionFixture
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
