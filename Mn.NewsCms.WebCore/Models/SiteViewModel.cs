using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Share;
using Mn.NewsCms.WebCore.WebLogic.BaseModel;

namespace Mn.NewsCms.WebCore.Models
{
    public class SiteViewModel : BaseViewModel<Site, long>
    {

        [DisplayName("عنوان")]
        [Required(ErrorMessage = "نام سایت را وارد کنید")]
        public string SiteTitle { get; set; }
        [DisplayName("آدرس")]
        [Required(ErrorMessage = "آدرس سایت را وارد کنید")]
        public string SiteUrl { get; set; }
        [DisplayName("خلاصه(Desc)")]
        public string SiteDesc { get; set; }
        public Nullable<int> CrawledCount { get; set; }
        public Nullable<System.DateTime> LastCrawledDate { get; set; }
        [DisplayName("تعداد بک لینکها")]
        public Nullable<int> LinkedCount { get; set; }
        [DisplayName("تعداد لینک های خروجی")]
        public Nullable<int> ExternalLinkCount { get; set; }
        [DisplayName("فید دارد")]
        public HasFeed HasFeed { get; set; }
        [DisplayName("مطالب تصویری")]
        public HasImage HasImage { get; set; }
        [DisplayName("کد مسیر عکس")]
        [MaxLength(128)]
        public string ImagePattern { get; set; }
        public Nullable<int> SkipType { get; set; }
        [DisplayName("نوع وبلاگ")]
        public bool IsBlog { get; set; }

        [DisplayName("امتیاز گوگل")]
        //[UIHint("_PageRank")]
        public byte? PageRank { get; set; }

        [DisplayName("قابلیت اپن گراف")]
        //[UIHint("_Flag")]
        public Nullable<bool> HasSocialTag { get; set; }
        [DisplayName("اختلاف زمانی")]
        public int TimeDifference { get; set; }

        //[UIHint("_ShowContentType")]
        [DisplayName("نحوه نمایش خبرها")]
        public ShowContent ShowContentType { get; set; }

        [DisplayName("وضعیت فعالیت")]
        public DeleteStatus DeleteStatus { get; set; }


    }
}