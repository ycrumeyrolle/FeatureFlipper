namespace FeatureFlipper.Tests
{
    using Xunit;

    public class DefaultConfigurationReaderTests
    {
        [Fact]
        public void GetValue()
        {
            // Arrange
            var reader = new DefaultConfigurationReader();

            // Act
            var result1 = reader.GetValue("key");
            var result2 = reader.GetValue("key");

            // Assert
            Assert.Equal(result1, result2);
        }
    }
}
