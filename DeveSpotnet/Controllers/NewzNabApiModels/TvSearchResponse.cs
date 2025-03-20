namespace DeveSpotnet.Controllers.NewzNabApiModels
{
    public class TvSearchResponse
    {
        /// <summary>
        /// The top-level RSS response.
        /// </summary>
        public TvSearchRss Rss { get; set; }
    }

    public class TvSearchRss
    {
        /// <summary>
        /// RSS version (should be "2.0").
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// XML namespace for Atom.
        /// </summary>
        public string XmlnsAtom { get; set; }

        /// <summary>
        /// The channel element.
        /// </summary>
        public TvSearchChannel Channel { get; set; }
    }

    public class TvSearchChannel
    {
        /// <summary>
        /// Title of the channel.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the channel.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Newznab-specific response element that indicates offset and total.
        /// </summary>
        public NewznabResponse NewznabResponse { get; set; }

        /// <summary>
        /// List of TV search items.
        /// </summary>
        public List<TvSearchItem> Items { get; set; } = new List<TvSearchItem>();
    }

    public class NewznabResponse
    {
        /// <summary>
        /// The current offset of the response.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// The total number of items found by the query.
        /// </summary>
        public int Total { get; set; }
    }

    public class TvSearchItem
    {
        // Standard RSS 2.0 elements

        /// <summary>
        /// Title of the item.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Globally unique identifier.
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// URL link to the item.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// URL for comments.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Publication date.
        /// </summary>
        public string PubDate { get; set; }

        /// <summary>
        /// Category string, e.g., "TV > XviD".
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Description of the item.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Enclosure element details.
        /// </summary>
        public Enclosure Enclosure { get; set; }

        /// <summary>
        /// Additional attributes (represented by newznab:attr elements).
        /// </summary>
        public List<NewznabAttr> Attributes { get; set; } = new List<NewznabAttr>();
    }

    public class Enclosure
    {
        /// <summary>
        /// URL of the enclosure.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Length (in bytes) of the enclosure.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// MIME type of the enclosure.
        /// </summary>
        public string Type { get; set; }
    }

    public class NewznabAttr
    {
        /// <summary>
        /// Name of the attribute.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value of the attribute.
        /// </summary>
        public string Value { get; set; }
    }
}
