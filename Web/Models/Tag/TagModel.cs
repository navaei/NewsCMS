using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Web.WebLogic.BaseModel;

namespace Mn.NewsCms.Web.Models
{
    public class AllTagModel
    {
        public string CatCode { get; set; }
        public string CatTitle { get; set; }
        public IEnumerable<TagViewModel> Tags { get; set; }
    }
    public class TagViewModel : BaseViewModel<Tag>
    {
        [Display(Name="عنوان")]
        public string Title { get; set; }
        [Display(Name = "مقدار")]
        public string Value { get; set; }
        [Display(Name = "رنگ")]
        public string Color { get; set; }
        [Display(Name = "کد تگ")]
        public string EnValue { get; set; }
        [Display(Name = "عکس تگ")]
        public string Thumbnail { get; set; }
        [Display(Name = "اندازه فونت")]
        public int FontSize { get; set; }
        [Display(Name = "عکس")]
        public string ImageThumbnail { get; set; }
        //public string BackgroundImage { get; set; }
        public Nullable<long> ParentTagId { get; set; }
        [Display(Name = "رنگ پس زمینه")]
        public string BackgroundColor { get; set; }
        [Display(Name = "نمایش درصفحه اول")]
        public Nullable<bool> InIndex { get; set; }
        public IEnumerable<FeedItem> Items { get; set; }
        [Display(Name = "دسته بندی")]
        public List<long> SelectedCategories { get; set; }

        [Display(Name = "تگ های مرتبط")]
        public List<long> SelectedTags { get; set; }
    }
}
