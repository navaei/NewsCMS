using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Mn.NewsCms.Common.Membership;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Share;

namespace Mn.NewsCms.Common
{
    public class Category : BaseEntity
    {
        public Category()
        {
            this.Feeds = new List<Feed>();
            this.PhotoItems = new List<PhotoItem>();
            this.SearchHistories = new List<SearchHistory>();
            this.Tags = new List<Tag>();
            this.Users = new List<User>();
        }

        [Column("CatId")]
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
        public string Title { get; set; }
        public Nullable<long> ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Category ParentCategory { get; set; }
        public string Code { get; set; }
        public bool Active { get; set; }
        public Nullable<int> Priority { get; set; }
        public string Description { get; set; }
        public ViewMode ViewMode { get; set; }
        public string KeyWords { get; set; }
        public string Color { get; set; }
        public byte[] Icon { get; set; }
        public string ImageThumbnail { get; set; }
        public Nullable<int> IndexItemsCount { get; set; }
        public virtual ICollection<Feed> Feeds { get; set; }
        public virtual ICollection<PhotoItem> PhotoItems { get; set; }
        public virtual ICollection<SearchHistory> SearchHistories { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Ad> Ads { get; set; }

    }
}
