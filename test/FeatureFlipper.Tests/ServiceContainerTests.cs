namespace FeatureFlipper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using Xunit;

    public class ServiceContainerTests
    {
        [Theory]
        [MemberData("SingleServices")]
        public void GetService_Know_ReturnsService(Type serviceType)
        {
            // Arrange 
            var container = new ServiceContainer();

            // Act
            var result = container.GetService(serviceType);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom(serviceType, result);
        }

        [Theory]
        [MemberData("SingleServices")]
        public void GetService_Twice_ReturnsSameService(Type serviceType)
        {
            // Arrange 
            var container = new ServiceContainer();

            // Act
            var result1 = container.GetService(serviceType);
            var result2 = container.GetService(serviceType);

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Same(result1, result2);
        }

        [Fact]
        public void GetService_GuardClause()
        {
            // Arrange 
            var container = new ServiceContainer();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() => container.GetService(null));
        }

        [Fact]
        public void GetService_Unknow_ThrowsInvalidOperationException()
        {
            // Arrange 
            var container = new ServiceContainer();

            // Act & assert
            Assert.Throws<InvalidOperationException>(() => container.GetService(this.GetType()));
        }
        
        [Fact]
        public void GetServices_GuardClause()
        {
            // Arrange 
            var container = new ServiceContainer();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() => container.GetServices(null));
        }

        [Theory]
        [MemberData("MultiServices")]
        public void GetServices_Know_ReturnsServices(Type serviceType)
        { 
            // Arrange 
            var container = new ServiceContainer();
            
            // Act
            var result = container.GetServices(serviceType);

            // Assert
            Assert.NotNull(result);
            foreach (var item in result)
            {
                Assert.IsAssignableFrom(serviceType, item);
            }
        }

        [Theory]
        [MemberData("MultiServices")]
        public void GetServices_Twice_ReturnsSameServices(Type serviceType)
        {
            // Arrange 
            var container = new ServiceContainer();

            // Act
            var result1 = container.GetServices(serviceType);
            var result2 = container.GetServices(serviceType);

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Same(result1, result2);
        }

        [Fact]
        public void GetServices_Null_ReturnEmptyArray()
        {
            // Arrange 
            var container = new ServiceContainer();
            container.Replace(typeof(IFeatureStateParser), () => (object[])null);

            // Act 
            var services = container.GetServices<IFeatureStateParser>();
            
            // Assert
            Assert.NotNull(services);
            Assert.Empty(services);
        }

        [Fact]
        public void GetServices_Unknow_ThrowsInvalidOperationException()
        {
            // Arrange 
            var container = new ServiceContainer();

            // Act & assert
            Assert.Throws<InvalidOperationException>(() => container.GetServices(this.GetType()));
        }
        
        [Fact]
        public void Replace_GuardClause()
        {
            // Arrange 
            var container = new ServiceContainer();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() => container.Replace(null, () => new object()));
            Assert.Throws<ArgumentNullException>(() => container.Replace(this.GetType(), (Func<object>)null));
            Assert.Throws<ArgumentNullException>(() => container.Replace(null, () => new object[0]));
            Assert.Throws<ArgumentNullException>(() => container.Replace(this.GetType(), (Func<IEnumerable<object>>)null));
        }

        [Fact]
        public void Replace_UnkownService_ThrowsException()
        {
            // Arrange 
            var container = new ServiceContainer();

            // Act & assert
            Assert.Throws<ArgumentException>(() => container.Replace(this.GetType(), () => new object()));
            Assert.Throws<ArgumentException>(() => container.Replace(this.GetType(), () => new object[0]));
        }

        [Fact]
        public void ReplaceSingle_ReturnsReplacedService()
        {
            // Arrange 
            var container = new ServiceContainer();
            Mock<ISystemClock> clock = new Mock<ISystemClock>();

            // Act
            container.Replace(typeof(ISystemClock), clock.Object);

            // Assert
            var result = container.GetService(typeof(ISystemClock));
            Assert.NotNull(result);

            Assert.Same(clock.Object, result);
        }

        [Fact]
        public void ReplaceMulti_ReturnsReplacedServices()
        {
            // Arrange 
            var container = new ServiceContainer();
            Mock<IFeatureProvider> provider1 = new Mock<IFeatureProvider>();
            Mock<IFeatureProvider> provider2 = new Mock<IFeatureProvider>();

            // Act
            container.Replace(typeof(IFeatureProvider), new[] { provider1.Object, provider2.Object });

            // Assert
            var result = container.GetServices(typeof(IFeatureProvider));
            Assert.NotNull(result);

            Assert.Same(provider1.Object, result.ElementAt(0));
            Assert.Same(provider2.Object, result.ElementAt(1));
        }

        [Fact]
        public void RemoveAll()
        {
            // Arrange 
            var container = new ServiceContainer();

            // Act
            container.RemoveAll(typeof(IFeatureStateParser));

            // Assert
            var result = container.GetServices(typeof(IFeatureStateParser));
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        public static IEnumerable<object[]> SingleServices()
        {
            yield return new object[] { typeof(ISystemClock) };
            yield return new object[] { typeof(IConfigurationReader) };
            yield return new object[] { typeof(IAssembliesResolver) };
            yield return new object[] { typeof(IPrincipalProvider) };
            yield return new object[] { typeof(ITypeResolver) };
            yield return new object[] { typeof(IMetadataProvider) };
            yield return new object[] { typeof(IRoleMatrixProvider) };
            yield return new object[] { typeof(IPrincipalProvider) };
            yield return new object[] { typeof(IFeatureFlipper) };
        }

        public static IEnumerable<object[]> MultiServices()
        {
            yield return new object[] { typeof(IFeatureStateParser) };
            yield return new object[] { typeof(IFeatureProvider) };
        }
    }
}
