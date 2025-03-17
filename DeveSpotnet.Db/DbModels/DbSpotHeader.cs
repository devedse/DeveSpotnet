using System.ComponentModel.DataAnnotations;

namespace DeveSpotnet.Db.DbModels
{
    public class DbSpotHeader
    {
        [Key]
        public int ArticleNumber { get; set; }

        public string? Subject { get; set; }

        public string? From { get; set; }

        public string? Date { get; set; }

        public string? MessageID { get; set; }

        public string? References { get; set; }

        public int Bytes { get; set; }

        public int Lines { get; set; }

        public int Code { get; set; }

        public string? Message { get; set; }

        //public int? Metadata_Season { get; set; }
        //public int? Metadata_Episode { get; set; }

        public bool ParsedHeader_Valid { get; set; }

        public string? ParsedHeader_Header { get; set; }
        public string? ParsedHeader_SelfSignedPubKey { get; set; }
        public string? ParsedHeader_UserSignature { get; set; }
        public bool? ParsedHeader_Verified { get; set; }
        public int? ParsedHeader_FileSize { get; set; }
        public string? ParsedHeader_MessageId { get; set; }
        public DateTime? ParsedHeader_Stamp { get; set; }
        public string? ParsedHeader_Poster { get; set; }
        public int? ParsedHeader_Category { get; set; }
        public int? ParsedHeader_KeyId { get; set; }
        public string? ParsedHeader_SubCatA { get; set; }
        public string? ParsedHeader_SubCatB { get; set; }
        public string? ParsedHeader_SubCatC { get; set; }
        public string? ParsedHeader_SubCatD { get; set; }
        public string? ParsedHeader_SubCatZ { get; set; }
        public bool? ParsedHeader_WasSigned { get; set; }
        public string? ParsedHeader_SpotterId { get; set; }
        public string? ParsedHeader_Title { get; set; }
        public string? ParsedHeader_Tag { get; set; }

        /// <summary>
        /// Contains the signature field extracted from the header.
        /// </summary>
        public string? ParsedHeader_HeaderSign { get; set; }

        /// <summary>
        /// Base64 encoded RSA modulus.
        /// </summary>
        public string? ParsedHeader_UserKey_Modulo { get; set; }
        /// <summary>
        /// Base64 encoded RSA exponent.
        /// </summary>
        public string? ParsedHeader_UserKey_Exponent { get; set; }


        /// <summary>
        /// Optional XML signature (if provided by the client).
        /// </summary>
        public string? ParsedHeader_XmlSignature { get; set; }
    }
}
