using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Mn.NewsCms.Common.Models
{
    public enum AdsType
    {
        [Description("نوشتاری")]
        Text = 1,
        [Description("تصویر")]
        Gif = 2,
        [Description("اسکریپت")]
        Script = 3,
        [Description("فلش")]
        Flash = 4,
    }
    public class Ad : BaseEntity
    {
        public Ad()
        {
            this.Tags = new List<Tag>();
            this.Categories = new List<Category>();
        }

        public override long Id { get; set; }
        [MaxLength(50)]
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "نوع تبلیغ")]
        public AdsType AdsType { get; set; }
        [MaxLength(100)]
        [Display(Name = "لینک")]
        public string Link { get; set; }
        [MaxLength(512)]
        [Display(Name = "محتوا")]
        public string Content { get; set; }
        [Display(Name = "زمان انتشار")]
        public DateTime CreationDate { get; set; }
        [Display(Name = "زمان انقضا")]
        public DateTime ExpireDate { get; set; }
        [Display(Name = "تبلیغ سراسری")]
        public bool? Global { get; set; }
        [Display(Name = "غیرفعال")]
        public bool Disable { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
