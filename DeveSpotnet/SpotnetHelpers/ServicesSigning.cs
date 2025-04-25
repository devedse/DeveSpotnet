using DeveSpotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DeveSpotnet.SpotnetHelpers
{
    internal static class PhpVerifyLogger
    {
        private static string EscapeForPhp(string s) =>
            s.Replace("\\", "\\\\").Replace("'", "\\'");

        /// <summary>Writes the PHP test-statement plus the C# outcome.</summary>
        public static void Write(string toCheck, string signature,
                                 RsaKey rsaKey, bool csharpResult)
        {
            string php =
                $"echo checkRsaSignature('{EscapeForPhp(toCheck)}','{signature}'," +
                $"['modulo'=>'{rsaKey.Modulo}','exponent'=>'{rsaKey.Exponent}'],true)" +
                "?'true':'false';" +
                $" // C#: {(csharpResult ? "true" : "false")}, Php (expect): {(csharpResult ? "true" : "false")}";

            File.AppendAllLines("php-verify.log", new[] { php });
        }
    }

    /// <summary>
    /// Provides methods for RSA signature verification and related operations.
    /// Modeled after your PHP Services_Signing_Base code.
    /// </summary>
    public static class ServicesSigning
    {
        // -----------------------------------------------------------------------------
        // 2.  CHANGE YOUR CheckRsaSignature METHOD – only the marked part is new.
        //    The helper is called *only* when the signature fails.
        // -----------------------------------------------------------------------------
        public static bool CheckRsaSignature(string toCheck, string signature,
                                             RsaKey rsaKey, bool useCache)
        {
            var sb = new StringBuilder();
            try
            {
                using RSA rsa = RSA.Create();

                RSAParameters parameters = new()
                {
                    Modulus = PHPBase64.FromBase64String(rsaKey.Modulo),
                    Exponent = PHPBase64.FromBase64String(rsaKey.Exponent)
                };

                sb.Append($"ModulusLength: {parameters.Modulus.Length}");
                rsa.ImportParameters(parameters);

                byte[] dataBytes = Encoding.UTF8.GetBytes(toCheck);
                byte[] signatureBytes = PHPBase64.FromBase64String(signature);

                bool ok = rsa.VerifyData(dataBytes, signatureBytes,
                                         HashAlgorithmName.SHA1,
                                         RSASignaturePadding.Pkcs1);

                // >>> NEW >>>  dump a PHP one-liner if C# says the sig is NOT ok
                if (!ok)
                {
                    PhpVerifyLogger.Write(toCheck, signature, rsaKey, ok);
                }
                else
                {
                    PhpVerifyLogger.Write(toCheck, signature, rsaKey, ok);
                }
                // <<< NEW <<<

                return ok;
            }
            catch (FormatException)
            {
                sb.AppendLine($" Exception: Format: {rsaKey.Modulo},{rsaKey.Exponent}");
                return false;
            }
            catch (Exception e)
            {
                sb.AppendLine($" Exception: {e}");
                return false;
            }
            finally
            {
                File.AppendAllLines("superlog.txt", new[] { sb.ToString().Trim() });
            }
        }

        /// <summary>
        /// Checks the RSA signature on the provided data.
        /// </summary>
        /// <param name="toCheck">The data string to verify.</param>
        /// <param name="signature">The signature (base64 encoded).</param>
        /// <param name="rsaKey">RSA key (with Base64-encoded Modulo and Exponent).</param>
        /// <param name="useCache">Not used in this implementation.</param>
        /// <returns>True if the signature is valid; otherwise, false.</returns>
        public static bool CheckRsaSignatureOld(string toCheck, string signature, RsaKey rsaKey, bool useCache)
        {
            var sb = new StringBuilder();
            try
            {
                using (RSA rsa = RSA.Create())
                {
                    RSAParameters parameters = new RSAParameters
                    {
                        Modulus = PHPBase64.FromBase64String(rsaKey.Modulo),
                        Exponent = PHPBase64.FromBase64String(rsaKey.Exponent)
                    };

                    sb.Append($"ModulusLength: {parameters.Modulus.Length}");

                    rsa.ImportParameters(parameters);
                    byte[] dataBytes = Encoding.UTF8.GetBytes(toCheck);
                    byte[] signatureBytes = PHPBase64.FromBase64String(signature);
                    return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                }
            }
            catch (FormatException e)
            {
                sb.AppendLine($" Exception: Format: {rsaKey.Modulo},{rsaKey.Exponent}");
                return false;
            }
            catch (Exception e)
            {
                sb.AppendLine($" Exception: {e}");
                return false;
            }
            finally
            {
                File.AppendAllLines("superlog.txt", [sb.ToString().Trim()]);
            }

        }

        /// <summary>
        /// Verifies the spot header signature.
        /// </summary>
        /// <param name="spot">The DbSpot instance containing the spot data.</param>
        /// <param name="signature">The header signature (base64 encoded).</param>
        /// <param name="rsaKeys">A dictionary of RSA keys indexed by keyid.</param>
        /// <returns>True if the signature is valid; otherwise, false.</returns>
        public static bool VerifySpotHeader(ParsedHeader spot, string signature, Dictionary<int, RsaKey> rsaKeys)
        {
            // Construct the string to verify:
            // PHP: $toCheck = $spot['title'] . substr($spot['header'], 0, strlen($spot['header']) - strlen($spot['headersign']) - 1) . $spot['poster'];
            int headerLen = spot.Header?.Length ?? 0;
            int headerSignLen = spot.HeaderSign?.Length ?? 0;
            int substringLength = headerLen - headerSignLen - 1;
            string headerPart = substringLength > 0 ? spot.Header.Substring(0, substringLength) : "";
            string toCheck = spot.Title + headerPart + spot.Poster;

            if (!rsaKeys.ContainsKey(spot.KeyId))
            {
                return false;
            }

            RsaKey key = rsaKeys[spot.KeyId];
            return CheckRsaSignature(toCheck, signature, key, true);
        }

        /// <summary>
        /// Verifies the full spot signature.
        /// </summary>
        /// <param name="spot">The DbSpot instance.</param>
        /// <returns>True if the full spot signature is valid; otherwise, false.</returns>
        public static bool VerifyFullSpot(ParsedHeader spot)
        {
            if (string.IsNullOrEmpty(spot.UserSignature) || spot.UserKey == null)
                return false;

            // First, verify using '<messageid>' as in the PHP code.
            string toCheck = "<" + spot.MessageId + ">";
            bool verified = CheckRsaSignature(toCheck, spot.UserSignature, spot.UserKey, false);
            // If not verified and an XML signature is provided, try that.
            if (!verified && !string.IsNullOrEmpty(spot.XmlSignature))
            {
                verified = CheckRsaSignature(spot.XmlSignature, spot.UserSignature, spot.UserKey, false);
            }
            return verified;
        }

        /// <summary>
        /// Verifies a comment’s signature.
        /// </summary>
        /// <param name="comment">A dictionary representing the comment.</param>
        /// <returns>True if the comment signature is valid; otherwise, false.</returns>
        public static bool VerifyComment(Dictionary<string, object> comment)
        {
            bool verified = false;
            if (comment.ContainsKey("user-signature") && comment.ContainsKey("user-key") &&
                comment["user-signature"] is string userSignature &&
                comment["user-key"] is RsaKey userKey &&
                comment.ContainsKey("messageid") && comment["messageid"] is string messageId)
            {
                string toCheck = "<" + messageId + ">";
                verified = CheckRsaSignature(toCheck, userSignature, userKey, false);
                if (!verified && comment.ContainsKey("body") && comment.ContainsKey("fromhdr"))
                {
                    // Assuming body is a List<string>.
                    var bodyLines = comment["body"] as List<string> ?? new List<string>();
                    string bodyCombined = string.Join("\r\n", bodyLines) + "\r\n";
                    string fromHdr = comment["fromhdr"] as string ?? "";
                    toCheck = "<" + messageId + ">" + bodyCombined + fromHdr;
                    verified = CheckRsaSignature(toCheck, userSignature, userKey, false);
                }
            }
            return verified;
        }

        /// <summary>
        /// Calculates an "expensive" hash so that the SHA1 hash of the message starts with "0000".
        /// </summary>
        public static string MakeExpensiveHash(string prefix, string suffix)
        {
            int runCount = 0;
            string uniquePart = "";
            string hash = ComputeSha1(prefix + suffix);
            while (!hash.StartsWith("0000"))
            {
                if (runCount > 400000)
                    throw new Exception("Unable to calculate SHA1 hash: " + runCount);
                runCount++;
                uniquePart = MakeRandomStr(15);
                hash = ComputeSha1(prefix + uniquePart + suffix);
            }
            return prefix + uniquePart + suffix;
        }

        /// <summary>
        /// Creates a random string of the specified length.
        /// </summary>
        public static string MakeRandomStr(int len)
        {
            const string possibleChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var random = new Random();
            var sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                int index = random.Next(possibleChars.Length);
                sb.Append(possibleChars[index]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Helper method to compute the SHA1 hash of an input string.
        /// </summary>
        private static string ComputeSha1(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
