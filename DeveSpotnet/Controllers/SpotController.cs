using DeveSpotnet.Models;
using DeveSpotnet.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeveSpotnet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpotController : ControllerBase
    {
        private readonly IUsenetService _usenetService;

        public SpotController(IUsenetService usenetService)
        {
            _usenetService = usenetService;
        }

        [HttpGet("retrieve")]
        public async Task<ActionResult<List<SpotPost>>> Retrieve()
        {
            var posts = await _usenetService.RetrieveSpotPostsAsync();
            return Ok(posts);
        }
    }
}
