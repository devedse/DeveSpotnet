using DeveSpotnet.Configuration;
using DeveSpotnet.Controllers.NewzNabApiModels;
using DeveSpotnet.Db.DbModels;
using DeveSpotnet.SpotnetHelpers;
using SpotnetClient.SpotnetHelpers;
using System.Globalization;
using System.Net;
using System.Web;

namespace DeveSpotnet.Services
{
    public static class NewznabConverter2
    {
        //public static List<NewznabJsonItem> ConvertToJson(List<DbSpotHeader> spots, int totalRows)
        //{
        //    var jsonItems = new List<NewznabJsonItem>();
        //    bool isFirst = true;

        //    foreach (var spot in spots)
        //    {
        //        var formattedSpot = SpotTemplateHelper.FormatSpotHeader(spot, ;
        //        var hcat = spot.ParsedHeader_Category.GetValueOrDefault();
        //        var subcata = spot.ParsedHeader_SubCatA ?? string.Empty;

        //        var nabCatResult = NewznabApiHelpers.Cat2NewznabCat(hcat, subcata, spot.ParsedHeader_SubCatZ ?? string.Empty);
        //        var nabCatParts = nabCatResult.Split('|');

        //        var jsonItem = new NewznabJsonItem
        //        {
        //            ID = spot.MessageID ?? string.Empty,
        //            Name = formattedSpot.Title,
        //            Size = spot.ParsedHeader_FileSize.GetValueOrDefault(),
        //            AddDate = spot.ParsedHeader_Stamp?.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) ?? string.Empty,
        //            Guid = spot.MessageID ?? string.Empty,
        //            FromName = formattedSpot.Poster,
        //            Completion = 100,
        //            CategoryID = nabCatParts.Length > 0 ? nabCatParts[0] : string.Empty,
        //            CategoryName = $"{SpotCategories.HeadCat2Desc(hcat)}: {SpotCategories.Cat2ShortDesc(hcat, subcata)}",
        //            CategoryIds = string.Join(",", nabCatParts.Where(p => !string.IsNullOrEmpty(p))),
        //            Comments = spot.CommentCount,
        //            TotalRows = isFirst ? totalRows : (int?)null
        //        };

        //        if (isFirst) isFirst = false;
        //        jsonItems.Add(jsonItem);
        //    }

        //    return jsonItems;
        //}

        public static NewznabRssResponse ConvertToRss(
            DeveSpotnetSettings deveSpotnetSettings,
            string apiKey,
            List<DbSpotHeader> spots,
            string spotwebUrl,
            string webMasterEmail,
            string userName,
            bool extendedMode,
            bool includeDelParam,
            int offset)
        {
            if (!spotwebUrl.EndsWith('/'))
            {
                spotwebUrl += '/';
            }

            var rssResponse = new NewznabRssResponse
            {
                Version = "2.0",
                Channel = new NewznabChannel
                {
                    AtomLink = new AtomLink
                    {
                        Href = $"{spotwebUrl}api",
                        Rel = "self",
                        Type = "application/rss+xml"
                    },
                    Title = "Spotweb Index",
                    Description = "Spotweb Index API Results",
                    Link = spotwebUrl,
                    Language = "en-gb",
                    WebMaster = $"{webMasterEmail} ({userName})",
                    Category = string.Empty,
                    Image = new RssImage
                    {
                        Url = $"{spotwebUrl}images/spotnet.gif",
                        Title = "Spotweb Index",
                        Link = spotwebUrl,
                        Description = "SpotWeb Index API Results"
                    },
                    Response = new NewznabResponse
                    {
                        Offset = offset,
                        Total = spots.Count
                    },
                    Items = new List<NewznabItem>()
                }
            };

            foreach (var spot in spots)
            {
                var formattedSpot = SpotTemplateHelper.FormatSpotHeader(spot, spotwebUrl);
                var hcat = spot.ParsedHeader_Category.GetValueOrDefault();
                var subcata = spot.ParsedHeader_SubCatA ?? string.Empty;


                //   $nzbUrl = $this->_tplHelper->makeBaseUrl('full').'api?t=g&amp;id='.$spot['messageid'].$this->_tplHelper->makeApiRequestString();
                //                if ($this->_params['del'] == '1' && $this->_spotSec->allowed(SpotSecurity::spotsec_keep_own_watchlist, '')) {
                //                    $nzbUrl .= '&amp;del=1';
                //                } // if
                // Generate NZB URL

                //Convert php codde above to this:
                var nzbUrl = SpotTemplateHelper.MakeBaseUrl(spotwebUrl, "full") + "api?t=g&id=" + spot.MessageID + SpotTemplateHelper.MakeApiRequestString(apiKey);



                var item = new NewznabItem
                {
                    Title = WebUtility.HtmlEncode(formattedSpot.Title),
                    Guid = new NewznabGuid
                    {
                        IsPermaLink = "false",
                        Value = spot.MessageID ?? string.Empty
                    },
                    Link = nzbUrl,
                    PubDate = spot.ParsedHeader_Stamp?.ToString("r", CultureInfo.InvariantCulture) ?? string.Empty,
                    Category = $"{SpotCategories.HeadCat2Desc(hcat)} > {SpotCategories.Cat2ShortDesc(hcat, subcata)}",
                    Enclosure = new Enclosure
                    {
                        Url = nzbUrl,
                        Length = spot.ParsedHeader_FileSize.GetValueOrDefault(),
                        Type = deveSpotnetSettings.NzbHandling.NzbPrepareAction == "zip" ? "application/zip" : "application/x-nzb"
                    },
                    Attributes = new List<NewznabAttribute>(),
                    IsNew = false.ToString().ToLower(), //SuperTodo: Don't care about New for now
                    IsSeen = formattedSpot.HasBeenSeen.ToString().ToLower(),
                    IsDownloaded = formattedSpot.HasBeenDownloaded.ToString().ToLower()
                };

                // Category attributes
                var nabCatResult = NewznabApiHelpers.Cat2NewznabCat(hcat, subcata, spot.ParsedHeader_SubCatZ ?? string.Empty);
                var nabCatParts = nabCatResult.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var catPart in nabCatParts)
                {
                    item.Attributes.Add(new NewznabAttribute
                    {
                        Name = "category",
                        Value = catPart
                    });
                }

                // Subtitle languages
                if (!string.IsNullOrEmpty(spot.ParsedHeader_SubCatC))
                {
                    var subcatcParts = spot.ParsedHeader_SubCatC.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    var subs = new List<string>();

                    if (subcatcParts.Intersect(new[] { "c2", "c1", "c6" }).Any())
                        subs.Add("dutch");

                    if (subcatcParts.Intersect(new[] { "c3", "c4", "c7" }).Any())
                        subs.Add("english");

                    if (subs.Count > 0)
                    {
                        item.Attributes.Add(new NewznabAttribute
                        {
                            Name = "subs",
                            Value = string.Join(",", subs)
                        });
                    }
                }

                // Size attribute
                item.Attributes.Add(new NewznabAttribute
                {
                    Name = "size",
                    Value = spot.ParsedHeader_FileSize.ToString()
                });

                // Extended attributes
                if (extendedMode)
                {
                    item.Attributes.Add(new NewznabAttribute
                    {
                        Name = "poster",
                        Value = $"{formattedSpot.Poster}@spot.net"
                    });

                    //SuperTodo implement later
                    //item.Attributes.Add(new NewznabAttribute
                    //{
                    //    Name = "comments",
                    //    Value = spot.CommentCount.ToString()
                    //});
                }

                rssResponse.Channel.Items.Add(item);
            }

            return rssResponse;
        }
    }
}