using DeveSpotnet.Models;

namespace DeveSpotnet.Services
{
    public interface IUsenetService
    {
        Task<string> FetchNzbAsync(ParsedFullSpot spot, CancellationToken ct = default);
        Task<string> ReadBinaryAsync(IEnumerable<string> segmentList, bool compressed, CancellationToken ct = default);
        Task<ParsedFullSpot?> ReadFullSpot(string messageId);
        Task<List<SpotPost>> RetrieveSpotPostsAsync();
    }
}
