using Mn.NewsCms.Common.BaseClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mn.NewsCms.Common.Models
{
    public enum MessageType
    {
        [Description("ارتباط با ما")]
        Contact = 0,
        [Description("تبلیغات")]
        Ads = 1,
    }
    public class ContactMessage : BaseEntity
    {
        [Display(Name = "نوع پیام")]
        public MessageType Type { get; set; }
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Display(Name = "فرستنده")]
        public string Name { get; set; }
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "شماره تماس")]
        public string Phone { get; set; }
        [Display(Name = "متن پیام")]
        public string Message { get; set; }
        [Display(Name = "خوانده شده")]
        public bool IsRead { get; set; }
        [Display(Name = "زمان ارسال")]
        public System.DateTime CreateDate { get; set; }
    }
}
