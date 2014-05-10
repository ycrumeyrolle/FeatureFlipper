﻿namespace FeatureFlipper.Tests
{
    using System.Threading;
    using Xunit;

    public class DefaultPrincipalProviderFixture
    {
        [Fact]
        public void GetValue()
        {
            // Arange
            var reader = new DefaultPrincipalProvider();

            // Act
            var principal = reader.Principal;

            // Assert
            Assert.Equal(Thread.CurrentPrincipal, principal);
        }
    }
}
