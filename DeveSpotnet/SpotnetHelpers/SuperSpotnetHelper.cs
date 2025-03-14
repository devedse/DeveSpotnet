using SpotnetClient.SpotnetHelpers;

namespace DeveSpotnet.SpotnetHelpers
{
    public static class SuperSpotnetHelper
    {
        public static Dictionary<int, RsaKey> RSAKeys = new Dictionary<int, RsaKey>
        {
            { 2, new RsaKey { Modulo = "ys8WSlqonQMWT8ubG0tAA2Q07P36E+CJmb875wSR1XH7IFhEi0CCwlUzNqBFhC+P", Exponent = "AQAB" } },
            { 3, new RsaKey { Modulo = "uiyChPV23eguLAJNttC/o0nAsxXgdjtvUvidV2JL+hjNzc4Tc/PPo2JdYvsqUsat", Exponent = "AQAB" } },
            { 4, new RsaKey { Modulo = "1k6RNDVD6yBYWR6kHmwzmSud7JkNV4SMigBrs+jFgOK5Ldzwl17mKXJhl+su/GR9", Exponent = "AQAB" } }
        };

        /// <summary>
        /// Parses a spot header string into a DbSpot instance.
        /// Returns null if the header is invalid.
        /// </summary>
        /// <param name="subj">Subject string (contains title and tag information)</param>
        /// <param name="from">The From header string</param>
        /// <param name="date">A date string (to be parsed)</param>
        /// <param name="messageId">Message identifier</param>
        /// <param name="rsaKeys">A dictionary of RSA keys indexed by keyid</param>
        public static DbSpot ParseHeader(string subj, string from, string date, string messageId)
        {
            var spot = new DbSpot();

            // --- Extract and process the "From" header ---
            int fromInfoPos = from.IndexOf('<');
            if (fromInfoPos == -1)
                return null;

            // Remove poster's name and the '<' and '>' characters.
            string fromAddressStr = from.Substring(fromInfoPos + 1, from.Length - fromInfoPos - 2);
            string[] fromAddress = fromAddressStr.Split('@');
            if (fromAddress.Length < 2)
                return null;

            // The part after the @ holds our header fields.
            spot.Header = fromAddress[1];

            // The part before the @ may include both the self-signed public key and user signature (separated by a dot).
            string[] headerSignatureTemp = fromAddress[0].Split('.');
            spot.SelfSignedPubKey = SpotUtil.SpotUnprepareBase64(headerSignatureTemp[0]);
            if (headerSignatureTemp.Length > 1)
                spot.UserSignature = SpotUtil.SpotUnprepareBase64(headerSignatureTemp[1]);

            spot.Verified = false;
            spot.FileSize = 0;
            spot.MessageId = messageId;

            if (DateTime.TryParse(date, out DateTime parsedDate))
                spot.Stamp = parsedDate;
            else
                spot.Stamp = DateTime.Now;

            // --- Split the header into fields ---
            string[] fields = spot.Header.Split('.');
            if (fields.Length < 6)
                return null;

            spot.Poster = from.Substring(0, fromInfoPos).Trim();
            if (fields[0].Length < 2)
                return null;

            // Category is the first digit (minus one per PHP logic).
            spot.Category = int.Parse(fields[0].Substring(0, 1)) - 1;
            // KeyId is the second digit.
            spot.KeyId = int.Parse(fields[0].Substring(1, 1));
            // FileSize is from the second field.
            spot.FileSize = int.Parse(fields[1]);

            // Initialize subcategory fields.
            spot.SubCatA = "";
            spot.SubCatB = "";
            spot.SubCatC = "";
            spot.SubCatD = "";
            spot.SubCatZ = "";
            spot.WasSigned = false;
            spot.SpotterId = "";
            bool isRecentKey = spot.KeyId != 1;

            if (spot.KeyId < 0)
                return null;

            // --- Process subcategories ---
            string strCatList = fields[0].Substring(2).ToLower() + "!!!";
            int strCatListLen = strCatList.Length;
            string tmpCatBuild = "";
            var validSubcats = new HashSet<char> { 'a', 'b', 'c', 'd', 'z' };

            for (int i = 0; i < strCatListLen; i++)
            {
                char current = strCatList[i];
                if (!char.IsDigit(current) && tmpCatBuild.Length > 0)
                {
                    char cat = tmpCatBuild[0];
                    if (validSubcats.Contains(cat))
                    {
                        int numberPart = 0;
                        if (tmpCatBuild.Length > 1)
                            int.TryParse(tmpCatBuild.Substring(1), out numberPart);
                        string formatted = cat.ToString() + numberPart.ToString() + "|";
                        switch (cat)
                        {
                            case 'a':
                                spot.SubCatA += formatted;
                                break;
                            case 'b':
                                spot.SubCatB += formatted;
                                break;
                            case 'c':
                                spot.SubCatC += formatted;
                                break;
                            case 'd':
                                spot.SubCatD += formatted;
                                break;
                            case 'z':
                                spot.SubCatZ += formatted;
                                break;
                        }
                    }
                    tmpCatBuild = "";
                }
                tmpCatBuild += current;
            }

            // Ensure that subcatz is always set.
            if (string.IsNullOrEmpty(spot.SubCatZ))
                spot.SubCatZ = SpotCategories.createSubcatZ(spot.Category, spot.SubCatA + spot.SubCatB + spot.SubCatD);

            // Map deprecated and language subcategories.
            spot.SubCatD = SpotCategories.mapDeprecatedGenreSubCategories(spot.Category, spot.SubCatD, spot.SubCatZ);
            spot.SubCatC = SpotCategories.mapLanguageSubCategories(spot.Category, spot.SubCatC, spot.SubCatZ);

            // --- Process the subject (title and tag) ---
            if (subj.Contains("=?") && subj.Contains("?="))
            {
                ParsingLegacy legacyParser = new ParsingLegacy();
                subj = subj.Replace("?= =?", "?==?");
                subj = legacyParser.OldEncodingParse(subj).Trim().Replace("\r", "").Replace("\n", "");
            }

            if (isRecentKey)
            {
                string[] tmp = subj.Split('|');
                spot.Title = tmp[0].Trim();
                spot.Tag = tmp.Length > 1 ? tmp[1].Trim() : "";
            }
            else
            {
                string[] tmp = subj.Split('|');
                if (tmp.Length <= 1)
                    tmp = new string[] { subj };

                spot.Tag = tmp[tmp.Length - 1].Trim();
                spot.Title = tmp.Length > 2 ? string.Join("|", tmp.Take(tmp.Length - 2)).Trim() : tmp[0].Trim();

                if (spot.Title.Contains('\u00C2') || spot.Title.Contains('\u00C3'))
                {
                    ParsingLegacy legacyParser = new ParsingLegacy();
                    spot.Title = legacyParser.OldEncodingParse(spot.Title).Trim();
                }
            }

            // Title and poster are mandatory.
            if (string.IsNullOrEmpty(spot.Title) || string.IsNullOrEmpty(spot.Poster))
                return spot;

            // --- Determine whether the spot must be signed ---
            bool mustBeSigned = isRecentKey || (spot.Stamp > new DateTime(2011, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            if (mustBeSigned)
            {
                spot.HeaderSign = fields[fields.Length - 1];
                spot.WasSigned = !string.IsNullOrEmpty(spot.HeaderSign);
            }
            else
            {
                spot.Verified = true;
                spot.WasSigned = false;
            }

            // --- Signature verification (if the spot was signed) ---
            if (spot.WasSigned)
            {
                int signingMethod = (spot.KeyId == 7) ? 2 : 1;
                switch (signingMethod)
                {
                    case 1:
                        {
                            string signature = SpotUtil.SpotUnprepareBase64(spot.HeaderSign);
                            if (RSAKeys.ContainsKey(spot.KeyId))
                            {
                                if (spot.KeyId == 2 && spot.FileSize == 999 && spot.SelfSignedPubKey.Length > 50)
                                {
                                    string userSignedHash = SpotUtil.ComputeSha1("<" + spot.MessageId + ">");
                                    if (userSignedHash.Substring(0, 4) == "0000")
                                    {
                                        var userRsaKey = new Dictionary<int, RsaKey>
                                        {
                                            { 2, new RsaKey { Modulo = spot.SelfSignedPubKey, Exponent = "AQAB" } }
                                        };
                                        if (ServicesSigning.VerifySpotHeader(spot, signature, userRsaKey))
                                            spot.SpotterId = SpotUtil.CalculateSpotterId(spot.SelfSignedPubKey);
                                    }
                                }
                                else
                                {
                                    spot.Verified = ServicesSigning.VerifySpotHeader(spot, signature, RSAKeys);
                                }
                            }
                        }
                        break;
                    case 2:
                        {
                            string signature = SpotUtil.SpotUnprepareBase64(spot.HeaderSign);
                            string userSignedHash = SpotUtil.ComputeSha1("<" + spot.MessageId + ">");
                            spot.Verified = (userSignedHash.Substring(0, 4) == "0000");
                            if (spot.Verified)
                            {
                                var userRsaKey = new Dictionary<int, RsaKey>
                                {
                                    { 7, new RsaKey { Modulo = spot.SelfSignedPubKey, Exponent = "AQAB" } }
                                };
                                if (ServicesSigning.VerifySpotHeader(spot, signature, userRsaKey))
                                    spot.SpotterId = SpotUtil.CalculateSpotterId(spot.SelfSignedPubKey);
                            }
                        }
                        break;
                }

                if (spot.Verified &&
                    !string.IsNullOrEmpty(spot.UserSignature) &&
                    !string.IsNullOrEmpty(spot.SelfSignedPubKey))
                {
                    spot.SpotterId = SpotUtil.CalculateSpotterId(spot.SelfSignedPubKey);
                    spot.UserKey = new RsaKey { Modulo = spot.SelfSignedPubKey, Exponent = "AQAB" };
                    spot.Verified = ServicesSigning.VerifyFullSpot(spot);
                }
            }

            // --- Convert text fields to UTF8 if verified ---
            if (spot.Verified)
            {
                spot.Title = SpotUtil.ConvertToUtf8(spot.Title, "ISO-8859-1");
                spot.Poster = SpotUtil.ConvertToUtf8(spot.Poster, "ISO-8859-1");
                spot.Tag = SpotUtil.ConvertToUtf8(spot.Tag, "ISO-8859-1");

                if (DateTime.Now < spot.Stamp)
                    spot.Stamp = DateTime.Now;
            }

            return spot;
        }
    }

    public class ParsingLegacy
    {
        public string OldEncodingParse(string input)
        {
            // Implement your legacy parsing here.
            return input;
        }
    }
}
