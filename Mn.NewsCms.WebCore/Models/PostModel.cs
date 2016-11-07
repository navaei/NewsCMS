using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mn.NewsCms.Common;
using Mn.NewsCms.WebCore.WebLogic.BaseModel;

namespace Mn.NewsCms.WebCore.Models
{
    public class PostModel : BaseViewModel<Post>
    {
        public PostModel()
        {
            SelectedCategories = new List<long>();
            SelectedTags = new List<long>();
        }

        [MaxLength(256)]
        [Required]
        [Display(Name = "عنوان خبر")]
        public string Title { get; set; }

        [MaxLength(50)]
        [Display(Name = "کد مطلب")]
        public string Name { get; set; }

        [MaxLength(512)]
        [Display(Name = "عنوان فرعی")]
        public string SubTitle { get; set; }

        [MaxLength(512)]
        [Display(Name = "تصویر شاخص")]
        public string PostImage { get; set; }

        [Display(Name = "زمان انتشار")]
        public DateTime? PublishDate { get; set; }

        [Required]
        [Display(Name = "متن خبر")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "قابلیت ارسال نظر")]
        public bool EnableComment { get; set; }
        [Display(Name = "نوع مطلب")]
        public PostType PostType { get; set; }
        [Display(Name = "نمایش درصفحه‌اصلی")]
        public bool ShowInIndex { get; set; }
        public int UserId { get; set; }

        [Display(Name = "دسته بندی")]
        public List<long> SelectedCategories { get; set; }

        [Display(Name = "تگ های مرتبط")]
        public List<long> SelectedTags { get; set; }
        [Display(Name = "دسته بندی")]
        public List<Category> Categories { get; set; }
        [Display(Name = "تگ های مرتبط")]
        public List<Common.Tag> Tags { get; set; }
        //public MetaData MetaData { get; set; }
        [Display(Name = "نظرات")]
        public List<Comment> Comments { get; set; }

        [Display(Name = "آرای مثبت")]
        public int LikeCount { get; set; }
        [Display(Name = "آرای منفی")]
        public int DislikeCount { get; set; }
        [Display(Name = "تعداد بازدید")]
        public int VisitCount { get; set; }

    }
}