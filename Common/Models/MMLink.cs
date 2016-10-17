using System;
using System.Collections.Generic;

namespace Tazeyab.Common.Models
{
    public class MMLink2
    {
        public int MMLinkId { get; set; }
        public string MMLink1 { get; set; }
        public Nullable<long> FeedId { get; set; }
        public Nullable<int> Id { get; set; }
        public string MMPattern { get; set; }
        public string AttributeName { get; set; }
        public virtual Category Category { get; set; }
        public virtual Feed Feed { get; set; }
    }
}
