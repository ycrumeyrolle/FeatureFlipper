namespace FeatureFlipper
{
    using System;

    /// <summary>
    /// Provides access to the normal system clock.
    /// </summary>
    public sealed class SystemClock : ISystemClock
    {
        /// <summary>
        /// Retrieves the current system time in UTC.
        /// </summary>
        public DateTimeOffset UtcNow
        {
            get
            {
                return DateTimeOffset.UtcNow;
            }
        }
    }
}
