using System.Text;
using System.Xml.Linq;

namespace DeveSpotnet.SpotnetHelpers
{
    public static class EncodingHelper
    {
        public static string IsoToUtf8(XElement? el)
        {
            if (el is null) return string.Empty;
            var iso = Encoding.GetEncoding("ISO-8859-1");
            byte[] bytes = iso.GetBytes((string)el);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string Utf8ToIso88591(string input)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(input);
            byte[] iso = Encoding.Convert(Encoding.UTF8,
                                           Encoding.GetEncoding("ISO-8859-1"),
                                           utf8);
            return Encoding.GetEncoding("ISO-8859-1").GetString(iso);
        }
    }
}
