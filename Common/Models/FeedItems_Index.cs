using Mn.NewsCms.Common.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mn.NewsCms.Common.Models
{
    public class FeedItems_Index : BaseEntity<long>
    {
        [Column("FeedItemIndexId")]
        public override long Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                base.Id = value;
            }
        }
        public string FeedItemId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime? PubDate { get; set; }
        public decimal VisitsCount { get; set; }
        public DateTime? CreateDate { get; set; }
        //public byte[] Image { get; set; }
        public string SiteURL { get; set; }
        public string SiteTitle { get; set; }
        public int? CatIdDefault { get; set; }
        public string ImageURL { get; set; }
    }
}
