using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Mn.NewsCms.Common.Membership;

namespace Mn.NewsCms.Common
{
    public class Comment : BaseEntity
    {
      
        [Required]
        [Display(Name="نام فرستنده")]
        [MaxLength(64)]
        public string SenderName { get; set; }
        [Required]
        [Display(Name = "متن نظر")]
        [MaxLength(1024)]
        public string Content { get; set; }
        [Display(Name = "ایمیل فرستنده")]
        [MaxLength(64)]
        public string EMail { get; set; }
        [Display(Name = "وضعیت تایید")]
        public bool Approve { get; set; }
        public long? CommentRefId { get; set; }
        [ScriptIgnore]
        public Comment CommentRef { get; set; }
        [Display(Name = "موافقین")]
        public int LikeCount { get; set; }
        [Display(Name = "مخالفین")]
        public int DislikeCount { get; set; }
        [Display(Name = "تاریخ ارسال")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "تاریخ بروزرسانی")]
        public DateTime UpdateDate { get; set; }
        public long? PostId { get; set; }
        [ScriptIgnore]
        public Post Post { get; set; }
        public int? UserId { get; set; }
        [ScriptIgnore]
        public User User { get; set; }

    }
}
