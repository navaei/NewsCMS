using Mn.NewsCms.Common.BaseClass;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mn.NewsCms.Common.Models
{
    public enum TypeOfLog : byte
    {
        NotSet = 0,
        Start = 1,
        End = 2,
        OK = 3,
        Info = 4,
        Error = 5,
        Other = 6,
        Warning = 7
    }
    public class LogsBuffer : BaseEntity
    {
        [Display(Name = "پیام")]
        [MaxLength(1024)]
        public string Value { get; set; }
        [Display(Name = "کد")]
        [MaxLength(64)]
        public string Code { get; set; }
        [Display(Name = "نوع لاگ")]
        public TypeOfLog Type { get; set; }
        [Display(Name = "زمان ایجاد")]
        public DateTime CreationDate { get; set; }
    }

}
