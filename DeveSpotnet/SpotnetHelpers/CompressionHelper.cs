using System.IO.Compression;
using System.Text;

namespace DeveSpotnet.SpotnetHelpers
{
    public static class CompressionHelper
    {
        /* ====================================================================
         *  Helper: inflate with dual strategy (z-lib header, then raw)
         * ==================================================================== */
        public static async Task<string?> TryInflateAsync(
            byte[] buffer,
            ILogger? log,
            CancellationToken ct)
        {
            // 1st attempt: assume z-lib header is present
            if (await InflateCoreAsync(buffer, ct) is { } ok1) return ok1;

            // 2nd attempt: prepend the standard 0x78 0x9C z-lib header
            var withHeader = new byte[buffer.Length + 2];
            withHeader[0] = 0x78;                   // CMF = Deflate + 32 KiB window
            withHeader[1] = 0x9C;                   // FLG = default check bits
            Buffer.BlockCopy(buffer, 0, withHeader, 2, buffer.Length);

            if (await InflateCoreAsync(withHeader, ct) is { } ok2) return ok2;

            log?.LogWarning("Deflate failed with and without header – returning raw data");
            return null;
        }

        public static async Task<string?> InflateCoreAsync(byte[] src, CancellationToken ct)
        {
            try
            {
                await using var input = new MemoryStream(src);
                await using var dfl = new DeflateStream(input, CompressionMode.Decompress, leaveOpen: false);
                await using var output = new MemoryStream();
                await dfl.CopyToAsync(output, 81920, ct);
                return Encoding.Latin1.GetString(output.ToArray());
            }
            catch (InvalidDataException)
            {
                return null;            // invalid header/stream – caller will retry
            }
        }
    }
}
