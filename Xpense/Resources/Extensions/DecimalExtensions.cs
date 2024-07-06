using System.Globalization;

namespace Xpense.Resources.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToEuroString(this decimal value)
        {
            // Define the culture info for Euro formatting
            var cultureInfo = new CultureInfo("nl-NL");

            // Format the decimal value as a string in Euro format
            return "€ " + value.ToString("N2", cultureInfo);
        }
    }
}
