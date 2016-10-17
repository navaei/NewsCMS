using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tazeyab.Web.Models
{
    public class FeedItemViewModel
    {
        public long? ItemId { get; set; }
        public string FeedItemId { get; set; }
        public string Description { get; set; }
        public string SiteTitle { get; set; }
        public string SiteUrl { get; set; }
        public string Tags { get; set; }
        public long SiteId { get; set; }
        public List<int> Cats { get; set; }
    }
}