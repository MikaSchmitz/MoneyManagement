using System.Diagnostics;
using Xpense.Resources.Enums;

namespace Xpense.Resources.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime AddTimeInterval(this DateTime dateTime, TimeInterval timeInterval, int multiplier = 1)
        {
            if (multiplier < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(multiplier));
            }

            switch (timeInterval) 
            {
                case TimeInterval.Daily:
                    return dateTime.AddDays(multiplier);

                case TimeInterval.Weekly:
                    var weeks = 7 * multiplier;
                    return dateTime.AddDays(weeks);

                case TimeInterval.Monthly:
                    return dateTime.AddMonths(multiplier);

                case TimeInterval.Yearly:
                    return dateTime.AddYears(multiplier);

                default:
                    Debug.WriteLine($"Could not find `{timeInterval.ToString()}`.");
                    return dateTime;
            };
        }
    }
}
