namespace FeatureFlipper.Unity.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Moq;
    using Xunit;

    public class FeatureVersionExtensionFixture
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FeatureVersionExtension(null));
            Assert.DoesNotThrow(() => new FeatureVersionExtension(new Mock<IFeatureFlipper>().Object));
        }
    }
}
