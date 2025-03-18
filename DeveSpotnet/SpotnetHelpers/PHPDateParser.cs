using System.Globalization;
using System.Text.RegularExpressions;

namespace DeveSpotnet.SpotnetHelpers
{
    public static partial class PHPDateParser
    {
        private static readonly Dictionary<string, string> TzMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "UTC", "+0000" },
            { "GMT", "+0000" },
            { "BST", "+0100" },
            { "IST", "+0100" },  // (Irish Standard Time, for example)
            { "WET", "+0000" },
            { "WEST", "+0100" },
            { "CET", "+0100" },
            { "CEST", "+0200" },
            { "EET", "+0200" },
            { "EEST", "+0300" },
            { "MSK", "+0300" },
            { "MSD", "+0400" },
            { "AST", "-0400" },
            { "ADT", "-0300" },
            { "NST", "-0330" },
            { "NDT", "-0230" },
            { "EST", "-0500" },
            { "EDT", "-0400" },
            { "CST", "-0600" },
            { "CDT", "-0500" },
            { "MST", "-0700" },
            { "MDT", "-0600" },
            { "PST", "-0800" },
            { "PDT", "-0700" },
            { "HST", "-1000" },
            { "AKST", "-0900" },
            { "AKDT", "-0800" },
            { "AEST", "+1000" },
            { "AEDT", "+1100" },
            { "ACST", "+0930" },
            { "ACDT", "+1030" },
            { "AWST", "+0800" }
        };

        // Build the time zone mapping pattern from the keys.
        private static readonly string TzMapPattern = @"\b(" + string.Join("|", TzMap.Keys) + @")\b";

        // Precompiled Regex to check for a numeric offset (e.g. "+0000" or "+00:00").
        [GeneratedRegex(@"[\+\-]\d{2}:?\d{2}")]
        private static partial Regex NumericOffsetRegex_Generated();

        // Precompiled Regex to remove any trailing parenthesized zone text (e.g. " (PDT)").
        [GeneratedRegex(@"\s*\([A-Za-z]+\)$")]
        private static partial Regex TrailingParenthesesRegex_Generated();

        // Precompiled Regex for matching any known time zone abbreviations.
        private static readonly Regex TzMapRegex = new Regex(TzMapPattern, RegexOptions.Compiled);

        // Array of parseable date formats.
        private static readonly string[] ParsableFormats =
        [
            "dd MMM yy HH:mm zzz",             // e.g. "20 Jul 16 10:47 +0200"
            "dd MMM yy HH:mm:ss zzz",          // e.g. "18 Sep 16 22:44:00 +0200"
            "ddd, dd MMM yy HH:mm zzz",        // e.g. "Mon, 26 Feb 18 12:45 +0100"
            "ddd, dd MMM yy HH:mm:ss zzz",
            "dd MMM yyyy HH:mm zzz",           // e.g. with four-digit year
            "dd MMM yyyy HH:mm:ss zzz",
            "ddd, dd MMM yyyy HH:mm zzz",
            "ddd, dd MMM yyyy HH:mm:ss zzz"
        ];

        /// <summary>
        /// Tries to parse various NNTP/Usenet-style date strings.
        /// It first checks if a numeric offset is already present.
        ///   - If a numeric offset is found, it removes any trailing parentheses.
        ///   - Otherwise, it replaces known time zone abbreviations with their numeric offsets.
        /// Then it attempts several parse patterns.
        /// </summary>
        /// <param name="dateString">The date string to parse.</param>
        /// <param name="dateTime">The parsed DateTimeOffset if successful.</param>
        /// <returns>True if parsing succeeded; otherwise false.</returns>
        public static bool TryParseNntpDate(string dateString, out DateTimeOffset dateTime)
        {
            string dateStringProcessing = dateString;

            if (string.IsNullOrWhiteSpace(dateStringProcessing))
            {
                dateTime = default;
                return false;
            }

            // 1. If a numeric offset (e.g. "+0000" or "+00:00") exists, remove any trailing parenthesized zone text.
            if (NumericOffsetRegex_Generated().IsMatch(dateStringProcessing))
            {
                dateStringProcessing = TrailingParenthesesRegex_Generated().Replace(dateStringProcessing, "").Trim();
            }
            else
            {
                // 2. Replace known time zone abbreviations with their numeric offsets.
                dateStringProcessing = TzMapRegex.Replace(dateStringProcessing, m =>
                {
                    if (TzMap.TryGetValue(m.Value, out string offset))
                    {
                        return offset;
                    }
                    return m.Value;
                });
            }

            // 3. First, try the built-in parsing.
            if (DateTimeOffset.TryParse(dateStringProcessing, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateTime))
            {
                return true;
            }

            // 4. Try parsing using each explicit format.
            foreach (var format in ParsableFormats)
            {
                if (DateTimeOffset.TryParseExact(dateStringProcessing, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out dateTime))
                {
                    return true;
                }
            }

            dateTime = default;
            return false;
        }
    }
}
