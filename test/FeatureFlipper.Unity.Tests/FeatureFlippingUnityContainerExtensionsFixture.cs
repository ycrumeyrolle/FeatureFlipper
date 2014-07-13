namespace FeatureFlipper.Unity.Tests
{
    using System;
    using Microsoft.Practices.Unity;
    using Moq;
    using Xunit;
    
    public class FeatureFlippingUnityContainerExtensionsFixture
    {
        [Fact]
        public void AddFeatureFlippingExtension_GuardClause()
        {
            Mock<IUnityContainer> container = new Mock<IUnityContainer>();
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>();

            Assert.Throws<ArgumentNullException>(() => FeatureFlippingUnityContainerExtensions.AddFeatureFlippingExtension(null, flipper.Object));
            Assert.Throws<ArgumentNullException>(() => FeatureFlippingUnityContainerExtensions.AddFeatureFlippingExtension(container.Object, null));
            Assert.Throws<ArgumentNullException>(() => FeatureFlippingUnityContainerExtensions.AddFeatureFlippingExtension(null));
            Assert.DoesNotThrow(() => FeatureFlippingUnityContainerExtensions.AddFeatureFlippingExtension(container.Object, flipper.Object));
        }

        [Fact]
        public void AddFeatureVersioningExtension_GuardClause()
        {
            Mock<IUnityContainer> container = new Mock<IUnityContainer>();
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>();

            Assert.Throws<ArgumentNullException>(() => FeatureFlippingUnityContainerExtensions.AddFeatureVersioningExtension(null, flipper.Object));
            Assert.Throws<ArgumentNullException>(() => FeatureFlippingUnityContainerExtensions.AddFeatureVersioningExtension(container.Object, null));
            Assert.Throws<ArgumentNullException>(() => FeatureFlippingUnityContainerExtensions.AddFeatureVersioningExtension(null));
            Assert.DoesNotThrow(() => FeatureFlippingUnityContainerExtensions.AddFeatureVersioningExtension(container.Object, flipper.Object));
        }
    }
}
