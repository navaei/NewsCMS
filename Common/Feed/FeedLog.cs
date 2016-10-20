using Mn.NewsCms.Common.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Mn.NewsCms.Common
{

    public class FeedLog : BaseEntity
    {
        public FeedLog()
        {
            HasError = false;
            InitDate = DateTime.Now;
        }     
        [Display(Name = "رخداد خطا")]
        public bool HasError { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(512)]
        public string Message { get; set; }

        [Display(Name = "زمان ایجاد")]
        public DateTime InitDate { get; set; }

        [Display(Name = "زمان ثبت")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "تعداد آیتمها")]
        public int ItemsCount { get; set; }

        [Display(Name = "فید")]
        public long FeedId { get; set; }
        [Display(Name = "فید")]
        [ForeignKey("FeedId")]
        public Feed Feed { get; set; }       
    }
}
