using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mn.NewsCms.Common.Membership;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Share;

namespace Mn.NewsCms.Common
{
    public enum HasImage : byte
    {
        NotSupport = 0,
        HtmlPattern = 1,
        OpenGraph = 2
    }
    public enum HasFeed : byte
    {
        No = 0,
        Rss = 1,
        Atom = 2,
        XPath = 5
    }
    public enum IsBlog : byte
    {
        No = 0,
        Blog = 1,
    }
    public enum ShowContent : byte
    {
        Inner = 0,
        InnerNoBanner = 1,
        OriginalSite = 2,
        NoContent = 3,
    }
    public class Site : BaseEntity<long>
    {
        public Site()
        {
            this.Feeds = new List<Feed>();
            this.SearchHistories = new List<SearchHistory>();
            this.Users = new List<User>();
        }

        [Column("SiteId")]
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
        public string SiteTitle { get; set; }
        [MaxLength(256)]
        public string SiteUrl { get; set; }
        [MaxLength(256)]
        public string SiteDesc { get; set; }
        //public string SiteTags { get; set; }
        public Nullable<int> CrawledCount { get; set; }
        public Nullable<System.DateTime> LastCrawledDate { get; set; }
        public Nullable<int> LinkedCount { get; set; }
        public Nullable<int> ExternalLinkCount { get; set; }
        public HasImage HasImage { get; set; }
        [MaxLength(128)]
        public string ImagePattern { get; set; }
        [NotMapped]
        public string IndexPageText { get; set; }
        public HasFeed HasFeed { get; set; }
        public Nullable<int> SkipType { get; set; }
        public bool IsBlog { get; set; }
        // public byte[] SiteLogo { get; set; }
        public Nullable<byte> PageRank { get; set; }
        /// <summary>
        /// Pers Minute
        /// </summary>
        public int TimeDifference { get; set; }
        public Nullable<bool> HasSocialTag { get; set; }
        public ShowContent ShowContentType { get; set; }
        public DeleteStatus DeleteStatus { get; set; }
        public virtual ICollection<Feed> Feeds { get; set; }
        public virtual ICollection<SearchHistory> SearchHistories { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<FeedItem> FeedItems { get; set; }
    }
}
