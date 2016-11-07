using System.ComponentModel.DataAnnotations;

namespace Mn.NewsCms.WebCore.Models
{

    public class UserDetailsModel
    {
        public string OpenID { get; set; }
        public string ProviderUrl { get; set; }
        public string FriendlyIdentifier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dob { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
    }
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور فعلی")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور جدید")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تایید رمز عبور")]
        [Compare("NewPassword", ErrorMessage = "رمز عبور با تایید رمز عبور یکسان نیست")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Display(Name = "OpenID")]
        public string OpenID { get; set; }

        [Required]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "مقدار لین فیلد الزارمی است")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Display(Name = "مرا بخاطر بسپار ؟")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Display(Name = "OpenID")]
        public string OpenID { get; set; }

        [Required]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "آدرس ایمیل")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تایید رمز عبور")]
        [Compare("Password", ErrorMessage = "پسورد و تکرارش با یکدیگر همخوانی ندارند")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "کد امنیتی")]
        public string Captcha { get; set; }
    }

    //public interface IMembershipService
    //{
    //    int MinPasswordLength { get; }
    //    bool ValidateUser(string userName, string password);
    //    MembershipCreateStatus CreateUser(string userName, string password,
    //                                      string email, string OpenID);
    //    bool ChangePassword(string userName, string oldPassword, string newPassword);
    //    MembershipUser GetUser(string OpenID);
    //}

}
