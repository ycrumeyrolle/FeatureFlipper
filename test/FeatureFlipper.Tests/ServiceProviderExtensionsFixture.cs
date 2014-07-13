namespace FeatureFlipper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using Xunit;

    public class ServiceProviderExtensionsFixture
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
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Replace(null, this.GetType(), (object)null));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Replace(new ServiceContainer(), this.GetType(), (object)null));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Replace(null, this.GetType(), (IEnumerable<object>)null));
            Assert.Throws<ArgumentNullException>(() => ServiceProviderExtensions.Replace(new ServiceContainer(), this.GetType(), (IEnumerable<object>)null));
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
            ServiceProviderExtensions.Add(container, typeof(IFeatureStateParser), new BooleanFeatureStateParser());

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
        public void AddFeatureStateParser()
        {
            // Arrange
            Mock<IFeatureStateParser> parser = new Mock<IFeatureStateParser>();
            
            // Act & assert
            Assert.DoesNotThrow(() => ServiceProviderExtensions.AddFeatureStateParser(new ServiceContainer(), parser.Object));
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
