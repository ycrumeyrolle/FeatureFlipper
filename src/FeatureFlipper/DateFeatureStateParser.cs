namespace FeatureFlipper
{
    using System;
    using System.Globalization;

    /// <summary>
    /// This implementation of <see cref="IFeatureStateParser"/> tries to parse a value into date 
    /// and compares it with the system clock.
    /// </summary>
    public sealed class DateFeatureStateParser : IFeatureStateParser
    {
        private readonly ISystemClock systemClock;
    
        /// <summary>
        /// Initializes a new instance of the <see cref="DateFeatureStateParser"/> class.
        /// </summary>
        public DateFeatureStateParser(ISystemClock systemClock)
        {
            this.systemClock = systemClock;
        }

        /// <summary>
        /// Tries to parse the value of the feature. It must be a valid representation of a <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="value">The value of the feature.</param>
        /// <param name="isOn">
        ///  When this method returns, if the feature is parsed, contains true if the feature is <c>On</c>
        ///  or false if the feature is <c>Off</c>.
        ///  If the feature is not parsed, contains false. 
        /// </param>
        /// <returns><c>true</c> if the feature is parsed; otherwise, <c>false</c>.</returns>   
        public bool TryParse(string value, string version, out bool isOn)
        {
            DateTimeOffset date;
            if (DateTimeOffset.TryParse(value, out date))
            {
                isOn = date <= this.systemClock.UtcNow;
                return true;
            }

            isOn = false;
            return false;
        }
    }
}
