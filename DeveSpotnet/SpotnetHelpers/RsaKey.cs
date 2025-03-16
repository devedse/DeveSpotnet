namespace DeveSpotnet.SpotnetHelpers
{

    public class RsaKey
    {
        /// <summary>
        /// Base64 encoded RSA modulus.
        /// </summary>
        public string Modulo { get; set; }
        /// <summary>
        /// Base64 encoded RSA exponent.
        /// </summary>
        public string Exponent { get; set; }
    }
}
