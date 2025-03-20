﻿using DeveSpotnet.Configuration;
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


        private string GetBaseUrl(HttpContext httpContext)
        {
            var request = httpContext.Request;
            var scheme = request.Scheme;
            var host = request.Host.Value;
            return $"{scheme}://{host}";
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
                //return Unauthorized("Missing required parameter 'apikey'.");
            }

            var foundSpots = _dbContext.SpotHeaders.Where(t => t.ParsedHeader_Valid && t.ParsedHeader_Title != null && t.ParsedHeader_Title.Contains(q)).ToList();


            var baseUrl = GetBaseUrl(HttpContext);

            //SuperTodo: Load from config, but for now w/e
            var deveSpotnetSettings = new DeveSpotnetSettings();
            var blah = NewznabConverter2.ConvertToRss(deveSpotnetSettings, apikey, foundSpots, baseUrl, "supertodo@email.com", "supertodousername", extended, del, offset);

            return Ok(blah);
        }
    }
}
