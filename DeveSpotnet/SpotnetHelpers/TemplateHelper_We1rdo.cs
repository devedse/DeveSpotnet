using DeveSpotnet.Controllers.NewzNabApiModels;
using DeveSpotnet.Db.DbModels;

namespace DeveSpotnet.SpotnetHelpers
{
    public static class TemplateHelper_We1rdo
    {
        // SuperTodo: Implement proper localization mechanism
        private static string _(string text) => text;

        public static Dictionary<string, Dictionary<string, int>> GetTemplatePreferences()
        {
            return new Dictionary<string, Dictionary<string, int>>
            {
                ["we1rdo"] = new Dictionary<string, int> { ["example_setting"] = 1 }
            };
        }

        public static string Cat2CssClass(DbSpotHeader spot)
        {
            var categoryCss = $"spotcat{spot.ParsedHeader_Category}";
            if (!string.IsNullOrEmpty(spot.ParsedHeader_SubCatZ))
            {
                var subcatz = spot.ParsedHeader_SubCatZ;
                if (subcatz.Length > 0)
                {
                    categoryCss += $" spotcat{spot.ParsedHeader_Category}_{subcatz.TrimEnd('|')}";
                }
            }
            return categoryCss;
        }

        public static string Filter2Cat(string s)
        {
            if (s.Contains("cat0", StringComparison.OrdinalIgnoreCase)) return "spotcat0";
            if (s.Contains("cat1", StringComparison.OrdinalIgnoreCase)) return "spotcat1";
            if (s.Contains("cat2", StringComparison.OrdinalIgnoreCase)) return "spotcat2";
            if (s.Contains("cat3", StringComparison.OrdinalIgnoreCase)) return "spotcat3";
            return "N/A";
        }

        public static Dictionary<string, string> GetFilterIcons()
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["application"] = _("Application"),
                ["bluray"] = _("Blu-Ray"),
                ["book"] = _("Book"),
                // ... rest of the entries
                ["dvd"] = _("DVD"),
                ["pda"] = _("PDA")
            };
        }

        public static Dictionary<string, string> GetSmileyList()
        {
            return new Dictionary<string, string>
            {
                ["biggrin"] = "templates/we1rdo/smileys/biggrin.gif",
                ["bloos"] = "templates/we1rdo/smileys/bloos.gif",
                // ... all other smiley entries
                ["wink"] = "templates/we1rdo/smileys/wink.gif"
            };
        }

        public static List<string> GetStaticFiles(string type)
        {
            switch (type.ToLower())
            {
                case "js":
                    return new List<string>
                {
                    "js/jquery/jquery.min.js",
                    // ... all JS files
                    "templates/we1rdo/js/jquery.tipTip.minified.js"
                };

                case "css":
                    return new List<string>
                {
                    "js/dynatree/skin-vista/ui.dynatree.css",
                    // ... all CSS files
                    "templates/we1rdo/css/tipTip.css"
                };

                case "ico":
                    return new List<string> { "images/favicon.ico" };

                default:
                    return new List<string>();
            }
        }
    }
}
