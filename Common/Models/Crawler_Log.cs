using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;

namespace Tazeyab.Common.Models
{
    public class Crawler_Log : BaseEntity
    {
        public int FeedId { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string PropertyNames { get; set; }
        public string PropertyValues { get; set; }
        public int Type { get; set; }
        public string Message { get; set; }
        public Nullable<int> NumberOfNewItem { get; set; }
    }
}
