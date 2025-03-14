using DeveSpotnet.Configuration;
using DeveSpotnet.Db;
using DeveSpotnet.Db.DbModels;
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
                    await ProcessBatchAsync(client, dbContext, stoppingToken);

                    // Wait before processing the next batch.
                    //await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
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
        private async Task ProcessBatchAsync(NntpClient client, DeveSpotnetDbContext dbContext, CancellationToken token)
        {
            // Retrieve the group details.
            var groupResponse = await client.GetGroupAsync(UsenetConstants.HdrGroup);
            if (!groupResponse.IsSuccessfullyComplete)
            {
                _logger.LogError("Failed to select group {Group}", UsenetConstants.HdrGroup);
                return;
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
                return;
            }

            // Determine the end article for this batch.
            int endArticle = Math.Min(startArticle + BatchSize - 1, groupHigh);
            _logger.LogInformation("Retrieving articles from {StartArticle} to {EndArticle}", startArticle, endArticle);

            // Retrieve headers for the given range.
            var xoverResponse = await client.XOverAsync(startArticle, endArticle);
            if (xoverResponse == null || xoverResponse.Count == 0)
            {
                _logger.LogInformation("No headers found in range {StartArticle} to {EndArticle}.", startArticle, endArticle);
                return;
            }

            // Map NNTP headers to database entities.
            var headersToAdd = new List<DbSpotHeader>();
            foreach (var header in xoverResponse)
            {
                var dbHeader = new DbSpotHeader
                {
                    ArticleNumber = header.ArticleNumber,
                    Subject = header.Subject,
                    From = header.From,
                    Date = header.Date,
                    MessageID = header.MessageID,
                    References = header.References,
                    Bytes = header.Bytes,
                    Lines = header.Lines,
                    Code = header.Code,
                    Message = header.Message
                };
                headersToAdd.Add(dbHeader);
            }

            // Save the new headers to the database.
            dbContext.SpotHeaders.AddRange(headersToAdd);
            await dbContext.SaveChangesAsync(token);
            _logger.LogInformation("Saved {Count} headers to the database.", headersToAdd.Count);
        }
    }
}
