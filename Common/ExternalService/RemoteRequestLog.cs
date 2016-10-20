using Mn.NewsCms.Common.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mn.NewsCms.Common.Models
{
    public class RemoteRequestLog : BaseEntity<long>
    {
        [Display(Name = "آدرس بلاگ")]
        public string RequestRefer { get; set; }
        [Display(Name = "˜کنترلر")]
        public string Controller { get; set; }
        [Display(Name = "عنوان")]
        public string Content { get; set; }
        [Display(Name = "زمان ایجاد")]
        public System.DateTime CreationDate { get; set; }
    }
}
