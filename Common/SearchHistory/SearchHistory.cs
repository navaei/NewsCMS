using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tazeyab.Common.Membership;

namespace Tazeyab.Common
{
    [Table("SearchHistory")]
    public class SearchHistory : BaseEntity
    {
        [Column("SearchHistoryId")]
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
        [Display(Name = "عبارت جستجو")]
        public string SearchKey { get; set; }
        [Display(Name = "زمان ایجاد")]
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.Guid> UserId { get; set; }
        public Nullable<long> TagId { get; set; }
        public Nullable<long> CatId { get; set; }
        public Nullable<long> SiteId { get; set; }

        [ForeignKey("CatId")]
        public virtual Category Category { get; set; }
        [Display(Name = "سایت")]
        public virtual Site Site { get; set; }
        [Display(Name = "کلیدواژه")]
        public virtual Tag Tag { get; set; }
        [Display(Name = "کاربر")]
        public virtual User User { get; set; }
    }
}
