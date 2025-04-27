using DeveSpotnet.SpotnetHelpers;

namespace DeveSpotnet.Models
{
    public sealed class ParsedFullHeader
    {
        public string FromHdr { get; set; } = string.Empty;  // $tmpAr['fromhdr']
        public DateTime Stamp { get; set; }                  // $tmpAr['stamp']
        public string FullXml { get; set; } = string.Empty;  // $tmpAr['fullxml']
        public string UserSignature { get; set; } = string.Empty;  // $tmpAr['user-signature']
        public string XmlSignature { get; set; } = string.Empty;  // $tmpAr['xml-signature']
        public string Newsreader { get; set; } = string.Empty;  // $tmpAr['newsreader']
        public string UserAvatar { get; set; } = string.Empty;  // $tmpAr['user-avatar']
        public RsaKey? UserKey { get; set; }                  // $tmpAr['user-key'] (Exponent & Modulus)
    }
}
