using DeveSpotnet.Db.DbModels;
using SpotnetClient.SpotnetHelpers;
using System.Web;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeveSpotnet.SpotnetHelpers
{
    public static class SpotTemplateHelper
    {
        public static string MakeApiRequestString(string apiKey)
        {
            return $"&amp;apikey={apiKey}";
        }


        public static string MakeBaseUrl(string spotwebUrl, string type)
        {
            var uri = new Uri(spotwebUrl);
            return type == "path" ? uri.AbsolutePath : spotwebUrl.TrimEnd('/');
        }

        public static FormattedSpot FormatSpotHeader(this DbSpotHeader spot, string spotwebUrl)
        {
            var formatted = new FormattedSpot();

            // Base URL calculations
            var basePath = MakeBaseUrl(spotwebUrl, "path");
            var baseFull = MakeBaseUrl(spotwebUrl, "full");

            // SuperTodo: Implement these URL builders fully
            formatted.SabnzbdUrl = ""; // Would use MakeSabnzbdUrl()
            formatted.SearchUrl = BuildSearchUrl(spot);
            formatted.SpotUrl = $"{basePath}?page=getspot&messageid={HttpUtility.UrlEncode(spot.MessageID)}";
            formatted.CatUrl = BuildCatUrl(spot, basePath);
            formatted.SubCatUrl = BuildSubCatUrl(spot, basePath);
            formatted.PosterUrl = $"{basePath}?search[value][]=Poster:=:{HttpUtility.UrlEncode(spot.ParsedHeader_Poster)}";

            // Title sanitization
            formatted.Title = SanitizeTitle(spot.ParsedHeader_Title);
            formatted.Poster = SanitizePoster(spot.ParsedHeader_Poster);

            // Category info
            var hcat = spot.ParsedHeader_Category.GetValueOrDefault();
            formatted.CatShortDesc = SpotCategories.Cat2ShortDesc(hcat, spot.ParsedHeader_SubCatA);

            var subCatNumber = SpotCategories.SubcatNumberFromHeadcat(hcat);
            if (int.TryParse(subCatNumber, out var subCatNumberAsInt))
            {
                formatted.CatDesc = SpotCategories.Cat2Desc(hcat, spot.GetSubCatField(subCatNumberAsInt));
            }

            // Status flags
            //SuperTodo: Implement this
            //formatted.HasBeenDownloaded = spot.DownloadStamp.HasValue;
            //formatted.HasBeenSeen = spot.SeenStamp.HasValue;
            //formatted.IsBeingWatched = spot.WatchStamp.HasValue;

            return formatted;
        }

        private static string SanitizeTitle(string title)
        {
            if (string.IsNullOrEmpty(title)) return "";

            var decoded = HttpUtility.HtmlDecode(title);
            var encoded = HttpUtility.HtmlEncode(decoded);
            var stripped = RemoveExtensiveDots(encoded);
            return stripped;
        }

        private static string SanitizePoster(string poster)
        {
            if (string.IsNullOrEmpty(poster)) return "";
            return HttpUtility.HtmlEncode(poster);
        }

        private static string RemoveExtensiveDots(string input)
        {
            // Replace more than 3 consecutive dots with space
            return System.Text.RegularExpressions.Regex.Replace(input, @"\.{4,}", " ");
        }

        private static string BuildSearchUrl(DbSpotHeader spot)
        {
            // SuperTodo: Implement search engine preference logic
            var searchString = HttpUtility.UrlEncode(spot.ParsedHeader_Title);
            return $"https://www.binsearch.info/?q={searchString}";
        }

        private static string BuildCatUrl(DbSpotHeader spot, string basePath)
        {
            var subcat = spot.ParsedHeader_SubCatA?.TrimEnd('|') ?? "";
            return $"{basePath}?search[tree]=cat{spot.ParsedHeader_Category}_{subcat}";
        }

        private static string BuildSubCatUrl(DbSpotHeader spot, string basePath)
        {
            var subcatNumberFromHeadCat = SpotCategories.SubcatNumberFromHeadcat(spot.ParsedHeader_Category.GetValueOrDefault());

            if (int.TryParse(subcatNumberFromHeadCat, out var subcatNumberFromHeadCatAsInt))
            {
                var subcatField = spot.GetSubCatField(subcatNumberFromHeadCatAsInt);
                var subcatz = spot.ParsedHeader_SubCatZ?.Length > 1 ? "_z" + spot.ParsedHeader_SubCatZ[1] : "";
                return $"{basePath}?search[tree]=cat{spot.ParsedHeader_Category}{subcatz}_{subcatField}";
            }
            return "";
        }

        private static string GetSubCatField(this DbSpotHeader spot, int subcatNumber)
        {
            return subcatNumber switch
            {
                0 => spot.ParsedHeader_SubCatA,
                1 => spot.ParsedHeader_SubCatB,
                2 => spot.ParsedHeader_SubCatC,
                3 => spot.ParsedHeader_SubCatD,
                _ => spot.ParsedHeader_SubCatZ
            } ?? "";
        }
    }

    public class FormattedSpot
    {
        public string SabnzbdUrl { get; set; }
        public string SearchUrl { get; set; }
        public string SpotUrl { get; set; }
        public string CatUrl { get; set; }
        public string SubCatUrl { get; set; }
        public string PosterUrl { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public string CatShortDesc { get; set; }
        public string CatDesc { get; set; }
        public bool HasBeenDownloaded { get; set; }
        public bool HasBeenSeen { get; set; }
        public bool IsBeingWatched { get; set; }
    }
}
