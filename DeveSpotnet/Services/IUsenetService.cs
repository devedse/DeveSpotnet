using DeveSpotnet.Models;

namespace DeveSpotnet.Services
{
    public interface IUsenetService
    {
        Task<object> ReadFullSpot(string messageId);
        Task<List<SpotPost>> RetrieveSpotPostsAsync();
    }
}
