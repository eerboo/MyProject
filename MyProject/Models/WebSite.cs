using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Models
{
    public class WebSite
    {
        public int ID { get; set; }
        public string SiteName { get; set; }
        public string SiteAddress { get; set; }
        public string TitleMask { get; set; }
        public string PostDateMask { get; set; }
        public string PostBodyMask { get; set; }
        public virtual ICollection<ParsedPost> ParsedPosts { get; set; }
    }
}