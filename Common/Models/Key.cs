using System;
using System.Collections.Generic;

namespace Tazeyab.Common.Models
{
    public class Key
    {
        public decimal KeyId { get; set; }
        public string Value { get; set; }
        public decimal FeedItemsId { get; set; }
        public Nullable<int> ConversionNumber { get; set; }
        public virtual FeedItem FeedItem { get; set; }
    }
}
