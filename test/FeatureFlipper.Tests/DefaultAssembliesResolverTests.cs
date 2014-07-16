namespace FeatureFlipper.Tests
{
    using Xunit;

    public class DefaultAssembliesResolverTests
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
