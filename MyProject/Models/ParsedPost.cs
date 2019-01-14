using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Models
{
    public class ParsedPost
    {
        public int ID { get; set; }
        public string Topic { get; set; }
        public string PostBody { get; set; }
        public DateTime? PostDate { get; set; }
        //  public string WebSiteID { get; set; }
        public DateTime? ParseDate { get; set; }
        public string PostUrl {get; set; }
       // public virtual WebSite WebSite { get; set; }
    }
}