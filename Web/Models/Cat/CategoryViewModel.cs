using Mn.Framework.Web.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tazeyab.Common;
using Tazeyab.Common.Share;

namespace Tazeyab.Web.Models
{
    public class CategoryViewModel : BaseViewModel
    {
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "والد")]
        public Nullable<long> ParentId { get; set; }
        [Display(Name = "کد دسته‌بندی")]
        public string Code { get; set; }
        [Display(Name = "اولویت")]
        public Nullable<int> Priority { get; set; }
        [Display(Name = "شرح")]
        public string Description { get; set; }
        [Display(Name = "نحوه نمایش")]
        public ViewMode ViewMode { get; set; }
        [Display(Name = "عبارات کلیدی")]
        public string KeyWords { get; set; }
        [Display(Name = "رنگ")]
        public string Color { get; set; }
    }
}