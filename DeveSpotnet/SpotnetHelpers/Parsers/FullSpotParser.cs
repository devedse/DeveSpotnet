using DeveSpotnet.Models;
using SpotnetClient.SpotnetHelpers;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DeveSpotnet.SpotnetHelpers.Parsers
{
    public static class FullSpotParser
    {
        /// <summary>Wrap the content of certain XML elements in <![CDATA[ … ]]></summary>
        private static string CorrectElmContents(string xml, IEnumerable<string> elems)
        {
            const string cdataStart = "<![CDATA[";
            const string cdataEnd = "]]>";

            /* remove low-ASCII characters (0x00 – 0x1F)                         */
            xml = Regex.Replace(xml, @"[\x00-\x1F]", string.Empty);

            foreach (string elem in elems)
            {
                int start = xml.IndexOf($"<{elem}>", StringComparison.OrdinalIgnoreCase);
                int end = xml.IndexOf($"</{elem}>", StringComparison.OrdinalIgnoreCase);
                if (start < 0 || end < 0) continue;

                /* Is CDATA already present?                                     */
                int contentStart = start + elem.Length + 2;          // <Title>…
                if (!xml.AsSpan(contentStart, cdataStart.Length).SequenceEqual(cdataStart))
                {
                    xml = xml.Replace($"<{elem}>", $"<{elem}>{cdataStart}")
                             .Replace($"</{elem}>", $"{cdataEnd}</{elem}>");
                }
            }
            return xml;
        }

        /// <summary>Strip 4-byte UTF-8 code points (MySQL «utf8mb3» safe).</summary>
        private static string Replace4Byte(string input, string replacement = "")
        {
            const string pattern =
                @"(?:
                    \xF0[\x90-\xBF][\x80-\xBF]{2}      # planes 1-3
                  | [\xF1-\xF3][\x80-\xBF]{3}          # planes 4-15
                  | \xF4[\x80-\x8F][\x80-\xBF]{2}      # plane 16
                )";
            return Regex.Replace(input, pattern, replacement,
                                 RegexOptions.IgnorePatternWhitespace);
        }

        /* ───────────────────────── public API ───────────────────────── */

        public static ParsedFullSpot ParseFull(string xmlStr)
        {
            var spot = new ParsedFullSpot();   // fills with default values

            /* 1 – fix legacy “invalid multiple segment” bug (issue #1608)       */
            if (xmlStr.Contains("spot.net></Segment", StringComparison.Ordinal))
            {
                xmlStr = xmlStr
                    .Replace("spot.net></Segment>", "spot.net</Segment>")
                    .Replace("spot.ne</Segment>", "spot.net</Segment>");
            }

            /* 2 – ensure CDATA where some clients forgot it                     */
            xmlStr = CorrectElmContents(xmlStr,
                         new[] { "Title", "Description", "Image", "Tag", "Website" });

            /* 3 – try to load XML (suppress errors like PHP’s @ operator)       */
            XDocument? top;
            try
            {
                top = XDocument.Parse(xmlStr, LoadOptions.PreserveWhitespace);
            }
            catch
            {
                top = null;
            }
            if (top?.Root is null || top.Root.Name != "Spotnet") return spot;

            XElement posting = top.Root.Element("Posting") ?? new XElement("Posting");

            /* 4 – straightforward scalar fields                                */
            spot.Created = (string)posting.Element("Created") ?? string.Empty;
            spot.Key = (int?)posting.Element("Key") ?? 0;

            string category = posting.Element("Category")?.Nodes()?.OfType<XText>()?.FirstOrDefault()?.Value ?? string.Empty;
            if (int.TryParse(category, out int cat))
            {
                spot.Category = cat;
            }
            spot.Website = (string)posting.Element("Website") ?? string.Empty;
            spot.Description = (string)posting.Element("Description") ?? string.Empty;
            spot.FileSize = (long?)posting.Element("Size") ?? 0;
            spot.Poster = EncodingHelper.IsoToUtf8(posting.Element("Poster"));
            spot.Tag = EncodingHelper.IsoToUtf8(posting.Element("Tag"));
            spot.Title = (string)posting.Element("Title") ?? string.Empty;

            /* HTML-decode & 4-byte sanitize title/description                  */
            spot.Title = Replace4Byte(WebUtility.HtmlDecode(spot.Title), "??");
            spot.Description = Replace4Byte(WebUtility.HtmlDecode(spot.Description), "??");

            /* Optional FTD extras                                              */
            if (posting.Element("Filename") is { } fn) spot.Filename = (string)fn;
            if (posting.Element("Newsgroup") is { } ng) spot.NewsGroup = (string)ng;

            /* 5 – IMAGE handling                                               */
            XElement? image = posting.Element("Image");
            if (image is not null && !image.Elements("Segment").Any())
            {
                spot.ImageUrl = (string)image;
            }
            else if (image is not null)
            {
                spot.ImageInfo = new ParsedFullSpot.ImageMeta
                {
                    Height = (int?)image.Attribute("Height") ?? 0,
                    Width = (int?)image.Attribute("Width") ?? 0,
                    Segments = image.Elements("Segment")
                                    .Select(s => (string)s)
                                    .Where(SpotUtil.ValidMessageId)
                                    .ToList()
                };
            }

            /* 6 – NZB segments                                                 */
            spot.NzbSegments = posting
                .Descendants("NZB").Elements("Segment")
                .Select(s => (string)s)
                .Where(SpotUtil.ValidMessageId)
                .ToList();

            /* 7 – PREVSPOTS                                                   */
            spot.PrevMsgIds = posting
                .Descendants("PREVSPOTS").Elements("Spot")
                .Select(s => (string)s)
                .Where(SpotUtil.ValidMessageId)
                .ToList();

            /* 8 – Extra / Newsreader                                           */
            spot.Newsreader = (string)top.Root.Element("Extra")?.Element("Newsreader") ?? string.Empty;

            /* 9 – Category offset for new keys                                 */
            if (spot.Key != 1)
            {
                spot.Category--;
            }

            /* 10 – SubCats                                                     */
            var subCatStrings = posting.Element("Category")?
                                         .Elements("SubCat").Select(s => (string)s).ToList()
                              ?? posting.Element("Category")?
                                         .Elements("Sub").Select(s => (string)s).ToList()
                              ?? new List<string>();

            foreach (string sub in subCatStrings)
            {
                Match m = Regex.Match(sub, @"(\d+)([aAbBcCdDzZ])(\d+)");
                if (!m.Success) continue;

                string letter = m.Groups[2].Value.ToLowerInvariant();  // a-d,z
                int number = int.Parse(m.Groups[3].Value);
                string token = $"{letter}{number}|";

                switch (letter)
                {
                    case "a": spot.SubCatA += token; break;
                    case "b": spot.SubCatB += token; break;
                    case "c": spot.SubCatC += token; break;
                    case "d": spot.SubCatD += token; break;
                    case "z": spot.SubCatZ += token; break;
                }
            }

            /* 11 – Guarantee subcatz / map legacy sub-cats                     */
            if (string.IsNullOrEmpty(spot.SubCatZ))
            {
                spot.SubCatZ = SpotCategories.createSubcatZ(
                                   spot.Category,
                                   spot.SubCatA + spot.SubCatB + spot.SubCatD);
            }
            spot.SubCatD = SpotCategories.mapDeprecatedGenreSubCategories(
                               spot.Category, spot.SubCatD, spot.SubCatZ);
            spot.SubCatC = SpotCategories.mapLanguageSubCategories(
                               spot.Category, spot.SubCatC, spot.SubCatZ);

            return spot;
        }


    }
}
