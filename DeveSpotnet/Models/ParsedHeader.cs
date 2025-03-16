using DeveSpotnet.SpotnetHelpers;

namespace DeveSpotnet.Models
{
    public class ParsedHeader
    {
        public string Header { get; set; }
        public string SelfSignedPubKey { get; set; }
        public string UserSignature { get; set; }
        public bool Verified { get; set; }
        public int FileSize { get; set; }
        public string MessageId { get; set; }
        public DateTime Stamp { get; set; }
        public string Poster { get; set; }
        public int Category { get; set; }
        public int KeyId { get; set; }
        public string SubCatA { get; set; }
        public string SubCatB { get; set; }
        public string SubCatC { get; set; }
        public string SubCatD { get; set; }
        public string SubCatZ { get; set; }
        public bool WasSigned { get; set; }
        public string SpotterId { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; }
        /// <summary>
        /// Contains the signature field extracted from the header.
        /// </summary>
        public string HeaderSign { get; set; }
        /// <summary>
        /// When available, the full public key (for user signature verification).
        /// </summary>
        public RsaKey UserKey { get; set; }
        /// <summary>
        /// Optional XML signature (if provided by the client).
        /// </summary>
        public string XmlSignature { get; set; }
    }
}
