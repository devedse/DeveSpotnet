using DeveSpotnet.Models;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml.Linq;

namespace DeveSpotnet.SpotnetHelpers
{
    public static class SuperSpotnetHelper
    {

        /// <summary>
        /// Parses a raw header string (containing one or more header lines) and extracts SpotNet fields.
        /// </summary>
        /// <param name="headerContent">The full header text (with line breaks).</param>
        /// <returns>A SpotHeader object populated with the parsed data.</returns>
        public static SpotHeader ParseHeader(string headerContent)
        {
            var result = new SpotHeader();

            // Split the header content into lines (handling CRLF and LF)
            var lines = headerContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                // Each header is expected to be in the format "Key: Value"
                int colonIndex = line.IndexOf(':');
                if (colonIndex < 0)
                    continue;

                string key = line.Substring(0, colonIndex).Trim().ToLowerInvariant();
                string value = line.Substring(colonIndex + 1).Trim();

                switch (key)
                {
                    case "from":
                        // Extract the portion before the first '<'
                        int ltIndex = value.IndexOf('<');
                        if (ltIndex > 0)
                        {
                            // In PHP, they use mb_convert_encoding from ISO-8859-1 to UTF-8.
                            // Here we assume the string is in ISO-8859-1 and convert to UTF-8.
                            string fromPart = value.Substring(0, ltIndex).Trim();
                            // For demonstration, we'll assume it's already UTF-8.
                            result.FromHdr = fromPart;
                        }
                        else
                        {
                            result.FromHdr = value;
                        }
                        break;

                    case "date":
                        // Parse the date string and convert it to a Unix timestamp.
                        if (DateTime.TryParse(value, out DateTime dt))
                        {
                            result.Stamp = new DateTimeOffset(dt).ToUnixTimeSeconds();
                        }
                        break;

                    case "x-xml":
                        // Concatenate the remainder of the line (after "X-XML:") to FullXml.
                        result.FullXml += value;
                        break;

                    case "x-user-signature":
                        result.UserSignature = SpotParseUtil.SpotUnprepareBase64(value);
                        break;

                    case "x-xml-signature":
                        result.XmlSignature = SpotParseUtil.SpotUnprepareBase64(value);
                        break;

                    case "x-newsreader":
                        result.Newsreader = value;
                        break;

                    case "x-user-avatar":
                        // Concatenate if multiple lines exist.
                        result.UserAvatar += value;
                        break;

                    case "x-user-key":
                        // The value is expected to be XML. Try to parse it.
                        try
                        {
                            var xml = XElement.Parse(value);
                            var exponent = xml.Element("Exponent")?.Value;
                            var modulus = xml.Element("Modulus")?.Value;
                            result.UserKey.Exponent = exponent ?? string.Empty;
                            result.UserKey.Modulo = modulus ?? string.Empty;
                        }
                        catch
                        {
                            // Ignore any XML parsing errors.
                        }
                        break;
                }
            }

            // After processing headers, if FullXml and Newsreader are present, try to insert the newsreader info into the XML.
            if (!string.IsNullOrWhiteSpace(result.FullXml) && !string.IsNullOrWhiteSpace(result.Newsreader))
            {
                try
                {
                    var xmlDoc = XElement.Parse(result.FullXml);
                    // Add an Extra element with a Newsreader child.
                    var extra = new XElement("Extra", new XElement("Newsreader", result.Newsreader));
                    xmlDoc.Add(extra);
                    result.FullXml = xmlDoc.ToString();
                }
                catch
                {
                    // If XML parsing fails, try to fix the XML by inserting a space after a single quote followed by a letter.
                    result.FullXml = Regex.Replace(result.FullXml, @"'([A-Za-z])", "' $1");
                    try
                    {
                        var xmlDoc = XElement.Parse(result.FullXml);
                        var extra = new XElement("Extra", new XElement("Newsreader", result.Newsreader));
                        xmlDoc.Add(extra);
                        result.FullXml = xmlDoc.ToString();
                    }
                    catch
                    {
                        // If it still fails, leave FullXml as is.
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Decodes a Base64-encoded string. Logs a warning and returns an empty string on failure.
        /// </summary>
        public static string DecodeBase64(string encodedString)
        {
            try
            {
                byte[] data = Convert.FromBase64String(encodedString);
                return Encoding.UTF8.GetString(data);
            }
            catch (FormatException)
            {
                return string.Empty;
            }
        }
    }

    public class SpotHeader
    {
        /// <summary>
        /// The “From” header’s human-readable part.
        /// </summary>
        public string FromHdr { get; set; } = string.Empty;

        /// <summary>
        /// The UNIX timestamp parsed from the Date header.
        /// </summary>
        public long Stamp { get; set; }

        /// <summary>
        /// The concatenated XML string from the X-XML header(s).
        /// </summary>
        public string FullXml { get; set; } = string.Empty;

        /// <summary>
        /// The user signature (decoded via our utility).
        /// </summary>
        public string UserSignature { get; set; } = string.Empty;

        /// <summary>
        /// The XML signature (decoded via our utility).
        /// </summary>
        public string XmlSignature { get; set; } = string.Empty;

        /// <summary>
        /// The newsreader information.
        /// </summary>
        public string Newsreader { get; set; } = string.Empty;

        /// <summary>
        /// The concatenated user avatar data.
        /// </summary>
        public string UserAvatar { get; set; } = string.Empty;

        /// <summary>
        /// The user key (with exponent and modulus).
        /// </summary>
        public UserKey UserKey { get; set; } = new UserKey();
    }

    public class UserKey
    {
        public string Exponent { get; set; } = string.Empty;
        public string Modulo { get; set; } = string.Empty;
    }

    public static class SpotParseUtil
    {
        /// <summary>
        /// This method “unprepares” a Base64 string as needed by the SpotNet protocol.
        /// For now it simply trims the input; extend it if additional processing is required.
        /// </summary>
        public static string SpotUnprepareBase64(string input)
        {
            return input.Trim();
        }
    }
}
