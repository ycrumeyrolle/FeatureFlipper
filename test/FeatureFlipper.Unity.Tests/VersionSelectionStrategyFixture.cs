namespace FeatureFlipper.Unity.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ObjectBuilder2;
    using Moq;
    using Xunit;

    public class VersionSelectionStrategyFixture
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new VersionSelectionStrategy(null, null));
            Assert.Throws<ArgumentNullException>(() => new VersionSelectionStrategy(new Mock<IFeatureFlipper>().Object, null));
        }

        [Fact]
        public void PreBuildUp_GuardClause()
        {
            // Arrange
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            VersionSelectionStrategy strategy = new VersionSelectionStrategy(flipper.Object, new Dictionary<Type, IList<TypeMapping>>());

            Assert.Throws<ArgumentNullException>(() => strategy.PreBuildUp(null));
        }

        [Fact]
        public void PreBuildUp_FeatureOn()
        {
            // Arrange
            bool isOn = true;
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), It.IsAny<string>(), out isOn))
                .Returns(true);

            IDictionary<Type, IList<TypeMapping>> featureVersionMapping = new Dictionary<Type, IList<TypeMapping>>();
            featureVersionMapping.Add(typeof(IFeature1), new List<TypeMapping>() { new TypeMapping { Type = typeof(Feature1), Name = "V1" } });

            VersionSelectionStrategy strategy = new VersionSelectionStrategy(flipper.Object, featureVersionMapping);

            Mock<IBuilderContext> context = new Mock<IBuilderContext>();
            context.SetupAllProperties();
            context.Object.BuildKey = new NamedTypeBuildKey(typeof(Feature1));
            context.Setup(c => c.OriginalBuildKey).Returns(new NamedTypeBuildKey(typeof(IFeature1)));

            // Act
            strategy.PreBuildUp(context.Object);

            // Assert
            Assert.Null(context.Object.Existing);
            Assert.NotNull(context.Object.BuildKey);
            Assert.Equal(typeof(Feature1), context.Object.BuildKey.Type);
            Assert.Equal(null, context.Object.BuildKey.Name);
        }

        [Fact]
        public void PreBuildUp_FeatureOff()
        {
            // Arrange
            bool isOn = false;
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), It.IsAny<string>(), out isOn))
                .Returns(true);

            IDictionary<Type, IList<TypeMapping>> featureVersionMapping = new Dictionary<Type, IList<TypeMapping>>();
            featureVersionMapping.Add(typeof(IFeature1), new List<TypeMapping>() { new TypeMapping { Type = typeof(Feature1), Name = "V1" } });

            VersionSelectionStrategy strategy = new VersionSelectionStrategy(flipper.Object, featureVersionMapping);

            Mock<IBuilderContext> context = new Mock<IBuilderContext>();
            context.SetupAllProperties();
            context.Object.BuildKey = new NamedTypeBuildKey(typeof(Feature1));
            context.Setup(c => c.OriginalBuildKey).Returns(new NamedTypeBuildKey(typeof(IFeature1)));

            // Act
            strategy.PreBuildUp(context.Object);

            // Assert
            Assert.Null(context.Object.Existing);
            Assert.NotNull(context.Object.BuildKey);
            Assert.Equal(typeof(Feature1), context.Object.BuildKey.Type);
            Assert.Equal("V1", context.Object.BuildKey.Name);
        }

        [Fact]
        public void PreBuildUp_FeatureUnknown()
        {
            // Arrange
            bool isOn = false;
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), It.IsAny<string>(), out isOn))
                .Returns(false);

            IDictionary<Type, IList<TypeMapping>> featureVersionMapping = new Dictionary<Type, IList<TypeMapping>>();
            featureVersionMapping.Add(typeof(IFeature1), new List<TypeMapping>() { new TypeMapping { Type = typeof(Feature1), Name = "V1" } });

            VersionSelectionStrategy strategy = new VersionSelectionStrategy(flipper.Object, featureVersionMapping);

            Mock<IBuilderContext> context = new Mock<IBuilderContext>();
            context.SetupAllProperties();
            context.Object.BuildKey = new NamedTypeBuildKey(typeof(Feature1));
            context.Setup(c => c.OriginalBuildKey).Returns(new NamedTypeBuildKey(typeof(IFeature1)));

            // Act
            strategy.PreBuildUp(context.Object);

            // Assert
            Assert.Null(context.Object.Existing);
            Assert.NotNull(context.Object.BuildKey);
            Assert.Equal(typeof(Feature1), context.Object.BuildKey.Type);
            Assert.Equal(null, context.Object.BuildKey.Name);
        }
    }
}
