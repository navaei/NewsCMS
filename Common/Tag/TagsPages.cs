using System;
using System.Collections.Generic;

namespace Mn.NewsCms.Common.Models
{
    public partial class TagsPages
    {
        public int TagPageId { get; set; }
        public int TagId { get; set; }
        public int PageId { get; set; }       
        public virtual Tag Tag { get; set; }
        public virtual Post Page { get; set; }
    }
}
