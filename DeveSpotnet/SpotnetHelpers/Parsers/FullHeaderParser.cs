using DeveSpotnet.Models;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DeveSpotnet.SpotnetHelpers.Parsers
{
    public static class FullHeaderParser
    {   
        /// <summary>
        /// Converts the raw NNTP header lines of a *full* spot into a
        /// <see cref="ParsedFullHeader"/>.
        /// </summary>
        public static ParsedFullHeader ParseFullHeader(IEnumerable<string> headerLines)
        {
            var result = new ParsedFullHeader();

            /* ───────────────────────── 1. walk through the raw headers ───────────────────────── */
            foreach (string raw in headerLines)
            {
                if (string.IsNullOrWhiteSpace(raw)) continue;

                int colon = raw.IndexOf(':');
                if (colon < 0) continue;

                string key = raw[..colon].Trim().ToLowerInvariant();
                string value = raw[(colon + 1)..].TrimStart();

                switch (key)
                {
                    case "from": ParseFrom(raw, result); break;
                    case "date": ParseDate(value, result); break;
                    case "x-xml": result.FullXml += value; break;
                    case "x-user-signature":
                        result.UserSignature = SpotUtil.SpotUnprepareBase64(value); break;
                    case "x-xml-signature":
                        result.XmlSignature = SpotUtil.SpotUnprepareBase64(value); break;
                    case "x-newsreader": result.Newsreader = value; break;
                    case "x-user-avatar": result.UserAvatar += value; break;
                    case "x-user-key": ParseUserKey(value, result); break;
                }
            }

            /* ───────────────────────── 2. add <Extra><Newsreader>…</Newsreader></Extra> ───────── */
            if (!string.IsNullOrEmpty(result.FullXml) && !string.IsNullOrEmpty(result.Newsreader))
            {
                XDocument? doc = TryParseXml(result.FullXml);

                // If the first attempt failed, replicate the PHP fallback:
                // insert a space after an apostrophe followed by an ASCII letter.
                if (doc is null)
                {
                    string fixedXml = Regex.Replace(
                                          result.FullXml,
                                          @"'([A-Za-z])",
                                          "' $1");
                    doc = TryParseXml(fixedXml);
                }

                if (doc is not null)
                {
                    XElement extra = doc.Root!.Element("Extra") ?? new XElement("Extra");
                    if (extra.Parent is null) doc.Root!.Add(extra);

                    extra.Add(new XElement("Newsreader", result.Newsreader));
                    result.FullXml = doc.Declaration + doc.Root!.ToString(SaveOptions.DisableFormatting);
                }
            }

            return result;
        }

        private static XDocument? TryParseXml(string xml)
        {
            try
            {
                return XDocument.Parse(xml, LoadOptions.PreserveWhitespace);
            }
            catch
            {
                return null;
            }
        }

        /* ------------------------------------------------------------------ */
        /*  Helpers                                                            */
        /* ------------------------------------------------------------------ */

        private static void ParseFrom(string raw, ParsedFullHeader result)
        {
            int lt = raw.IndexOf('<');
            if (lt <= 0) return;

            int start = "From: ".Length;
            int len = lt - start - 1;               // strip trailing space
            if (len <= 0) return;

            string name = raw.Substring(start, len).Trim();
            result.FromHdr = EncodingHelper.Utf8ToIso88591(name);
        }

        private static void ParseDate(string value, ParsedFullHeader result)
        {
            if (DateTimeOffset.TryParse(value,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal,
                    out var dto))
            {
                result.Stamp = dto.UtcDateTime;
            }
        }

        private static void ParseUserKey(string xml, ParsedFullHeader result)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);
                var exp = doc.Root?.Element("Exponent")?.Value;
                var mod = doc.Root?.Element("Modulus")?.Value;
                if (!string.IsNullOrEmpty(exp) && !string.IsNullOrEmpty(mod))
                {
                    result.UserKey = new RsaKey { Exponent = exp, Modulo = mod };
                }
            }
            catch
            {
                /* ignore malformed XML exactly like SpotWeb does */
            }
        }
    }
}
