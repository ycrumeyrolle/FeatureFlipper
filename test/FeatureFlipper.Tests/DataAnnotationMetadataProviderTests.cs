namespace FeatureFlipper.Tests
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using Xunit;

    public class DataAnnotationMetadataProviderTests
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DataAnnotationMetadataProvider(null));
        }

        [Fact]
        public void GetMetadata_GuardClause()
        {
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => MetadataProviderExtensions.GetMetadata(null, string.Empty));
        }

        [Fact]
        public void GetMetadata_NoType_ReturnNull()
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
            DataAnnotationMetadataProvider provider = new DataAnnotationMetadataProvider(store.Object);

            // Act
            var result = provider.GetMetadata("X");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetMetadata_ReturnsFeatureDictionary()
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
            DataAnnotationMetadataProvider provider = new DataAnnotationMetadataProvider(store.Object);

            // Act
            var result = provider.GetMetadata("X");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Name, "X");
            Assert.NotNull(result.FeatureType);
            Assert.NotNull(result.GetRoles());
            Assert.NotNull(result.GetDependsOn());
        }
    }
}
