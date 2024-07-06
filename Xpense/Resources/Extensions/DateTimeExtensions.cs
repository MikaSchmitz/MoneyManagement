using System.Diagnostics;
using System.Globalization;
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

        public static int DaysRemainingInWeek(this DateTime date)
        {
            // Assuming the week starts on Sunday
            DayOfWeek firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            int daysRemaining = ((int)firstDayOfWeek + 7 - (int)date.DayOfWeek) % 7;
            return daysRemaining == 0 ? 7 : daysRemaining;
        }

        public static int DaysRemainingInMonth(this DateTime date)
        {
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            return daysInMonth - date.Day;
        }

        public static int DaysRemainingInYear(this DateTime date)
        {
            DateTime lastDayOfYear = new DateTime(date.Year, 12, 31);
            return (lastDayOfYear - date).Days;
        }
    }
}
