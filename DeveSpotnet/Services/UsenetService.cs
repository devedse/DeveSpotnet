using DeveSpotnet.Configuration;
using DeveSpotnet.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using System.Text;
using DeveSpotnet.SpotnetHelpers;
using System.Reflection.PortableExecutable;
using mcnntp.common.client;
using mcnntp.common;
using DeveSpotnet.SpotnetHelpers.Parsers;

namespace DeveSpotnet.Services
{
    public class UsenetService : IUsenetService
    {
        private static readonly object _lockObject = new object();
        private readonly UsenetSettings _settings;
        private readonly ILogger<UsenetService> _logger;

        public UsenetService(IOptions<UsenetSettings> options, ILogger<UsenetService> logger)
        {
            _settings = options.Value;
            _logger = logger;
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

        public async Task<object> ReadFullSpot(string messageId)
        {
            NntpClient? client = null;
            try
            {
                client = await ConnectClientAsync(CancellationToken.None);

                var header = await client.HeadAsync($"<{messageId}>");

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
                //var parsedHeader = SuperSpotnetHelper.ParseHeader(header.Subject, header.From, header.Date, trimmedMessageId);
            }
            finally
            {
                if (client != null)
                {
                    await client.DisconnectAsync();
                }
            }
            return null;
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

    }
}
