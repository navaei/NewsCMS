using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mn.NewsCms.Common.EventsLog;

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
