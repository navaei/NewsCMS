using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tazeyab.Common;
using Tazeyab.Common.Share;

namespace Tazeyab.Common
{
    public class CategoryModel
    {
        [Display(Name = "شناسه")]
        public long Id { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "سرگروه")]
        public Nullable<int> ParentId { get; set; }
        [Display(Name = "کد")]
        public string Code { get; set; }
        [Display(Name = "اولویت")]
        public Nullable<int> Priority { get; set; }
        [Display(Name = "شرح")]
        public string Description { get; set; }
        [Display(Name = "نحوه نمایش")]
        public ViewMode ViewMode { get; set; }
        [Display(Name = "کلمات کلیدی")]
        public string KeyWords { get; set; }
        [Display(Name = "رنگ")]
        public string Color { get; set; }
    }
}