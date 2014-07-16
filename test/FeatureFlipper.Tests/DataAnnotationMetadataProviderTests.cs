namespace FeatureFlipper.Tests
{
    using System;
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
            Mock<ITypeResolver> typeResolver = new Mock<ITypeResolver>(MockBehavior.Strict);
            typeResolver
                .Setup(r => r.GetTypes())
                .Returns((Type[])null);
            DataAnnotationMetadataProvider provider = new DataAnnotationMetadataProvider(typeResolver.Object);

            // Act
            var result = provider.GetMetadata("X");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetMetadata_ReturnsFeatureDictionary()
        {
            // Arrange  
            Mock<Type> type1 = new Mock<Type>();
            type1.SetupAllProperties();
            type1
                .Setup(t => t.GetCustomAttributes(typeof(FeatureAttribute), It.IsAny<bool>()))
                .Returns(new[] { new FeatureAttribute("X") });

            Mock<Type> type2 = new Mock<Type>();
            type2.SetupAllProperties();
            type2
                .Setup(t => t.GetCustomAttributes(typeof(FeatureAttribute), It.IsAny<bool>()))
                .Returns(new[] { new FeatureAttribute("Y") });

            Mock<ITypeResolver> typeResolver = new Mock<ITypeResolver>(MockBehavior.Strict);
            typeResolver
                .Setup(r => r.GetTypes())
                .Returns(new[] { type1.Object, type2.Object });
            DataAnnotationMetadataProvider provider = new DataAnnotationMetadataProvider(typeResolver.Object);

            // Act
            var result = provider.GetMetadata("X");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Name, "X");
            Assert.NotNull(result.FeatureType);
            Assert.NotNull(result.GetRoles());
        }

        [Fact]
        public void GetMetadata_Doublon_ThrowsException()
        {
            // Arrange  
            Mock<Type> type1 = new Mock<Type>();
            type1.SetupAllProperties();
            type1
                .Setup(t => t.GetCustomAttributes(typeof(FeatureAttribute), It.IsAny<bool>()))
                .Returns(new[] { new FeatureAttribute("X") });

            Mock<ITypeResolver> typeResolver = new Mock<ITypeResolver>(MockBehavior.Strict);
            typeResolver
                .Setup(r => r.GetTypes())
                .Returns(new[] { type1.Object, type1.Object });
            DataAnnotationMetadataProvider provider = new DataAnnotationMetadataProvider(typeResolver.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => provider.GetMetadata("X"));
        }

        [Fact]
        public void GetMetadata_Cycles_ThrowsException()
        {
            // Arrange  
            Mock<Type> type1 = new Mock<Type>();
            type1.SetupAllProperties();
            type1
                .Setup(t => t.GetCustomAttributes(typeof(FeatureAttribute), It.IsAny<bool>()))
                .Returns(new[] { new FeatureAttribute("X") { DependsOn = "Y" } });
            Mock<Type> type2 = new Mock<Type>();
            type2.SetupAllProperties();
            type2
                .Setup(t => t.GetCustomAttributes(typeof(FeatureAttribute), It.IsAny<bool>()))
                .Returns(new[] { new FeatureAttribute("Y") { DependsOn = "X" } });

            Mock<ITypeResolver> typeResolver = new Mock<ITypeResolver>(MockBehavior.Strict);
            typeResolver
                .Setup(r => r.GetTypes())
                .Returns(new[] { type1.Object, type2.Object });
            DataAnnotationMetadataProvider provider = new DataAnnotationMetadataProvider(typeResolver.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => provider.GetMetadata("X"));
        }
    }
}
