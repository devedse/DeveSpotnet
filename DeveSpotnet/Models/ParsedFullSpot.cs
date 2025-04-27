namespace DeveSpotnet.Models
{
    /// <summary>
    /// Mirrors the PHP $tpl_spot array returned by Services_Format_Parsing::parseFull()
    /// </summary>
    public sealed class ParsedFullSpot
    {
        public int Category { get; set; }
        public string Website { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public ImageMeta? ImageInfo { get; set; }

        public string SabnzbdUrl { get; set; } = string.Empty;   // (not set here)
        public string MessageId { get; set; } = string.Empty;   // (supply later)
        public string SearchUrl { get; set; } = string.Empty;   // (not set here)

        public string Description { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string Poster { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;
        public string NewsGroup { get; set; } = string.Empty;

        public string SubCatA { get; set; } = string.Empty;
        public string SubCatB { get; set; } = string.Empty;
        public string SubCatC { get; set; } = string.Empty;
        public string SubCatD { get; set; } = string.Empty;
        public string SubCatZ { get; set; } = string.Empty;

        public string Created { get; set; } = string.Empty;   // raw string keeps original format
        public int Key { get; set; }

        public List<string> NzbSegments { get; set; } = new();
        public List<string> PrevMsgIds { get; set; } = new();

        public string Newsreader { get; set; } = string.Empty;

        /* ..................... helper DTO ................................. */
        public sealed class ImageMeta
        {
            public int Height { get; set; }
            public int Width { get; set; }
            public List<string> Segments { get; set; } = new();
        }
    }
}
