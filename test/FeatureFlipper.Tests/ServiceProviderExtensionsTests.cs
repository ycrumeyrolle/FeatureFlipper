namespace FeatureFlipper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using Xunit;

    public class ServiceProviderExtensionsTests
    {
        [Fact]
        public void GetService_GuardClause()
        {
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.GetService<ISystemClock>(null));
        }

        [Fact]
        public void GetServices_GuardClause()
        {
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.GetServices<ISystemClock>(null));
        }

        [Fact]
        public void Replace_GuardClause()
        {
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Replace(null, this.GetType(), new object()));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Replace(new ServiceContainer(), null, new object()));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Replace(new ServiceContainer(), this.GetType(), (object)null));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Replace(null, this.GetType(), new object[0]));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Replace(new ServiceContainer(), null, new object[0]));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Replace(new ServiceContainer(), this.GetType(), null));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Replace<IFeatureFlipper>(null, new Mock<IFeatureFlipper>().Object));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Replace<IFeatureFlipper>(new ServiceContainer(), null));
            Assert.DoesNotThrow(() => ServiceProviderExtensions.Replace(new ServiceContainer(), new Mock<IFeatureFlipper>().Object));
        }

        [Fact]
        public void Replace()
        {
            // Arrange 
            var container = new ServiceContainer();
            var flipper = new Mock<IFeatureFlipper>();

            // Act & assert
            ServiceProviderExtensions.Replace(container, flipper.Object);

            // Assert
            var service = container.GetService<IFeatureFlipper>();
            Assert.Same(flipper.Object, service);
        }

        [Fact]
        public void Add_GuardClause()
        {
            // Arrange
            Mock<IFeatureStateParser> parser = new Mock<IFeatureStateParser>();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Add(null, this.GetType(), new object()));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Add(new ServiceContainer(), null, new object()));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Add(new ServiceContainer(), this.GetType(), null));
            Assert.DoesNotThrow(() => ServiceProviderExtensions.Add(new ServiceContainer(), typeof(IFeatureStateParser), parser.Object));
        }

        [Fact]
        public void Add()
        {   // Arrange
            Mock<IFeatureStateParser> parser = new Mock<IFeatureStateParser>();
            var container = new ServiceContainer();
            var beforeAdding = container.GetServices<IFeatureStateParser>().Count();

            // Act 
            ServiceProviderExtensions.Add(container, typeof(IFeatureStateParser), parser.Object);

            // Assert
            Assert.Equal(beforeAdding + 1, container.GetServices<IFeatureStateParser>().Count());
        }

        [Fact]
        public void AddOfTService_GuardClause()
        {
            // Arrange
            Mock<IFeatureStateParser> parser = new Mock<IFeatureStateParser>();
            Mock<IAssembliesResolver> resolver = new Mock<IAssembliesResolver>();
            
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Add<object>(null, new object()));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Add<object>(new ServiceContainer(), null));
            Assert.Throws<InvalidOperationException>(() => ServiceProviderExtensions.Add<IAssembliesResolver>(new ServiceContainer(), resolver.Object));
            Assert.DoesNotThrow(() => ServiceProviderExtensions.Add<IFeatureStateParser>(new ServiceContainer(), parser.Object));
        }

        [Fact]
        public void AddFeatureStateParser_GuardClause()
        {
            // Arrange
            Mock<IFeatureStateParser> parser = new Mock<IFeatureStateParser>();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.AddFeatureStateParser(null, parser.Object));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.AddFeatureStateParser(new ServiceContainer(), null));
        }

        [Fact]
        public void AddFeatureStateParser()
        {
            // Arrange
            Mock<IFeatureStateParser> parser = new Mock<IFeatureStateParser>();
            
            // Act & assert
            Assert.DoesNotThrow(() => ServiceProviderExtensions.AddFeatureStateParser(new ServiceContainer(), parser.Object));
        }

        [Fact]
        public void AddFeatureProvider_GuardClause()
        {
            // Arrange
            Mock<IFeatureProvider> provider = new Mock<IFeatureProvider>();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.AddFeatureProvider(null, provider.Object));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.AddFeatureProvider(new ServiceContainer(), null));
        }

        [Fact]
        public void AddFeatureProvider()
        {
            // Arrange
            Mock<IFeatureProvider> provider = new Mock<IFeatureProvider>();

            // Act & assert
            Assert.DoesNotThrow(() => ServiceProviderExtensions.AddFeatureProvider(new ServiceContainer(), provider.Object));
        }

        [Fact]
        public void RemoveAll()
        {
            // Arrange
            Mock<IFeatureProvider> provider = new Mock<IFeatureProvider>();

            // Act & assert
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.RemoveAll<object>(null));
            Assert.DoesNotThrow(() => ServiceProviderExtensions.RemoveAll<IFeatureStateParser>(new ServiceContainer()));
        }
    }
}
