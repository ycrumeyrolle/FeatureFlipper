namespace FeatureFlipper.Tests
{
    using System;
    using Moq;
    using Xunit;

    public class DateFeatureStateParserFixture
    {
        [Fact]
        public void TryParse_GuardClause()
        {
            // Arrange
            bool isOn;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => FeatureStateParserExtensions.TryParse(null, null, out isOn));
        }

        [Theory]
        [InlineData("2000-06-06T00:00:00+00:00", true, true)]
        [InlineData("2000-06-06T00:00:01+00:00", false, true)]
        [InlineData("1999-06-05T23:59:59+00:00", true, true)]
        [InlineData("X", false, false)]
        [InlineData(null, false, false)]
        public void TryParse(string value, bool expectedIsOn, bool expectedResult)
        {
            // Arange
            Mock<ISystemClock> clock = new Mock<ISystemClock>(MockBehavior.Strict);
            clock
                .Setup(c => c.UtcNow)
                .Returns(new DateTimeOffset(2000, 06, 06, 0, 0, 0, TimeSpan.Zero));
            var parser = new DateFeatureStateParser(clock.Object);
            bool isOn;

            // Act
            var result = parser.TryParse(value, out isOn);

            // Assert
            Assert.Equal(expectedIsOn, isOn);
            Assert.Equal(expectedResult, result);
        }
    }
}
