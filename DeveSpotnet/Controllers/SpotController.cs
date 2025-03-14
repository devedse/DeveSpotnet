using DeveSpotnet.Db;
using DeveSpotnet.Db.DbModels;
using DeveSpotnet.Models;
using DeveSpotnet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeveSpotnet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpotController : ControllerBase
    {
        private readonly IUsenetService _usenetService;
        private readonly DeveSpotnetDbContext _dbContext;

        public SpotController(IUsenetService usenetService, DeveSpotnetDbContext dbContext)
        {
            _usenetService = usenetService;
            _dbContext = dbContext;
        }

        //Search for spot by title
        [HttpGet]
        public async Task<ActionResult<List<DbSpotHeader>>> Search(string title)
        {

            var result = await _dbContext.SpotHeaders.Where(x => EF.Functions.Like(x.Subject, title)).ToListAsync();
            return Ok(result);
        }

        [HttpGet("search2")]
        public async Task<ActionResult<List<DbSpotHeader>>> Search2(string title)
        {
            // Prepare the search pattern (add wildcards if needed)
            var pattern = $"%{title}%";

            // Start with a queryable on the SpotHeaders
            var query = _dbContext.SpotHeaders.AsQueryable();

            // Check the provider name and conditionally use ILike or Like
            if (_dbContext.Database.ProviderName == "Npgsql.EntityFrameworkCore.PostgreSQL")
            {
                // Use case-insensitive ILike for PostgreSQL
                query = query.Where(x => EF.Functions.ILike(x.Subject, pattern));
            }
            else
            {
                // Use standard Like for other providers (e.g., SQLite)
                query = query.Where(x => EF.Functions.Like(x.Subject, pattern));
            }

            var result = await query.ToListAsync();
            return Ok(result);
        }

        [HttpGet("retrieve")]
        public async Task<ActionResult<List<SpotPost>>> Retrieve()
        {
            var posts = await _usenetService.RetrieveSpotPostsAsync();
            return Ok(posts);
        }
    }
}
