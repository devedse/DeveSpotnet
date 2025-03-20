using System.Xml.Serialization;

namespace DeveSpotnet.Controllers.NewzNabApiModels
{
    // Root element for XML response
    [XmlRoot("rss")]
    public class NewznabRssResponse
    {
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns { get; set; } = new XmlSerializerNamespaces();

        [XmlAttribute("version")]
        public string Version { get; set; } = "2.0";

        [XmlElement("channel")]
        public NewznabChannel Channel { get; set; }

        public NewznabRssResponse()
        {
            // Correct way to declare XML namespaces
            Xmlns.Add("atom", "http://www.w3.org/2005/Atom");
            Xmlns.Add("newznab", "http://www.newznab.com/DTD/2010/feeds/attributes/");
        }
    }

    public class NewznabChannel
    {
        [XmlElement("atom:link")]
        public AtomLink AtomLink { get; set; }

        [XmlElement("title")]
        public string Title { get; set; } = "Spotweb Index";

        [XmlElement("description")]
        public string Description { get; set; } = "Spotweb Index API Results";

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("language")]
        public string Language { get; set; } = "en-gb";

        [XmlElement("webMaster")]
        public string WebMaster { get; set; }

        [XmlElement("category")]
        public string Category { get; set; } = "";

        [XmlElement("image")]
        public RssImage Image { get; set; }

        [XmlElement("newznab:response")]
        public NewznabResponse Response { get; set; }

        [XmlElement("item")]
        public List<NewznabItem> Items { get; set; } = new List<NewznabItem>();
    }

    public class AtomLink
    {
        [XmlAttribute("href")]
        public string Href { get; set; }

        [XmlAttribute("rel")]
        public string Rel { get; set; } = "self";

        [XmlAttribute("type")]
        public string Type { get; set; } = "application/rss+xml";
    }

    public class RssImage
    {
        [XmlElement("url")]
        public string Url { get; set; }

        [XmlElement("title")]
        public string Title { get; set; } = "Spotweb Index";

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("description")]
        public string Description { get; set; } = "SpotWeb Index API Results";
    }

    public class NewznabResponse
    {
        [XmlAttribute("offset")]
        public int Offset { get; set; }

        [XmlAttribute("total")]
        public int Total { get; set; }
    }

    public class NewznabItem
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("guid")]
        public NewznabGuid Guid { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("pubDate")]
        public string PubDate { get; set; }

        [XmlElement("category")]
        public string Category { get; set; }

        [XmlElement("enclosure")]
        public Enclosure Enclosure { get; set; }

        [XmlElement("newznab:attr")]
        public List<NewznabAttribute> Attributes { get; set; } = new List<NewznabAttribute>();

        [XmlElement("new")]
        public string IsNew { get; set; }

        [XmlElement("seen")]
        public string IsSeen { get; set; }

        [XmlElement("downloaded")]
        public string IsDownloaded { get; set; }
    }

    public class NewznabGuid
    {
        [XmlAttribute("isPermaLink")]
        public string IsPermaLink { get; set; } = "false";

        [XmlText]
        public string Value { get; set; }
    }

    public class Enclosure
    {
        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("length")]
        public long Length { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; } = "application/x-nzb";
    }

    public class NewznabAttribute
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }

    // Class for JSON response
    public class NewznabJsonItem
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string AddDate { get; set; }
        public string Guid { get; set; }
        public string FromName { get; set; }
        public int Completion { get; set; } = 100;
        public string CategoryID { get; set; }
        public int Comments { get; set; }
        public string CategoryName { get; set; }
        public string CategoryIds { get; set; }
        public int? TotalRows { get; set; }
    }
}
