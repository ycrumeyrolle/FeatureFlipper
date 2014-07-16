namespace FeatureFlipper.Tests
{
    using Xunit;

    [Trait("Category", "Parser")]
    public class VersionStateParserTests
    {
        [Theory]
        [InlineData("V1", "V1", true, true)]
        [InlineData("V1", "V2", true, false)]
        [InlineData("V1", "v1", true, false)]
        [InlineData("v1", "V1", true, false)]
        [InlineData("V1", null, false, false)]
        public void TryParse(string version, string expectedVersion, bool expectedResult, bool expectedIsOn)
        {
            // Arrange
            var parser = new VersionStateParser();
            bool isOn;

            // Act
            var result = parser.TryParse(version, expectedVersion, out isOn);

            // Assert
            Assert.Equal(expectedResult, result);
            Assert.Equal(expectedIsOn, isOn);
        }
    }
}
