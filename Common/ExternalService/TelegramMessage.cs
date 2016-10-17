using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tazeyab.Common
{
    public class TelegramMessage : BaseEntity
    {
        [MaxLength(64)]
        [Display(Name = "دستور ارسالی")]
        public string Command { get; set; }
        [Display(Name = "شماره چت")]
        public long UpdateId { get; set; }
        [Display(Name = "شناسه کاربر")]
        public int ChatId { get; set; }
        [MaxLength(128)]
        [Display(Name = "نام کاربر")]
        public string Name { get; set; }
        [Display(Name = "زمان ارسال")]
        public DateTime CreateDateTime { get; set; }
    }
}
