using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mn.NewsCms.Web.Models
{
    public class AddWebSiteViewModel
    {
        public long SiteId { get; set; }
        public string SiteTitle { get; set; }
        public string SiteUrl { get; set; }
        public string SiteDesc { get; set; }
    }
}