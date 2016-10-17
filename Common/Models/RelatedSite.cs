using System;
using System.Collections.Generic;

namespace Tazeyab.Common.Models
{
    public class RelatedSite
    {
        public decimal Id { get; set; }
        public decimal MainSiteId { get; set; }
        public decimal RelatedSiteId { get; set; }
    }
}
