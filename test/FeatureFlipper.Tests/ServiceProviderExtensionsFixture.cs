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
    }
}
