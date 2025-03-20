using DeveSpotnet.Configuration;
using DeveSpotnet.Db;
using DeveSpotnet.Db.DbModels;
using DeveSpotnet.Models;
using DeveSpotnet.SpotnetHelpers;
using mcnntp.common.client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeveSpotnet.HostedServices
{
    public class SpotRetrieverHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IOptions<UsenetSettings> _settings;
        private readonly ILogger<SpotRetrieverHostedService> _logger;
        private const int BatchSize = 5000;

        public SpotRetrieverHostedService(
            IServiceScopeFactory scopeFactory,
            IOptions<UsenetSettings> settings,
            ILogger<SpotRetrieverHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _settings = settings;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            NntpClient? client = null;
            try
            {
                // Connect and authenticate once before entering the processing loop.
                client = await ConnectClientAsync(stoppingToken);
                if (client == null)
                {
                    _logger.LogError("Initial connection to the NNTP server failed.");
                    return;
                }

                while (!stoppingToken.IsCancellationRequested)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<DeveSpotnetDbContext>();

                    // Process a batch of headers.
                    var itemsProcessed = await ProcessBatchAsync(client, dbContext, stoppingToken);

                    if (itemsProcessed == 0)
                    {
                        _logger.LogInformation("No new headers were found, waiting before checking again.");

                        // No new headers were found, wait before checking again.
                        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the hosted service.");
            }
            finally
            {
                if (client != null)
                {
                    await client.DisconnectAsync();
                }
            }
        }

        /// <summary>
        /// Connects to the NNTP server and authenticates.
        /// </summary>
        private async Task<NntpClient?> ConnectClientAsync(CancellationToken token)
        {
            var client = new NntpClient
            {
                Port = _settings.Value.Port
            };

            var connectResult = await client.ConnectAsync(_settings.Value.Host, _settings.Value.Ssl);
            if (!connectResult.IsSuccessfullyComplete)
            {
                _logger.LogError("Could not connect to the Usenet server at {Host}", _settings.Value.Host);
                return null;
            }

            var authResult = await client.AuthenticateUsernamePasswordAsync(_settings.Value.Username, _settings.Value.Password);
            if (!authResult.IsSuccessfullyComplete)
            {
                _logger.LogError("Usenet authentication failed for user {Username}", _settings.Value.Username);
                return null;
            }

            _logger.LogInformation("Connected and authenticated to NNTP server {Host}", _settings.Value.Host);
            return client;
        }

        /// <summary>
        /// Retrieves and processes a batch of article headers.
        /// </summary>
        private async Task<int> ProcessBatchAsync(NntpClient client, DeveSpotnetDbContext dbContext, CancellationToken token)
        {
            // Retrieve the group details.
            var groupResponse = await client.GetGroupAsync(UsenetConstants.HdrGroup);
            if (!groupResponse.IsSuccessfullyComplete)
            {
                throw new Exception($"Failed to select group {UsenetConstants.HdrGroup}.");
            }

            int groupLow = groupResponse.LowWatermark;
            int groupHigh = groupResponse.HighWatermark;

            // Determine the starting article number based on what is already stored.
            int lastStoredArticle = await dbContext.SpotHeaders.MaxAsync(h => (int?)h.ArticleNumber, token) ?? 0;
            int startArticle = lastStoredArticle > 0 ? lastStoredArticle + 1 : groupLow;

            if (startArticle > groupHigh)
            {
                _logger.LogInformation("No new articles available. Current stored article: {Article}, group high watermark: {High}",
                    startArticle, groupHigh);
                return 0;
            }

            // Determine the end article for this batch.
            int endArticle = Math.Min(startArticle + BatchSize - 1, groupHigh);

            // Calculate remaining items in the group and log that info while obtaining headers.
            int remainingItems = groupHigh - endArticle;
            _logger.LogInformation("Retrieving articles from {StartArticle} to {EndArticle}. {Remaining} more items remain to be processed.",
                startArticle, endArticle, remainingItems);

            // Retrieve headers for the given range.
            var xoverResponse = await client.XOverAsync(startArticle, endArticle);
            if (xoverResponse == null || xoverResponse.Count == 0)
            {
                _logger.LogInformation("No headers found in range {StartArticle} to {EndArticle}.", startArticle, endArticle);
                return 0;
            }

            // Map NNTP headers to database entities.
            var headersToAdd = new List<DbSpotHeader>();
            int countValid = 0;
            foreach (var header in xoverResponse)
            {
                ParsedHeader? parsedHeader = null;
                var trimmedMessageId = header.MessageID?.TrimStart('<').TrimEnd('>');
                try
                {
                    parsedHeader = SuperSpotnetHelper.ParseHeader(header.Subject, header.From, header.Date, trimmedMessageId);
                    if (parsedHeader != null)
                    {
                        countValid++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "Error parsing header for article {ArticleNumber}", header.ArticleNumber);
                }
                var dbHeader = new DbSpotHeader
                {
                    ArticleNumber = header.ArticleNumber,
                    Subject = header.Subject,
                    From = header.From,
                    Date = header.Date,
                    MessageID = trimmedMessageId,
                    References = header.References,
                    Bytes = header.Bytes,
                    Lines = header.Lines,
                    Code = header.Code,
                    Message = header.Message,

                    ParsedHeader_Valid = parsedHeader != null,
                    ParsedHeader_Header = parsedHeader?.Header,
                    ParsedHeader_SelfSignedPubKey = parsedHeader?.SelfSignedPubKey,
                    ParsedHeader_UserSignature = parsedHeader?.UserSignature,
                    ParsedHeader_Verified = parsedHeader?.Verified,
                    ParsedHeader_FileSize = parsedHeader?.FileSize,
                    ParsedHeader_MessageId = parsedHeader?.MessageId,
                    ParsedHeader_Stamp = parsedHeader?.Stamp,
                    ParsedHeader_Poster = parsedHeader?.Poster,
                    ParsedHeader_Category = parsedHeader?.Category,
                    ParsedHeader_KeyId = parsedHeader?.KeyId,
                    ParsedHeader_SubCatA = parsedHeader?.SubCatA,
                    ParsedHeader_SubCatB = parsedHeader?.SubCatB,
                    ParsedHeader_SubCatC = parsedHeader?.SubCatC,
                    ParsedHeader_SubCatD = parsedHeader?.SubCatD,
                    ParsedHeader_SubCatZ = parsedHeader?.SubCatZ,
                    ParsedHeader_WasSigned = parsedHeader?.WasSigned,
                    ParsedHeader_SpotterId = parsedHeader?.SpotterId,
                    ParsedHeader_Title = parsedHeader?.Title,
                    ParsedHeader_Tag = parsedHeader?.Tag,
                    ParsedHeader_HeaderSign = parsedHeader?.HeaderSign,
                    ParsedHeader_UserKey_Modulo = parsedHeader?.UserKey?.Modulo,
                    ParsedHeader_UserKey_Exponent = parsedHeader?.UserKey?.Exponent,
                    ParsedHeader_XmlSignature = parsedHeader?.XmlSignature
                };
                headersToAdd.Add(dbHeader);
            }

            // Save the new headers to the database.
            dbContext.SpotHeaders.AddRange(headersToAdd);

            var testje = headersToAdd.Where(t => t.ParsedHeader_Stamp != null).OrderBy(t => t.ParsedHeader_Stamp).ToList();
            var lastProccessedTimeStampString = headersToAdd.Any() ? headersToAdd.Max(h => h.ParsedHeader_Stamp).ToString() : "N/A";

            await dbContext.SaveChangesAsync(token);
            _logger.LogInformation("Saved {Count} headers to the database. {Valid} headers were successfully parsed, {Invalid} where invalid. Last processed TimeStamp: {LastTimeStamp}",
                headersToAdd.Count,
                countValid,
                headersToAdd.Count - countValid,
                lastProccessedTimeStampString);

            return headersToAdd.Count;
        }

    }
}
