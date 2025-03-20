//using DeveSpotnet.Controllers.NewzNabApiModels;
//using DeveSpotnet.Db.DbModels;
//using DeveSpotnet.SpotnetHelpers;
//using SpotnetClient.SpotnetHelpers;

//namespace DeveSpotnet.Services
//{

//    public static class NewznabConverter
//    {

//        public static NewznabRssResponse ConvertToRssResponse(List<DbSpotHeader> spots, int offset, bool extended, bool del, string baseUrl)
//        {
//            var apiUrl = $"{baseUrl}/api";
//            var imageUrl = $"{baseUrl}/images/spotnet.gif";

//            var response = new NewznabRssResponse
//            {
//                Channel = new NewznabChannel
//                {
//                    AtomLink = new AtomLink { Href = apiUrl },
//                    Link = baseUrl,
//                    WebMaster = "temp@tempemail.com",
//                    Image = new RssImage
//                    {
//                        Url = imageUrl,
//                        Link = baseUrl
//                    },
//                    Response = new NewznabResponse
//                    {
//                        Offset = offset,
//                        Total = spots.Count
//                    },
//                    Items = spots.Select(spot => ConvertToRssItem(spot, baseUrl, extended, del)).ToList()
//                }
//            };

//            return response;
//        }

//        public static List<NewznabJsonItem> ConvertToJsonResponse(List<DbSpotHeader> spots)
//        {
//            var result = new List<NewznabJsonItem>();
//            var totalRows = spots.Count;

//            foreach (var spot in spots)
//            {
//                var item = new NewznabJsonItem
//                {
//                    ID = spot.ParsedHeader_MessageId,
//                    Name = spot.ParsedHeader_Title,
//                    Size = spot.ParsedHeader_FileSize ?? 0,
//                    AddDate = spot.ParsedHeader_Stamp?.ToString("yyyy-MM-dd HH:mm:ss"),
//                    Guid = spot.ParsedHeader_MessageId,
//                    FromName = spot.ParsedHeader_Poster,
//                    Completion = 100
//                };

//                // Get category info
//                var (categoryId, categoryIds) = NewznabApiHelpers.Cat2NewznabCat(
//                    spot.ParsedHeader_Category ?? 0,
//                    spot.ParsedHeader_SubCatA,
//                    spot.ParsedHeader_SubCatZ);

//                item.CategoryID = categoryId;
//                item.CategoryIds = categoryIds;
//                item.CategoryName = $"{SpotCategories.HeadCat2Desc(spot.ParsedHeader_Category ?? 0)}: {Cat2ShortDesc(spot.ParsedHeader_Category ?? 0, spot.ParsedHeader_SubCatA)}";
//                item.Comments = 0; // You'll need to add comment count to your model or query

//                // Set the total rows only on the first item
//                if (result.Count == 0)
//                {
//                    item.TotalRows = totalRows;
//                }

//                result.Add(item);
//            }

//            return result;
//        }

//        private static NewznabItem ConvertToRssItem(DbSpotHeader spot, string baseUrl, bool extended, bool del)
//        {
//            var nzbUrl = $"{baseUrl}/api?t=g&id={spot.ParsedHeader_MessageId}";
//            if (del)
//            {
//                nzbUrl += "&del=1";
//            }

//            var item = new NewznabItem
//            {
//                Title = spot.ParsedHeader_Title,
//                Guid = new NewznabGuid { Value = spot.ParsedHeader_MessageId },
//                Link = nzbUrl,
//                PubDate = spot.ParsedHeader_Stamp?.ToString("r"),
//                Category = $"{SpotCategories.HeadCat2Desc(spot.ParsedHeader_Category ?? 0)} > {Cat2ShortDesc(spot.ParsedHeader_Category ?? 0, spot.ParsedHeader_SubCatA)}",
//                Enclosure = new Enclosure
//                {
//                    Url = nzbUrl,
//                    Length = spot.ParsedHeader_FileSize ?? 0,
//                    Type = "application/x-nzb" // You might want to make this configurable
//                },
//                Attributes = new List<NewznabAttribute>(),
//                IsNew = "false", // You'll need to implement IsSpotNew method
//                IsSeen = "false", // You'll need to implement this
//                IsDownloaded = "false" // You'll need to implement this
//            };

//            // Add category attributes
//            var (categoryId, categoryIds) = Cat2NewznabCat(
//                spot.ParsedHeader_Category ?? 0,
//                spot.ParsedHeader_SubCatA,
//                spot.ParsedHeader_SubCatZ);

//            if (!string.IsNullOrEmpty(categoryId))
//            {
//                var categoryParts = categoryIds.Split(',');
//                if (categoryParts.Length > 0)
//                {
//                    item.Attributes.Add(new NewznabAttribute { Name = "category", Value = categoryParts[0] });
//                }
//                if (categoryParts.Length > 1)
//                {
//                    item.Attributes.Add(new NewznabAttribute { Name = "category", Value = categoryParts[1] });
//                }
//            }

//            // Add size attribute
//            item.Attributes.Add(new NewznabAttribute { Name = "size", Value = (spot.ParsedHeader_FileSize ?? 0).ToString() });

//            // Add extended attributes if requested
//            if (extended)
//            {
//                item.Attributes.Add(new NewznabAttribute
//                {
//                    Name = "poster",
//                    Value = $"{spot.ParsedHeader_Poster}@spot.net"
//                });

//                item.Attributes.Add(new NewznabAttribute
//                {
//                    Name = "comments",
//                    Value = "0" // You'll need to add comment count to your model or query 
//                });
//            }

//            // Handle subtitles based on SubCatC
//            if (!string.IsNullOrEmpty(spot.ParsedHeader_SubCatC))
//            {
//                var subs = GetSubtitleLanguages(spot.ParsedHeader_SubCatC);
//                if (subs.Any())
//                {
//                    item.Attributes.Add(new NewznabAttribute
//                    {
//                        Name = "subs",
//                        Value = string.Join(",", subs)
//                    });
//                }
//            }

//            return item;
//        }

//        private (string categoryId, string categoryIds) Cat2NewznabCat(int category, string subCatA, string subCatZ)
//        {
//            // This is a simplified mapping implementation for Newznab categories
//            // You'll need to expand this based on your specific category system
//            switch (category)
//            {
//                case 0: // Image
//                    return ("1000", "1000,1010");
//                case 1: // Sound
//                    return ("3000", "3000,3010");
//                case 2: // Games
//                    return ("1000", "1000,1020");
//                case 3: // Applications
//                    return ("4000", "4000,4010");
//                default:
//                    return (string.Empty, string.Empty);
//            }
//        }

//        private string Cat2ShortDesc(int category, string subCatA)
//        {
//            // This is a simplified implementation. You might need more complex logic here
//            // based on how your SpotCategories class is used.
//            return subCatA ?? "Unknown";
//        }

//        private List<string> GetSubtitleLanguages(string subCatC)
//        {
//            var result = new List<string>();
//            if (string.IsNullOrEmpty(subCatC))
//            {
//                return result;
//            }

//            var subcats = subCatC.Split(',');

//            // Dutch subtitles
//            if (subcats.Contains("c2") || subcats.Contains("c1") || subcats.Contains("c6"))
//            {
//                result.Add("dutch");
//            }

//            // English subtitles
//            if (subcats.Contains("c3") || subcats.Contains("c4") || subcats.Contains("c7"))
//            {
//                result.Add("english");
//            }

//            return result;
//        }
//    }
//}
