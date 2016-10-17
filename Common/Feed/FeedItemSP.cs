using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Tazeyab.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace Tazeyab.Common
{

    [NotMapped]
    public class FeedItemSP : FeedItem
    {
        //public long? ItemId { get; set; }
        //public string FeedItemId { get; set; }
        //public string Description { get; set; }      
        //public string Tags { get; set; }
        //public long SiteId { get; set; }
        public IEnumerable<int> Cats { get; set; }
    }
}
