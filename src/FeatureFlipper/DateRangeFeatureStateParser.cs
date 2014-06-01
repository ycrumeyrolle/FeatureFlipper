namespace FeatureFlipper
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Represents a parser of date range. A date range is a string like "Date1,Date2".
    /// </summary>
    public class DateRangeFeatureStateParser : IFeatureStateParser
    {
        private static readonly char[] separators = new[] { ',' };

        private static readonly  Func<DateTimeOffset, DateTimeOffset, bool> includeStartPredicate = (startDate, now) => startDate <= now;
        private static readonly Func<DateTimeOffset, DateTimeOffset, bool> excludeStartPredicate = (startDate, now) => startDate < now;
        private static readonly Func<DateTimeOffset, DateTimeOffset, bool> includeEndPredicate = (endDate, now) => now <= endDate;
        private static readonly Func<DateTimeOffset, DateTimeOffset, bool> excludeEndPredicate = (endDate, now) => now < endDate;

        private readonly ISystemClock systemClock;

        public DateRangeFeatureStateParser(ISystemClock systemClock)
        {
            if (systemClock == null)
            {
                throw new ArgumentNullException("systemClock");
            }

            this.systemClock = systemClock;
        }

        public bool TryParse(string value, string version, out bool isOn)
        {
            if (value == null)
            {
                isOn = false;
                return false;
            }

            Func<DateTimeOffset, DateTimeOffset, bool> startPredicate;
            Func<DateTimeOffset, DateTimeOffset, bool> endPredicate;

            char startChar = value[0];
            if (startChar == '[' || startChar == '(')
            {
                startPredicate = excludeStartPredicate;
                value = value.Substring(1, value.Length - 1);
            }
            else
            {
                startPredicate = includeStartPredicate;
                if (startChar == ']')
                {
                    value = value.Substring(1, value.Length - 1);
                }
            }

            char endChar = value[value.Length - 1];
            if (endChar == ']' || endChar == ')')
            {
                endPredicate = excludeEndPredicate;
                value = value.Substring(0, value.Length - 1);
            }
            else
            {
                endPredicate = includeEndPredicate;
                if (endChar == '[')
                {
                    value = value.Substring(0, value.Length - 1);
                }
            }

            var ranges = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            if (ranges.Length != 2)
            {
                isOn = false;
                return false;
            }

            DateTimeOffset startDate;
            DateTimeOffset endDate;
            if (!DateTimeOffset.TryParse(ranges[0], out startDate) || !DateTimeOffset.TryParse(ranges[1], out endDate))
            {
                isOn = false;
                return false;
            }

            var now = this.systemClock.UtcNow;
            isOn = startPredicate(startDate, now) && endPredicate(endDate, now);
            return true;
        }
    }
}
