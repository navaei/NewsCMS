using Mn.Framework.Web.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tazeyab.Common;
using Tazeyab.Common.Models;

namespace Tazeyab.Web.Areas.Dashboard.Models
{
    public class AdsModel : BaseViewModel<Ad>
    {
        public AdsModel()
        {
            this.Tags = new List<Tag>();
            this.Categories = new List<Category>();
        }
        [Required]
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
        [Required]
        [Display(Name = "زمان انتشار")]
        public System.DateTime CreationDate { get; set; }
        [Display(Name = "زمان انقضا")]
        public System.DateTime ExpireDate { get; set; }
        [Display(Name = "تبلیغ سراسری")]
        public bool Global { get; set; }
        [Display(Name = "غیرفعال")]
        public bool Disable { get; set; }
        //[Display(Name = "فایل تبلیغ")]
        //public IEnumerable<HttpPostedFileBase> Files { get; set; }

        [Display(Name = "دسته بندی")]
        public List<long> SelectedCategories { get; set; }

        [Display(Name = "تگ های مرتبط")]
        public List<long> SelectedTags { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}