namespace FeatureFlipper.Tests
{
    using Xunit;

    public class BooleanFeatureStateParserFixture
    {
        [Theory]
        [InlineData("true", true, true)]
        [InlineData("false", false, true)]
        [InlineData("TRUE", true, true)]
        [InlineData("FALSE", false, true)]
        [InlineData("X", false, false)]
        [InlineData(null, false, false)]
        public void TryParse(string value, bool expectedIsOn, bool expectedResult)
        {
            // Arrange
            var parser = new BooleanFeatureStateParser();
            bool isOn;

            // Act
            var result = parser.TryParse(value, null, out isOn);

            // Assert
            Assert.Equal(expectedIsOn, isOn);
            Assert.Equal(expectedResult, result);
        }
    }
}
