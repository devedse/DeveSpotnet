using System.Security.Cryptography;
using System.Text;

namespace DeveSpotnet.SpotnetHelpers
{
    public static class SpotUtil
    {
        /// <summary>
        /// Decodes specially encoded binary postings.
        /// Replaces "=C" with newline, "=B" with carriage return,
        /// "=A" with null character, and "=D" with "=".
        /// </summary>
        public static string UnspecialZipStr(string input)
        {
            if (input == null) return null;
            return input.Replace("=C", "\n")
                        .Replace("=B", "\r")
                        .Replace("=A", "\0")
                        .Replace("=D", "=");
        }

        /// <summary>
        /// Encodes a string so that only a specific set of characters is escaped.
        /// Replaces "=" with "=D", newline with "=C", carriage return with "=B",
        /// and null character with "=A".
        /// </summary>
        public static string SpecialZipStr(string input)
        {
            if (input == null) return null;
            return input.Replace("=", "=D")
                        .Replace("\n", "=C")
                        .Replace("\r", "=B")
                        .Replace("\0", "=A");
        }

        /// <summary>
        /// Prepares a Base64-encoded string by replacing "/" with "-s"
        /// and "+" with "-p".
        /// </summary>
        public static string SpotPrepareBase64(string input)
        {
            if (input == null) return null;
            return input.Replace("/", "-s")
                        .Replace("+", "-p");
        }

        /// <summary>
        /// Reverses the Base64 preparation by ensuring the input is padded
        /// to a multiple of 4 and replacing "-s" with "/" and "-p" with "+".
        /// </summary>
        public static string SpotUnprepareBase64(string input)
        {
            if (input == null) return null;
            int paddingLen = input.Length % 4;
            if (paddingLen > 0)
            {
                input += new string('=', 4 - paddingLen);
            }
            return input.Replace("-s", "/")
                        .Replace("-p", "+");
        }

        /// <summary>
        /// Validates that the messageId does not contain any invalid characters.
        /// </summary>
        public static bool ValidMessageId(string messageId)
        {
            if (messageId == null) return false;
            string invalidChars = "<>";
            foreach (char c in messageId)
            {
                if (invalidChars.IndexOf(c) >= 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Calculates the spotter ID using the user's public key.
        /// The public key (in Base64) is decoded, its CRC32 computed,
        /// and the resulting 4-byte value is Base64 encoded (with '/', '+', and '=' removed).
        /// </summary>
        public static string CalculateSpotterId(string userKey)
        {
            if (string.IsNullOrEmpty(userKey))
                throw new ArgumentException("User key cannot be null or empty.", nameof(userKey));

            // Decode the Base64-encoded user key.
            byte[] decoded = Convert.FromBase64String(userKey);
            uint crc = Crc32.Compute(decoded);

            // Construct a 4-byte array from the CRC32 (little-endian).
            byte[] crcBytes = new byte[4];
            crcBytes[0] = (byte)(crc & 0xFF);
            crcBytes[1] = (byte)((crc >> 8) & 0xFF);
            crcBytes[2] = (byte)((crc >> 16) & 0xFF);
            crcBytes[3] = (byte)((crc >> 24) & 0xFF);

            // Base64 encode and remove '/', '+', '=' characters.
            string base64 = Convert.ToBase64String(crcBytes);
            return base64.Replace("/", "").Replace("+", "").Replace("=", "");
        }

        /// <summary>
        /// Converts a string from a specified source encoding to UTF8.
        /// For example, convert from "ISO-8859-1" to UTF8.
        /// </summary>
        public static string ConvertToUtf8(string input, string sourceEncoding)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            Encoding srcEnc = Encoding.GetEncoding(sourceEncoding);
            byte[] srcBytes = srcEnc.GetBytes(input);
            return Encoding.UTF8.GetString(srcBytes);
        }

        /// <summary>
        /// Computes the SHA1 hash of the provided input string.
        /// Returns the hash as a hexadecimal string.
        /// </summary>
        public static string ComputeSha1(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
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

    /// <summary>
    /// Simple CRC32 implementation.
    /// </summary>
    public static class Crc32
    {
        private static readonly uint[] Table = new uint[256];

        static Crc32()
        {
            uint polynomial = 0xEDB88320;
            for (uint i = 0; i < 256; i++)
            {
                uint crc = i;
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 1) == 1)
                        crc = (crc >> 1) ^ polynomial;
                    else
                        crc >>= 1;
                }
                Table[i] = crc;
            }
        }

        /// <summary>
        /// Computes the CRC32 for the given byte array.
        /// </summary>
        public static uint Compute(byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            uint crc = 0xFFFFFFFF;
            foreach (byte b in bytes)
            {
                crc = (crc >> 8) ^ Table[(crc & 0xFF) ^ b];
            }
            return crc ^ 0xFFFFFFFF;
        }
    }
}