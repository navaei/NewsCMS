using Mn.NewsCms.Common.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using Mn.NewsCms.Common.Share;

namespace Mn.NewsCms.Common
{
    public class Feed : BaseEntity
    {
        public Feed()
        {
            this.FeedItems = new List<FeedItem>();
        }
        [Column("FeedId")]
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
        [MaxLength(128)]
        [Required]
        public string Title { get; set; }
        [MaxLength(256)]
        public string Description { get; set; }
        [MaxLength(512)]
        [Required]
        public string Link { get; set; }
        [MaxLength(128)]
        public string CopyRight { get; set; }
        public DateTime CreationDate { get; set; }
        public FeedType? FeedType { get; set; }        
        public bool InIndex { get; set; }
        public DeleteStatus Deleted { get; set; }
        public int? CatIdDefault { get; set; }
        
        public int UpdatingCount { get; set; }
        public byte UpdatingErrorCount { get; set; }
        [MaxLength(512)]
        public string LastUpdatedItemUrl { get; set; }
        public int UpdateSpeed { get; set; }
        public long? UpdateDurationId { get; set; }
        public DateTime? LastUpdaterVisit { get; set; }
        public DateTime? LastUpdateDateTime { get; set; }
        [ScriptIgnore]
        public virtual ICollection<Category> Categories { get; set; }
        [ScriptIgnore]
        public virtual ICollection<Tag> Tags { get; set; }
        [ScriptIgnore]
        public virtual ICollection<FeedItem> FeedItems { get; set; }
        public long SiteId { get; set; }

        [ForeignKey("SiteId")]
        [ScriptIgnore]
        public virtual Site Site { get; set; }

        [ForeignKey("UpdateDurationId")]
        [ScriptIgnore]
        public virtual UpdateDuration UpdateDuration { get; set; }
    }
}
