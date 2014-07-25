namespace FeatureFlipper.Tests
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using Xunit;

    public class DefaultFeatureExplorerTests
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DefaultFeatureExplorer(null));
        }
        
        [Fact]
        public void GetFeatures_NoType_ReturnsEmptyDictionary()
        {
            // Arrange
            Mock<IFeatureMetadataStore> store = new Mock<IFeatureMetadataStore>(MockBehavior.Strict);
            store
                .Setup(s => s.Value)
                .Returns(new Dictionary<string, Dictionary<string, FeatureMetadata>>());
            Mock<ITypeResolver> typeResolver = new Mock<ITypeResolver>(MockBehavior.Strict);
            typeResolver
                .Setup(r => r.GetTypes())
                .Returns((Type[])null);
            DefaultFeatureExplorer explorer = new DefaultFeatureExplorer(store.Object);

            // Act
            var result = explorer.GetFeatures();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void GetFeatures_ReturnsFeatureDictionary()
        {
            // Arrange  
            Dictionary<string, FeatureMetadata> feature1 = new Dictionary<string, FeatureMetadata>();
            feature1.Add(string.Empty, new FeatureMetadata("X", null, this.GetType(), null, null));

            Dictionary<string, FeatureMetadata> feature2 = new Dictionary<string, FeatureMetadata>();
            feature2.Add(string.Empty, new FeatureMetadata("Y", null, this.GetType(), null, null));
            
            Dictionary<string, FeatureMetadata> feature3 = new Dictionary<string, FeatureMetadata>();
            feature3.Add(string.Empty, new FeatureMetadata("Z", null, this.GetType(), null, null));
            Mock<IFeatureMetadataStore> store = new Mock<IFeatureMetadataStore>(MockBehavior.Strict);
            store
                .Setup(s => s.Value)
                .Returns(new Dictionary<string, Dictionary<string, FeatureMetadata>>()
                         {
                             { "X", feature1 },
                             { "Y", feature2 },
                             { "Z", feature3 }
                         });
            DefaultFeatureExplorer explorer = new DefaultFeatureExplorer(store.Object);

            // Act
            var result = explorer.GetFeatures();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }
    }
}
