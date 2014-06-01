namespace FeatureFlipper
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Represents a parser of date range. A date range is a string like "Date1,Date2".
    /// Valid ranges are defined by the ISO 31-11 standard : 
    /// <list type="square">
    ///     <item>(date1, date2)</item>
    ///     <item>[date1, date2]</item>
    ///     <item>]date1, date2[</item>
    ///     <item>[date1, date2[</item>
    ///     <item>]date1, date2]</item>
    /// </list>
    /// </summary>
    public class DateRangeFeatureStateParser : IFeatureStateParser
    {
        private static readonly char[] Separators = new[] { ',' };

        private static readonly Func<DateTimeOffset, DateTimeOffset, bool> IncludeStartPredicate = (startDate, now) => startDate <= now;
        private static readonly Func<DateTimeOffset, DateTimeOffset, bool> ExcludeStartPredicate = (startDate, now) => startDate < now;
        private static readonly Func<DateTimeOffset, DateTimeOffset, bool> IncludeEndPredicate = (endDate, now) => now <= endDate;
        private static readonly Func<DateTimeOffset, DateTimeOffset, bool> ExcludeEndPredicate = (endDate, now) => now < endDate;

        private readonly ISystemClock systemClock;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeFeatureStateParser"/> class.
        /// </summary>
        public DateRangeFeatureStateParser(ISystemClock systemClock)
        {
            if (systemClock == null)
            {
                throw new ArgumentNullException("systemClock");
            }

            this.systemClock = systemClock;
        }

        /// <summary>
        /// Tries to parse the value of the feature.
        /// </summary>
        /// <param name="value">The value of the feature.</param>
        /// <param name="version">Optionnal. The version of the feature.</param>
        /// <param name="isOn">
        ///  When this method returns, if the feature is parsed as a date range, contains true if the feature is <c>On</c>
        ///  or false if the feature is <c>Off</c>.
        ///  If the feature is not parsed, contains false. 
        /// </param>
        /// <returns><c>true</c> if the feature is parsed; otherwise, <c>false</c>.</returns>   
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
            if (startChar == '[')
            {
                startPredicate = ExcludeStartPredicate;
                value = value.Substring(1, value.Length - 1);
            }
            else
            {
                startPredicate = IncludeStartPredicate;
                if (startChar == ']' || startChar == '(')
                {
                    value = value.Substring(1, value.Length - 1);
                }
            }

            char endChar = value[value.Length - 1];
            if (endChar == ']')
            {
                endPredicate = ExcludeEndPredicate;
                value = value.Substring(0, value.Length - 1);
            }
            else
            {
                endPredicate = IncludeEndPredicate;
                if (endChar == '[' || endChar == ')')
                {
                    value = value.Substring(0, value.Length - 1);
                }
            }

            var ranges = value.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
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
