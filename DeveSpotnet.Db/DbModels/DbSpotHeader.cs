using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveSpotnet.Db.DbModels
{
    public class DbSpotHeader
    {
        [Key]
        public int ArticleNumber { get; set; }

        public string? Subject { get; set; }

        public string? From { get; set; }

        public string? Date { get; set; }

        public string? MessageID { get; set; }

        public string? References { get; set; }

        public int Bytes { get; set; }

        public int Lines { get; set; }

        public int Code { get; set; }

        public string? Message { get; set; }
    }
}
