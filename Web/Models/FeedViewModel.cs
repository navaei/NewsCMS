using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Share;
using Mn.NewsCms.Web.WebLogic.BaseModel;
using Mn.NewsCms.Web.WebLogic.Binder;

namespace Mn.NewsCms.Web.Models
{
    //[ModelBinder(typeof(PersianDateModelBinder))]
    public class FeedViewModel : BaseViewModel<Feed>
    {
        [Display(Name = "سایت")]
        public long SiteId { get; set; }
        [Display(Name = "شناسه")]
        public long FeedId { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "سایت")]
        public string SiteSiteTitle { get; set; }
        [Display(Name = "آدرس سایت")]
        public string SiteSiteUrl { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Display(Name = "آدرس فید")]
        public string Link { get; set; }
        [Display(Name = "نوع فید")]
        public FeedType FeedType { get; set; }
        [Display(Name = "تعداد خطاها")]
        public byte? UpdatingErrorCount { get; set; }
        [Display(Name = "وضعیت")]
        public DeleteStatus Deleted { get; set; }
        [Display(Name = "آخرین مطلب")]
        public string LastUpdatedItemUrl { get; set; }
        [Display(Name = "سرعت بروزرسانی")]
        public Nullable<int> UpdateSpeed { get; set; }
        [Display(Name = "بازه بروزرسانی")]
        public Nullable<long> UpdateDurationId { get; set; }
        [Display(Name = "بازه بروزرسانی")]
        public string UpdateDurationTitle { get; set; }

        [Display(Name = "آخرین زمان بازدید بروزرسان")]
        [ReadOnly(true)]
        public DateTime? LastUpdaterVisit { get; set; }

        [Display(Name = "آخرین زمان بروز شدن")]
        [ReadOnly(true)]
        public DateTime? LastUpdateDateTime { get; set; }

        public ICollection<Category> Categories { get; set; }
        public ICollection<Tag> Tags { get; set; }

        [Display(Name = "دسته بندی")]
        public List<long> SelectedCategories { get; set; }

        [Display(Name = "تگ های مرتبط")]
        public List<long> SelectedTags { get; set; }
    }
}