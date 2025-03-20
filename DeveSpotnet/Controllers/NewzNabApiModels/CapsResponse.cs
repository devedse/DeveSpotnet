namespace DeveSpotnet.Controllers.NewzNabApiModels
{
    public class CapsResponse
    {
        /// <summary>
        /// Server-specific information.
        /// </summary>
        public ServerInfo Server { get; set; }

        /// <summary>
        /// The search limits (e.g. default results, max results).
        /// </summary>
        public LimitsInfo Limits { get; set; }

        /// <summary>
        /// Server retention in days (how long NZB information is stored).
        /// </summary>
        public int Retention { get; set; }

        /// <summary>
        /// List of searchable categories.
        /// </summary>
        public List<Category> Categories { get; set; } = new List<Category>();

        /// <summary>
        /// List of active, indexed usenet groups.
        /// </summary>
        public List<Group> Groups { get; set; } = new List<Group>();

        /// <summary>
        /// List of active genres.
        /// </summary>
        public List<Genre> Genres { get; set; } = new List<Genre>();
    }

    public class ServerInfo
    {
        /// <summary>
        /// The version of the protocol implemented by the server.
        /// All implementations should be backwards compatible.
        /// </summary>
        public string Version { get; set; }
    }

    public class LimitsInfo
    {
        /// <summary>
        /// The default number of search results returned.
        /// </summary>
        public int Default { get; set; }

        /// <summary>
        /// The maximum number of search results that can be returned.
        /// </summary>
        public int Max { get; set; }
    }

    public class Category
    {
        /// <summary>
        /// Unique category ID, can be either one of the standard category IDs or a site specific ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A descriptive name for the category. Can be site/language specific.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the contents of the category.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of subcategories.
        /// </summary>
        public List<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    }

    public class SubCategory
    {
        /// <summary>
        /// Unique subcategory ID, can be either one of the standard category IDs or a site specific ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A descriptive name for the subcategory. Can be site/language specific.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the contents of the subcategory.
        /// </summary>
        public string Description { get; set; }
    }

    public class Group
    {
        /// <summary>
        /// Name of the usenet group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the usenet group.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Date and time the usenet group was last updated.
        /// </summary>
        public DateTime LastUpdate { get; set; }
    }

    public class Genre
    {
        /// <summary>
        /// Id of the genre.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the genre.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The category ID the genre is associated with.
        /// </summary>
        public string CategoryId { get; set; }
    }
}
