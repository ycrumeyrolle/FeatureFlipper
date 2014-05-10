namespace FeatureFlipper.Unity.Tests
{
    using System;
    using Microsoft.Practices.ObjectBuilder2;
    using Moq;
    using Xunit;

    public class FlippingBuilderStrategyFixture
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FlippingBuilderStrategy(null));
        }

        [Fact]
        public void PreBuildUp_GuardClause()
        {
            // Arrange
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            FlippingBuilderStrategy strategy = new FlippingBuilderStrategy(flipper.Object);

            Assert.Throws<ArgumentNullException>(() => strategy.PreBuildUp(null));
        }

        [Fact]
        public void PreBuildUp_FeatureOn()
        {
            // Arrange
            bool isOn = true;
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), out isOn))
                .Returns(true);

            FlippingBuilderStrategy strategy = new FlippingBuilderStrategy(flipper.Object);

            Mock<IBuilderContext> context = new Mock<IBuilderContext>();
            context.SetupAllProperties();
            context.Object.BuildKey = new NamedTypeBuildKey(typeof(Feature1));

            // Act
            strategy.PreBuildUp(context.Object);

            // Assert
            Assert.Null(context.Object.Existing);
            Assert.False(context.Object.BuildComplete);
        }

        [Fact]
        public void PreBuildUp_FeatureOff()
        {
            // Arrange
            bool isOn = false;
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), out isOn))
                .Returns(true);

            FlippingBuilderStrategy strategy = new FlippingBuilderStrategy(flipper.Object);

            Mock<IBuilderContext> context = new Mock<IBuilderContext>();
            context.SetupAllProperties();
            context
                .Setup(c => c.OriginalBuildKey)
                .Returns(new NamedTypeBuildKey(typeof(IFeature1)));
            context.Object.BuildKey = new NamedTypeBuildKey(typeof(Feature1));

            // Act
            strategy.PreBuildUp(context.Object);

            // Assert
            Assert.NotNull(context.Object.Existing);
            Assert.True(context.Object.BuildComplete);
        }

        [Fact]
        public void PreBuildUp_FeatureUnknown()
        {
            // Arrange
            bool isOn = false;
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), out isOn))
                .Returns(false);

            FlippingBuilderStrategy strategy = new FlippingBuilderStrategy(flipper.Object);

            Mock<IBuilderContext> context = new Mock<IBuilderContext>();
            context.SetupAllProperties();
            context
                .Setup(c => c.OriginalBuildKey)
                .Returns(new NamedTypeBuildKey(typeof(IFeature1)));
            context.Object.BuildKey = new NamedTypeBuildKey(typeof(Feature1));

            // Act
            strategy.PreBuildUp(context.Object);

            // Assert
            Assert.Null(context.Object.Existing);
            Assert.False(context.Object.BuildComplete);
        }
    }
}
