namespace FeatureFlipper.Tests
{
    using System;
    using Moq;
    using Xunit;

    public class DateRangeFeatureStateParserFixture
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new DateRangeFeatureStateParser(null));
        }

        [Theory]
        [InlineData("2000-06-06T00:00:00+00:00,2000-06-07T00:00:00+00:00", true, true)]
        [InlineData("[2000-06-06T00:00:00+00:00,2000-06-07T00:00:00+00:00]", false, true)]
        [InlineData("]2000-06-06T00:00:00+00:00,2000-06-07T00:00:00+00:00]", true, true)]
        [InlineData("(2000-06-06T00:00:00+00:00,2000-06-07T00:00:00+00:00]", true, true)]
        [InlineData("[2000-06-06T00:00:00+00:00,2000-06-07T00:00:00+00:00[", false, true)]
        [InlineData("[2000-06-06T00:00:00+00:00,2000-06-07T00:00:00+00:00)", false, true)]
        [InlineData("(2000-06-06T00:00:00+00:00,2000-06-07T00:00:00+00:00)", true, true)]
        [InlineData("2000-06-05T00:00:00+00:00,2000-06-06T00:00:00+00:00", true, true)]
        [InlineData("[2000-06-05T00:00:00+00:00,2000-06-06T00:00:00+00:00]", false, true)]
        [InlineData("]2000-06-05T00:00:00+00:00,2000-06-06T00:00:00+00:00]", false, true)]
        [InlineData("(2000-06-05T00:00:00+00:00,2000-06-06T00:00:00+00:00]", false, true)]
        [InlineData("[2000-06-05T00:00:00+00:00,2000-06-06T00:00:00+00:00[", true, true)]
        [InlineData("[2000-06-05T00:00:00+00:00,2000-06-06T00:00:00+00:00)", true, true)]
        [InlineData("]2000-06-05T00:00:00+00:00,2000-06-06T00:00:00+00:00[", true, true)]
        [InlineData("(2000-06-05T00:00:00+00:00,2000-06-06T00:00:00+00:00)", true, true)]
        [InlineData("2000-06-05T00:00:00+00:00,2000-06-07T00:00:00+00:00", true, true)]
        [InlineData("[2000-06-05T00:00:00+00:00,2000-06-07T00:00:00+00:00]", true, true)]
        [InlineData("]2000-06-05T00:00:00+00:00,2000-06-07T00:00:00+00:00]", true, true)]
        [InlineData("(2000-06-05T00:00:00+00:00,2000-06-07T00:00:00+00:00]", true, true)]
        [InlineData("[2000-06-05T00:00:00+00:00,2000-06-07T00:00:00+00:00[", true, true)]
        [InlineData("[2000-06-05T00:00:00+00:00,2000-06-07T00:00:00+00:00)", true, true)]
        [InlineData("]2000-06-05T00:00:00+00:00,2000-06-07T00:00:00+00:00[", true, true)]
        [InlineData("(2000-06-05T00:00:00+00:00,2000-06-07T00:00:00+00:00)", true, true)]
        [InlineData("X", false, false)]
        [InlineData("2000-06-06T00:00:00+00:00,X", false, false)]
        [InlineData("X,2000-06-06T00:00:00+00:00", false, false)]
        [InlineData("[2000-06-06T00:00:00+00:00,X]", false, false)]
        [InlineData("[X,2000-06-06T00:00:00+00:00]", false, false)]
        [InlineData(null, false, false)]
        public void TryParse(string value, bool expectedIsOn, bool expectedResult)
        {
            // Arange
            Mock<ISystemClock> clock = new Mock<ISystemClock>(MockBehavior.Strict);
            clock
                .Setup(c => c.UtcNow)
                .Returns(new DateTimeOffset(2000, 06, 06, 0, 0, 0, TimeSpan.Zero));
            var parser = new DateRangeFeatureStateParser(clock.Object);
            bool isOn;

            // Act
            var result = parser.TryParse(value, null, out isOn);

            // Assert
            Assert.Equal(expectedIsOn, isOn);
            Assert.Equal(expectedResult, result);
        }
    }
}
