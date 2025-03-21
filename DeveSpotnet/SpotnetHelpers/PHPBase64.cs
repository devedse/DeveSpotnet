using System.Text;

namespace DeveSpotnet.SpotnetHelpers
{
    public static class PHPBase64
    {
        public static byte[] FromBase64String(string base64)
        {
            if (base64 == null)
                throw new ArgumentNullException(nameof(base64));

            // Trim whitespace if needed (PHP's base64_decode ignores whitespace)
            base64 = base64.Trim();

            // Remove all trailing '=' characters
            string trimmed = base64.TrimEnd('=');

            // Calculate the number of '=' required to pad the string to a multiple of 4.
            int padNeeded = (4 - (trimmed.Length % 4)) % 4;
            string normalized = trimmed + new string('=', padNeeded);

            return Convert.FromBase64String(normalized);
        }
    }
}
