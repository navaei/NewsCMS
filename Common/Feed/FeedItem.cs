using Mn.Framework.Common;
using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tazeyab.Common.Config;

namespace Tazeyab.Common
{
    public class FeedItem : BaseEntity<Guid>
    {
        public FeedItem()
        {
            
        }

        [Column("FeedItemId")]
        public override Guid Id
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

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ItemId { get; set; }
        public string ItemId_Temp { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }
        [MaxLength(256)]
        public string Link { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        [Display(Name = "دارای عکس")]
        public bool HasPhoto { get; set; }
        //[MaxLength(256)]
        [NotMapped]
        public string PhotoUrl
        {
            get
            {
                return string.Concat("/", ServiceFactory.Get<IAppConfigBiz>().VisualItemsPathVirtual().ToLower(), "/", Id, ".jpg");
            }
        }
        public DateTime? PubDate { get; set; }
        public int VisitsCount { get; set; }
        public DateTime CreateDate { get; set; }
        public bool? Deleted { get; set; }
        public int? IndexedType { get; set; }
        public long FeedId { get; set; }
        public virtual Feed Feed { get; set; }
        public long? AlternativeFeedId { get; set; }
        public long? SiteId { get; set; }

        [Display(Name = "عنوان سایت")]
        [MaxLength(64)]
        public string SiteTitle { get; set; }

        [Display(Name = "آدرس سایت")]
        [MaxLength(64)]
        public string SiteUrl { get; set; }
        public ShowContent ShowContentType { get; set; }
    }
}
