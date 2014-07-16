namespace FeatureFlipper.Unity.Tests
{
    using System;
    using Moq;
    using Xunit;

    public class FeatureVersionExtensionTests
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
