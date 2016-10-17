using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tazeyab.Common.Models
{
    public class ItemVisited
    {
        public int ItemVisitedId { get; set; }
        public string FeedItemId { get; set; }
        public int VisitCount { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? PubDate { get; set; }
        public string Title { get; set; }
        public string SiteTitle { get; set; }
        public string SiteUrl { get; set; }
        public int FeedId { get; set; }
        public long SiteId { get; set; }
        public string Link { get; set; }
        public string EntityCode { get; set; }
        public int EntityRef { get; set; }

    }
}
