using DeveSpotnet.Models;

namespace DeveSpotnet.Services
{
    public interface IUsenetService
    {
        Task<List<SpotPost>> RetrieveSpotPostsAsync();
    }
}
