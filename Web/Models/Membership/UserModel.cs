using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Mn.NewsCms.Common.Membership;

namespace Mn.NewsCms.Web.Models.Membership
{
    public class UserModel : BaseViewModel
    {
        [MaxLength(128)]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [MaxLength(128)]
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [MaxLength(128)]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "شمار تماس")]
        public string PhoneNumber { get; set; }
        [Display(Name = "خبرنامه")]
        public Nullable<bool> Subscription { get; set; }
        [Display(Name = "فعال بودن")]
        public bool LockoutEnabled { get; set; }
        [Display(Name = "تایید ایمیل")]
        public bool EmailConfirmed { get; set; }
        public List<int> SelectedRoles { get; set; }

        public User ToModel(User dbUser)
        {
            Common.Helper.AutoMapper.Map(this, dbUser);
            return dbUser;
        }
    }
}