using System.Globalization;
using System.Text.RegularExpressions;

namespace DeveSpotnet.SpotnetHelpers
{
    public static class PHPDateParser
    {
        public static bool TryParseNntpDate(string dateString, out DateTimeOffset dateTime)
        {
            // First, try direct parsing (handles many standard formats)
            if (DateTimeOffset.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateTime))
            {
                return true;
            }

            // List of possible NNTP date formats
            string[] formats = new string[]
            {
                "ddd, dd MMM yy HH:mm:ss UTC",      // Sun, 10 Mar 19 20:11:41 UTC
                "ddd, dd MMM yyyy HH:mm:ss zzz",   // Sun, 10 Mar 2019 20:11:41 +0000
                "ddd, dd MMM yyyy HH:mm:ss 'GMT'", // Sun, 10 Mar 2019 20:11:41 GMT
                "ddd, dd MMM yy HH:mm:ss zzz",     // Sun, 10 Mar 19 20:11:41 +0000
                "ddd, dd MMM yyyy HH:mm:ss",       // Sun, 10 Mar 2019 20:11:41
                "dd MMM yyyy HH:mm:ss zzz",        // 10 Mar 2019 20:11:41 +0000
                "ddd, dd-MMM-yyyy HH:mm:ss zzz"    // Sun, 10-Mar-2019 20:11:41 +0000
            };

            // Try each format explicitly
            foreach (var format in formats)
            {
                if (DateTimeOffset.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateTime))
                {
                    return true;
                }
            }

            // Remove parenthesized time zone names (e.g., "(PDT)", "(UTC)")
            string cleanedDate = Regex.Replace(dateString, @"\s\([A-Z]+\)$", "").Trim();
            if (cleanedDate != dateString)
            {
                return TryParseNntpDate(cleanedDate, out dateTime);
            }

            // If all else fails, return false
            dateTime = default;
            return false;
        }
    }
}
