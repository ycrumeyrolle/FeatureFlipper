namespace FeatureFlipper.Unity.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using Moq;
    using Xunit;

    public class FeatureUnityContainerExtensionsFixture
    {
        [Fact]
        public void Resolve_ReturnsConcrete()
        {
            // Arrange
            bool isOnValue = true;
            Mock<IFeatureProvider> featureProvider = new Mock<IFeatureProvider>(MockBehavior.Strict);
            featureProvider
                .Setup(p => p.TryIsOn(It.IsAny<FeatureMetadata>(), out isOnValue))
                .Returns(true);

            Mock<IMetadataProvider> metadataProvider = new Mock<IMetadataProvider>();
            metadataProvider
                .Setup(p => p.GetMetadata(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new FeatureMetadata("X", null, this.GetType(), null, null));

            IFeatureFlipper flipper = new DefaultFeatureFlipper(new[] { featureProvider.Object }, metadataProvider.Object);

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IService, Service>();
                container.AddFeatureFlippingExtension(flipper);

                // Act
                var service = container.Resolve<IService>();

                // Assert
                Assert.NotNull(service);
                Assert.IsType<Service>(service);
                Assert.Equal(42, service.IntProperty);
                Assert.Equal(42, service.GetValue());
                Assert.Equal("A", service.StringMethod());
                Assert.NotNull(service.ArrayMethod());
                Assert.Equal(3, service.ArrayMethod().Count());
                Assert.NotNull(service.CollectionMethod());
                Assert.Equal(3, service.CollectionMethod().Count());
                Assert.NotNull(service.IEnumerableMethod());
                Assert.Equal(4, service.IEnumerableMethod().Count());
                Assert.NotNull(service.ObjectMethod());
                int value = 10;
                service.OutMethod(out value);
                Assert.Equal(42, value);
                string[] array;
                service.OutArrayMethod(out array);
                Assert.Equal(3, array.Length);
                string outString;
                service.OutStringMethod(out outString);
                Assert.Equal("42", outString);
                IEnumerable<string> outEnumerable;
                service.OutEnumerableMethod(out outEnumerable);
                Assert.NotNull(outEnumerable);
                Assert.Equal(1, outEnumerable.Count());
                Assert.Equal("42", outEnumerable.First());
                ICollection<string> outCollection;
                service.OutCollectionMethod(out outCollection);
                Assert.NotNull(outCollection);
                Assert.Equal(1, outCollection.Count());
                Assert.Equal("42", outCollection.First());
            }
        }

        [Fact]
        public void Resolve_ReturnsNullObject()
        {
            // Arrange  
            bool isOnValue = false;
            Mock<IFeatureProvider> featureProvider = new Mock<IFeatureProvider>(MockBehavior.Strict);
            featureProvider
                .Setup(p => p.TryIsOn(It.IsAny<FeatureMetadata>(), out isOnValue))
                .Returns(true);

            Mock<IMetadataProvider> metadataProvider = new Mock<IMetadataProvider>();
            metadataProvider
                .Setup(p => p.GetMetadata(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new FeatureMetadata("X", null, this.GetType(), null, null));

            IFeatureFlipper flipper = new DefaultFeatureFlipper(new[] { featureProvider.Object }, metadataProvider.Object);

            using (IUnityContainer container = new UnityContainer())
            {
                container.AddFeatureFlippingExtension(flipper);

                container.RegisterType<IService, Service>();

                // Act
                var service = container.Resolve<IService>();

                // Assert
                Assert.NotNull(service);
                Assert.IsNotType<Service>(service);
                Assert.IsAssignableFrom<IService>(service);

                Assert.IsAssignableFrom(typeof(int), service.IntProperty);
                Assert.Equal(0, service.IntProperty);

                Assert.IsAssignableFrom(typeof(string), service.StringMethod());
                Assert.Equal(string.Empty, service.StringMethod());

                Assert.IsAssignableFrom(typeof(int), service.GetValue());
                Assert.Equal(0, service.GetValue());

                Assert.IsAssignableFrom(typeof(string[]), service.ArrayMethod());
                Assert.NotNull(service.ArrayMethod());
                Assert.Equal(0, service.ArrayMethod().Count());

                Assert.IsAssignableFrom(typeof(ICollection<bool>), service.CollectionMethod());
                Assert.NotNull(service.CollectionMethod());
                Assert.Equal(0, service.CollectionMethod().Count());

                Assert.IsAssignableFrom(typeof(IEnumerable<int>), service.IEnumerableMethod());
                Assert.NotNull(service.IEnumerableMethod());
                Assert.Equal(0, service.IEnumerableMethod().Count());

                Assert.Null(service.ObjectMethod());

                int value = 10;
                service.OutMethod(out value);
                Assert.Equal(10, value);

                string[] array;
                service.OutArrayMethod(out array);
                Assert.NotNull(array);
                Assert.IsAssignableFrom(typeof(string[]), array);
                Assert.Equal(0, array.Length);

                string outString = "x";
                service.OutStringMethod(out outString);
                Assert.Equal(string.Empty, outString);

                IEnumerable<string> outEnumerable = new[] { "x" };
                service.OutEnumerableMethod(out outEnumerable);
                Assert.NotNull(outEnumerable);
                Assert.IsAssignableFrom(typeof(IEnumerable<string>), outEnumerable);
                Assert.Equal(0, outEnumerable.Count());

                ICollection<string> outCollection;
                service.OutCollectionMethod(out outCollection);
                Assert.IsAssignableFrom(typeof(ICollection<string>), outCollection);
                Assert.NotNull(outCollection);
                Assert.Equal(0, outCollection.Count());
            }
        }

        [Fact]
        public void AddFlippingExtension()
        {
            // Arrange 
            IUnityContainer container = new UnityContainer();

            // Act
            Assert.DoesNotThrow(() => container.AddFeatureFlippingExtension());
        }

        [Fact]
        public void AddFlippingExtension_CustomInstance()
        {
            // Arrange 
            IUnityContainer container = new UnityContainer();
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>();

            // Act
            Assert.DoesNotThrow(() => container.AddFeatureFlippingExtension(flipper.Object));
        }

        [Fact]
        public void AddVersioningExtension()
        {
            // Arrange 
            IUnityContainer container = new UnityContainer();

            // Act
            Assert.DoesNotThrow(() => container.AddFeatureVersioningExtension());
        }

        [Fact]
        public void AddVersioningExtension_CustomInstance()
        {
            // Arrange 
            IUnityContainer container = new UnityContainer();
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>();

            // Act
            Assert.DoesNotThrow(() => container.AddFeatureVersioningExtension(flipper.Object));
        }
    }
}
