using DeveSpotnet.Controllers.NewzNabApiControllerHelpers;
using DeveSpotnet.Controllers.NewzNabApiModels;
using DeveSpotnet.Db;
using DeveSpotnet.Db.DbModels;
using DeveSpotnet.Models;
using DeveSpotnet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace DeveSpotnet.Controllers
{
    [ApiController]
    [Route("api")]
    [ServiceFilter(typeof(OverrideAcceptHeaderFilter))] //Uses o=XML or o=JSON to override the accept header which ensures C# uses the right serializer
    public class NewzNabApiController : ControllerBase
    {
        private readonly IUsenetService _usenetService;
        private readonly DeveSpotnetDbContext _dbContext;

        public NewzNabApiController(IUsenetService usenetService, DeveSpotnetDbContext dbContext)
        {
            _usenetService = usenetService;
            _dbContext = dbContext;
        }

        [HttpGet]
        [QueryParameterConstraint("t", "caps")]
        [Produces("application/xml", "application/json")]
        public IActionResult Caps()
        {
            // Create a sample Caps response. In a real implementation, these values would be dynamic.
            var response = new CapsResponse
            {
                Server = new ServerInfo { Version = "1.0" },
                Limits = new LimitsInfo { Default = 100, Max = 100 },
                Retention = 30, // e.g., 30 days retention
                Categories =
                [
                    new Category
                    {
                        Id = "1000",
                        Name = "TV",
                        Description = "Television shows",
                        SubCategories =
                        [
                            new SubCategory { Id = "1010", Name = "HDTV", Description = "High Definition Television" },
                            new SubCategory { Id = "1020", Name = "SDTV", Description = "Standard Definition Television" }
                        ]
                    }
                ],
                Groups =
                [
                    new Group { Name = "alt.tv", Description = "General TV discussion", LastUpdate = DateTime.UtcNow }
                ],
                Genres =
                [
                    new Genre { Id = "1", Name = "Drama", CategoryId = "1000" },
                    new Genre { Id = "2", Name = "Comedy", CategoryId = "1000" }
                ]
            };

            return Ok(response);
            //return Content(capsXml.ToString(), "application/xml");
        }


        [HttpGet]
        [QueryParameterConstraint("t", "tvsearch")]
        [Produces("application/xml", "application/json")]
        public IActionResult TvSearch(
                    [FromQuery] string apikey,
                    [FromQuery] string? cat,
                    [FromQuery] string? season,
                    [FromQuery] string? q,
                    [FromQuery] string? rid,
                    [FromQuery] string? ep,
                    [FromQuery] int? maxage,
                    [FromQuery] string? attrs,
                    [FromQuery, ModelBinder(BinderType = typeof(BooleanBinder))] bool extended = false,
                    [FromQuery] int offset = 0,
                    [FromQuery] int limit = 100,
                    [FromQuery] bool del = false
            )
        {
            // Ensure the apikey is provided; otherwise, return 401 Unauthorized.
            if (string.IsNullOrWhiteSpace(apikey))
            {
                return Unauthorized("Missing required parameter 'apikey'.");
            }

            // In a real implementation, you would perform the TV search using the parameters.
            // For demonstration, we return a static TV search response using new C# 12 collection initializer syntax.
            var response = new TvSearchResponse
            {
                Rss = new TvSearchRss
                {
                    Version = "2.0",
                    XmlnsAtom = "http://www.w3.org/2005/Atom",
                    Channel = new TvSearchChannel
                    {
                        Title = "example.com",
                        Description = "example.com API results",
                        NewznabResponse = new NewznabResponse { Offset = offset, Total = 1234 },
                        Items = [
                            new TvSearchItem
                            {
                                Title = "A.Public.Domain.Tv.Show.S06E05",
                                Guid = "http://servername.com/rss/viewnzb/e9c515e02346086e3a477a5436d7bc8c",
                                Link = "http://servername.com/rss/nzb/e9c515e02346086e3a477a5436d7bc8c&i=1&r=18cf9f0a736041465e3bd521d00a90b9",
                                Comments = "http://servername.com/rss/viewnzb/e9c515e02346086e3a477a5436d7bc8c#comments",
                                PubDate = DateTime.UtcNow.ToString("r"),
                                Category = "TV > XviD",
                                Description = "Some TV show",
                                Enclosure = new Enclosure
                                {
                                    Url = "http://servername.com/rss/nzb/e9c515e02346086e3a477a5436d7bc8c&i=1&r=18cf9f0a736041465e3bd521d00a90b9",
                                    Length = 154653309,
                                    Type = "application/x-nzb"
                                },
                                Attributes = [
                                    new NewznabAttr { Name = "category", Value = "5030" },
                                    new NewznabAttr { Name = "size", Value = "154653309" },
                                    new NewznabAttr { Name = "season", Value = "3" },
                                    new NewznabAttr { Name = "episode", Value = "2" }
                                ]
                            },
                            new TvSearchItem
                            {
                                Title = "A.Public.Domain.Tv.Show.S06E05",
                                Guid = "http://servername.com/rss/viewnzb/e9c515e02346086e3a477a5436d7bc8c",
                                Link = "http://servername.com/rss/nzb/e9c515e02346086e3a477a5436d7bc8c&i=1&r=18cf9f0a736041465e3bd521d00a90b9",
                                Comments = "http://servername.com/rss/viewnzb/e9c515e02346086e3a477a5436d7bc8c#comments",
                                PubDate = DateTime.UtcNow.ToString("r"),
                                Category = "TV > XviD",
                                Description = "Some TV show",
                                Enclosure = new Enclosure
                                {
                                    Url = "http://servername.com/rss/nzb/e9c515e02346086e3a477a5436d7bc8c&i=1&r=18cf9f0a736041465e3bd521d00a90b9",
                                    Length = 4294967295,
                                    Type = "application/x-nzb"
                                },
                                Attributes = [
                                    new NewznabAttr { Name = "category", Value = "5000" },
                                    new NewznabAttr { Name = "category", Value = "5030" },
                                    new NewznabAttr { Name = "size", Value = "4294967295" },
                                    new NewznabAttr { Name = "season", Value = "3" },
                                    new NewznabAttr { Name = "episode", Value = "1" }
                                ]
                            }
                        ]
                    }
                }
            };

            return Ok(response);
        }
    }
}
