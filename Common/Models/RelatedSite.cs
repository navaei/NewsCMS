using System;
using System.Collections.Generic;

namespace Mn.NewsCms.Common.Models
{
    public class RelatedSite
    {
        public decimal Id { get; set; }
        public decimal MainSiteId { get; set; }
        public decimal RelatedSiteId { get; set; }
    }
}
