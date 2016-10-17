using System;
using System.Collections.Generic;

namespace Tazeyab.Common.Models
{
    public class CatsFeedsTop
    {
        public decimal CatFeedTopId { get; set; }
        public int Id { get; set; }
        public int FeedId { get; set; }
        public virtual Category Category { get; set; }
        public virtual Feed Feed { get; set; }
    }
}
