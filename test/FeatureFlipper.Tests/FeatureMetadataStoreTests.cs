namespace FeatureFlipper.Tests
{
    using System;
    using System.Collections.Generic;
    using FeatureFlipper.CycleDetection;
    using Moq;
    using Xunit;

    public class FeatureMetadataStoreTests
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FeatureMetadataStore(null, new Mock<ICycleDetector>().Object));
            Assert.Throws<ArgumentNullException>(() => new FeatureMetadataStore(new Mock<ITypeResolver>().Object, null));
        }
        
        [Fact]
        public void GetValue_NoType_ReturnNothing()
        {
            // Arrange
            Mock<ICycleDetector> detector = new Mock<ICycleDetector>(MockBehavior.Strict);

            Mock<ITypeResolver> resolver = new Mock<ITypeResolver>(MockBehavior.Strict);

            resolver
                .Setup(r => r.GetTypes())
                .Returns((Type[])null);
            FeatureMetadataStore store = new FeatureMetadataStore(resolver.Object, detector.Object);

            // Act
            var result = store.Value;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void GetValue_ReturnsFeatureDictionary()
        {
            // Arrange   
            Mock<ICycleDetector> detector = new Mock<ICycleDetector>(MockBehavior.Strict);
            detector
                .Setup(d => d.DetectCycles(It.IsAny<IEnumerable<FeatureMetadata>>()))
                .Returns(new string[0]);

            Mock<ITypeResolver> resolver = new Mock<ITypeResolver>(MockBehavior.Strict);

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

            resolver
                .Setup(r => r.GetTypes())
                .Returns(new[] { type1.Object, type2.Object });

            FeatureMetadataStore store = new FeatureMetadataStore(resolver.Object, detector.Object);

            // Act
            var result = store.Value;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
        
        [Fact]
        public void GetValue_Doublon_ThrowsException()
        {
            // Arrange   
            Mock<ICycleDetector> detector = new Mock<ICycleDetector>(MockBehavior.Strict);

            Mock<ITypeResolver> resolver = new Mock<ITypeResolver>(MockBehavior.Strict);

            Mock<Type> type1 = new Mock<Type>();
            type1.SetupAllProperties();
            type1
                .Setup(t => t.GetCustomAttributes(typeof(FeatureAttribute), It.IsAny<bool>()))
                .Returns(new[] { new FeatureAttribute("X") });
            
            resolver
                .Setup(r => r.GetTypes())
                .Returns(new[] { type1.Object, type1.Object });

            FeatureMetadataStore store = new FeatureMetadataStore(resolver.Object, detector.Object);
            
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => store.Value);
        }

        [Fact]
        public void GetValue_Cycles_ThrowsException()
        { 
            // Arrange   
            Mock<ICycleDetector> detector = new Mock<ICycleDetector>(MockBehavior.Strict);
            detector
                .Setup(d => d.DetectCycles(It.IsAny<IEnumerable<FeatureMetadata>>()))
                .Returns(new[] { "X -> Y" });
            
            Mock<ITypeResolver> resolver = new Mock<ITypeResolver>(MockBehavior.Strict);
            
            resolver
                  .Setup(r => r.GetTypes())
                  .Returns(new Type[0]);

            FeatureMetadataStore store = new FeatureMetadataStore(resolver.Object, detector.Object);
           
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => store.Value);
        }
    }
}
