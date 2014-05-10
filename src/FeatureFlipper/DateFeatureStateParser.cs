namespace FeatureFlipper
{
    using System;
    using System.Globalization;

    public class DateFeatureStateParser : IFeatureStateParser
    {
        private readonly ISystemClock systemClock;

        public DateFeatureStateParser(ISystemClock systemClock)
        {
            this.systemClock = systemClock;
        }

        public DateFeatureStateParser()
            : this(new SystemClock())
        {
        }

        public bool TryParse(string value, out bool isOn)
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
