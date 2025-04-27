using DeveSpotnet.Configuration;
using DeveSpotnet.Models;
using DeveSpotnet.SpotnetHelpers;
using DeveSpotnet.SpotnetHelpers.Parsers;
using mcnntp.common.client;
using Microsoft.Extensions.Options;
using System.IO.Compression;
using System.Text;

namespace DeveSpotnet.Services
{
    public class UsenetService : IUsenetService, IAsyncDisposable
    {
        private static readonly object _lockObject = new object();
        private readonly UsenetSettings _settings;
        private readonly ILogger<UsenetService> _logger;

        private NntpClient _nntpClient;

        public UsenetService(IOptions<UsenetSettings> options, ILogger<UsenetService> logger)
        {
            _settings = options.Value;
            _logger = logger;

            _nntpClient = ConnectClientAsync(CancellationToken.None).Result ?? throw new InvalidOperationException("Could not start NntpClient, see errors for more details");
        }

        /// <summary>
        /// Connects to the NNTP server and authenticates.
        /// </summary>
        private async Task<NntpClient?> ConnectClientAsync(CancellationToken token)
        {
            var client = new NntpClient
            {
                Port = _settings.Port
            };

            var connectResult = await client.ConnectAsync(_settings.Host, _settings.Ssl);
            if (!connectResult.IsSuccessfullyComplete)
            {
                _logger.LogError("Could not connect to the Usenet server at {Host}", _settings.Host);
                return null;
            }

            var authResult = await client.AuthenticateUsernamePasswordAsync(_settings.Username, _settings.Password);
            if (!authResult.IsSuccessfullyComplete)
            {
                _logger.LogError("Usenet authentication failed for user {Username}", _settings.Username);
                return null;
            }

            _logger.LogInformation("Connected and authenticated to NNTP server {Host}", _settings.Host);
            return client;
        }

        public async Task<ParsedFullSpot?> ReadFullSpot(string messageId)
        {
            var header = await _nntpClient.HeadAsync($"<{messageId}>");

            var fullHeader = FullHeaderParser.ParseFullHeader(header.Lines);

            //var xoverThing = await client.XOverAsync(header.a, header.ArticleNumber);
            var verified = ServicesSigning.VerifyFullSpot(new ParsedHeader()
            {
                UserSignature = fullHeader.UserSignature,
                UserKey = fullHeader.UserKey,
                MessageId = messageId,
                XmlSignature = fullHeader.XmlSignature,
            });


            if (verified)
            {
                var spotterId = SpotUtil.CalculateSpotterId(fullHeader.UserKey.Modulo);
            }

            var fullParsedSpot = FullSpotParser.ParseFull(fullHeader.FullXml);

            return fullParsedSpot;
        }

        /// <summary>
        /// Returns the NZB file for a fully-parsed spot.
        /// (No caching yet – plug in your favourite cache later.)
        /// </summary>
        public async Task<string> FetchNzbAsync(ParsedFullSpot spot,
                                               CancellationToken ct = default)
        {
            if (spot.NzbSegments is null || spot.NzbSegments.Count == 0)
            {
                _logger.LogWarning("Spot {Id} has no NZB segments", spot.MessageId);
                return string.Empty;
            }

            // SpotWeb always stores NZB blocks compressed.
            string nzbXml = await ReadBinaryAsync(spot.NzbSegments, compressed: true, ct);

            // Fallback if the file is empty / corrupt – identical to SpotWeb.
            if (string.IsNullOrWhiteSpace(nzbXml))
            {
                nzbXml = "<xml><error>Invalid NZB file, unable to retrieve correct NZB file</error></xml>";
            }

            return nzbXml;
        }

        /// <summary>
        /// Retrieves and (optionally) decompresses a binary that is spread
        /// across several NNTP segments, reproducing SpotWeb’s
        /// <c>readBinary()</c> behaviour.
        /// </summary>
        /// <param name="segmentList">List of Message-IDs *without* “&lt;…&gt;”.</param>
        /// <param name="compressed">True when the payload is DEFLATE-compressed.</param>
        public async Task<string> ReadBinaryAsync(
            IEnumerable<string> segmentList,
            bool compressed,
            CancellationToken ct = default)
        {
            var builder = new StringBuilder(8192);

            foreach (string seg in segmentList)
            {
                var resp = await _nntpClient.BodyAsync($"<{seg}>", ct);
                if (!resp.IsSuccessfullyComplete)
                {
                    _logger.LogWarning("BODY failed for {Segment}", seg);
                    continue;                       // skip broken segments
                }

                foreach (string line in resp.Lines)
                    builder.Append(line);
            }

            /* 1 – undo the “special zip” escaping                              */
            string unescaped = SpotUtil.UnspecialZipStr(builder.ToString());

            /* 2 – optionally inflate                                           */
            if (!compressed)
                return unescaped;

            byte[] deflateBytes = Encoding.Latin1.GetBytes(unescaped);

            var decompressedString = await CompressionHelper.TryInflateAsync(deflateBytes, _logger, default);

            if (!string.IsNullOrWhiteSpace(decompressedString))
            {
                return decompressedString;
            }
            else
            {
                _logger.LogWarning("Deflate decompression failed; returning raw payload");
                return unescaped;
            }
        }


        public async Task<List<SpotPost>> RetrieveSpotPostsAsync()
        {
            var spotPosts = new List<SpotPost>();
            var client = new NntpClient()
            {
                Port = _settings.Port,
            };

            // Step 1: Connect to `free.pt` to retrieve spots
            var connectResult = await client.ConnectAsync(_settings.Host, _settings.Ssl);
            if (!connectResult.IsSuccessfullyComplete)
            {
                _logger.LogError("Could not connect to the Usenet server at {Host}", _settings.Host);
                throw new Exception("Could not connect to the Usenet server.");
            }

            var authResult = await client.AuthenticateUsernamePasswordAsync(_settings.Username, _settings.Password);
            if (!authResult.IsSuccessfullyComplete)
            {
                _logger.LogError("Usenet authentication failed for user {Username}", _settings.Username);
                throw new Exception("Usenet authentication failed.");
            }


            var groupResponse = await client.GetGroupAsync(UsenetConstants.HdrGroup);
            if (!groupResponse.IsSuccessfullyComplete)
            {
                _logger.LogError("Failed to select group {Group}", UsenetConstants.HdrGroup);
                throw new Exception($"Failed to select group {UsenetConstants.HdrGroup}.");
            }


            var messageId = "<Yd96rJhZDR0pIPJZwQyxI@spot.net>";
            var articleNumber = 4234898;


            var xoverFor4234898 = await client.XOverAsync(articleNumber, articleNumber);
            var roeroms06e05xover = xoverFor4234898.First();

            var head = await client.HeadAsync(messageId);

            var article2 = await client.ArticleAsync(messageId);


            var parsedFrom = XoverHeaderParser.ParseHeader(roeroms06e05xover.Subject, roeroms06e05xover.From, roeroms06e05xover.Date, roeroms06e05xover.MessageID);



            //SuperSpotnetHelper.ParseHeader("", headers.from)


            Console.WriteLine(article2.Message);

            //int lastArticle = groupResponse.HighWatermark;
            //int firstArticle = Math.Max(groupResponse.LowWatermark, lastArticle - 99); // Get the last 100 articles

            //_logger.LogInformation("Retrieving last 100 spots from {Group}, articles {First}-{Last}", UsenetConstants.HdrGroup, firstArticle, lastArticle);



            //var xoverResponse = await client.XOverAsync(firstArticle, lastArticle);


            //foreach (var header in xoverResponse)
            //{
            //    Console.WriteLine(header.Subject);

            //    var article = await client.ArticleAsync(header.ArticleNumber);
            //    var article2 = await client.ArticleAsync(header.MessageID);

            //}



            //// Use XOVER to fetch multiple article headers at once
            //var xoverResponse = client.Xover(new NntpArticleRange(firstArticle, lastArticle));
            //if (!xoverResponse.Success)
            //{
            //    _logger.LogWarning("XOVER failed in {Group}", UsenetConstants.HdrGroup);
            //    return spotPosts;
            //}

            ////Bug in library where you can not use foreach since it then overwrites other results for next Head queries
            //var allHeaders = xoverResponse.Lines.ToList();

            //var blahRegex = new Regex(@"<(\w*)@spot.net>", RegexOptions.Compiled);

            //foreach (var header in allHeaders)
            //{

            //    //string subject = article.Subject ?? string.Empty;
            //    //if (subject.Contains("Spot", StringComparison.OrdinalIgnoreCase))
            //    //{
            //    //    _logger.LogInformation("Found spot: {Subject}", subject);
            //    //    spotPosts.Add(new SpotPost
            //    //    {
            //    //        ArticleNumber = article.Number,
            //    //        Subject = subject,
            //    //        MessageId = article.MessageId
            //    //    });
            //    //}
            //}

            //// Step 2: Find the corresponding NZB files in `alt.binaries.ftd`
            //_logger.LogInformation("Now searching for NZB files in {Group}", UsenetConstants.NzbGroup);

            //var nzbGroupResponse = client.Group(UsenetConstants.NzbGroup);
            //if (!nzbGroupResponse.Success)
            //{
            //    _logger.LogError("Failed to select group {Group}", UsenetConstants.NzbGroup);
            //    throw new Exception($"Failed to select group {UsenetConstants.NzbGroup}.");
            //}

            //long nzbLastArticle = nzbGroupResponse.Group.HighWaterMark;
            //long nzbFirstArticle = Math.Max(nzbGroupResponse.Group.LowWaterMark, nzbLastArticle - 99);

            //var nzbXoverResponse = client.Xover(new NntpArticleRange(nzbFirstArticle, nzbLastArticle));
            //if (!nzbXoverResponse.Success)
            //{
            //    _logger.LogWarning("XOVER failed in {Group}", UsenetConstants.NzbGroup);
            //    return spotPosts;
            //}

            ////foreach (var article in nzbXoverResponse.Articles)
            ////{
            ////    if (article.Subject.Contains(".nzb", StringComparison.OrdinalIgnoreCase))
            ////    {
            ////        _logger.LogInformation("Found NZB file: {Subject}", article.Subject);
            ////        var spot = spotPosts.FirstOrDefault(s => article.Subject.Contains(s.Subject, StringComparison.OrdinalIgnoreCase));
            ////        if (spot != null)
            ////        {
            ////            spot.NzbMessageId = article.MessageId;
            ////        }
            ////    }
            ////}

            await client.DisconnectAsync();

            return spotPosts;
        }

        public async ValueTask DisposeAsync()
        {
            if (_nntpClient != null)
            {
                await _nntpClient.DisconnectAsync();
                _nntpClient = null;
            }
        }
    }
}
