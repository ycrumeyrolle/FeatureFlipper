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
                .Setup(p => p.TryIsOn(It.IsAny<string>(), out isOnValue))
                .Returns(true);

            IFeatureFlipper flipper = new DefaultFeatureFlipper(new[] { featureProvider.Object });

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
                Assert.NotNull(service.CollectionMethod());
                Assert.NotNull(service.IEnumerableMethod());
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
                .Setup(p => p.TryIsOn(It.IsAny<string>(), out isOnValue))
                .Returns(true);

            IFeatureFlipper flipper = new DefaultFeatureFlipper(new[] { featureProvider.Object });

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
                Assert.Equal(0, service.IntProperty);
                Assert.Equal(string.Empty, service.StringMethod());
                Assert.Equal(0, service.GetValue());
                Assert.NotNull(service.ArrayMethod());
                Assert.NotNull(service.CollectionMethod());
                Assert.NotNull(service.IEnumerableMethod());
                Assert.Null(service.ObjectMethod());
                int value = 10;
                service.OutMethod(out value);
                Assert.Equal(10, value);
                string[] array;
                service.OutArrayMethod(out array);
                Assert.NotNull(array);
                Assert.Equal(0, array.Length);
                string outString = "x";
                service.OutStringMethod(out outString);
                Assert.Equal(string.Empty, outString);
                IEnumerable<string> outEnumerable = new[] { "x" };
                service.OutEnumerableMethod(out outEnumerable);
                Assert.NotNull(outEnumerable);
                Assert.Equal(0, outEnumerable.Count());
                ICollection<string> outCollection;
                service.OutCollectionMethod(out outCollection);
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
            container.AddFeatureFlippingExtension();

            // Assert 
            var flipper = container.Resolve<IFeatureFlipper>();
            Assert.NotNull(flipper);
            Assert.Same(Features.Flipper, flipper);
        }
    }
}
