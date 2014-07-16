namespace FeatureFlipper.Unity.Tests
{
    using Microsoft.Practices.Unity;
    using Moq;
    using Xunit;

    public class FeatureFlipperUnityTests
    {
        [Fact]
        public void Resolve_FeatureOn_ReturnsInstance()
        {
            // Arrange
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>();
            bool isOn = true;
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), It.IsAny<string>(), out isOn))
                .Returns(true);
            UnityContainer container = new UnityContainer();
            container.AddFeatureFlippingExtension(flipper.Object);
            container.RegisterType<IFeature, Feature1>();

            // Act
            var result = container.Resolve<IFeature>();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Feature1>(result);
        }

        [Fact]
        public void Resolve_FeatureOn_ReturnsNullObject()
        {
            // Arrange
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>();
            bool isOn = false;
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), It.IsAny<string>(), out isOn))
                .Returns(true);
            UnityContainer container = new UnityContainer();
            container.AddFeatureFlippingExtension(flipper.Object);
            container.RegisterType<IFeature, Feature1>();

            // Act
            var result = container.Resolve<IFeature>();

            // Assert
            Assert.NotNull(result);
            Assert.IsNotType<Feature1>(result);
        }

        [Fact]
        public void Resolve_FeatureVersionOn_ReturnsVersion2()
        {
            // Arrange
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>();
            bool isOn = true;
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), "V1", out isOn))
                .Returns(false);
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), "V2", out isOn))
                .Returns(true);
            UnityContainer container = new UnityContainer();
            container.AddFeatureFlippingExtension(flipper.Object);
            container.AddFeatureVersioningExtension(flipper.Object);
            container.RegisterType<IFeature, Feature1>("V1");
            container.RegisterType<IFeature, Feature2>("V2");

            // Act
            var result = container.Resolve<IFeature>();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Feature2>(result);
        }
    }
}
