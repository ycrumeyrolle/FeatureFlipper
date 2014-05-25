namespace FeatureFlipper.Tests
{
    using System;
    using Xunit;

    public class SystemClockFixture
    {
        [Fact]
        public void UtcNow()
        {
            // Arrange 
            SystemClock clock = new SystemClock();

            // Act
            var expectedNow = DateTimeOffset.UtcNow;
            var utcNow = clock.UtcNow;

            // Pseudo-assert
            Assert.InRange(utcNow, expectedNow.AddSeconds(-1), expectedNow.AddSeconds(1));
        }
    }
}
