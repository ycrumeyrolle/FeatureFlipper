namespace FeatureFlipper.Tests
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using Xunit;

    public class DefaultAssembliesResolverFixture
    {
        [Fact]
        public void GetAssemblies_ReturnsAssemblies()
        {
            // Arrange
            DefaultAssembliesResolver resolver = new DefaultAssembliesResolver();

            // Act
            var result = resolver.GetAssemblies();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }
    }
}
