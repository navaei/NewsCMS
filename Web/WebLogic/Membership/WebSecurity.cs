using Mn.Framework.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Mn.NewsCms.Common;


namespace Mn.NewsCms.Web.WebLogic
{
    public sealed class WebSecurity
    {
        public static HttpContextBase Context => new HttpContextWrapper(HttpContext.Current);

        public static HttpRequestBase Request => Context.Request;

        public static HttpResponseBase Response => Context.Response;

        public static System.Security.Principal.IPrincipal User => Context.User;

        public static bool IsAuthenticated => User.Identity.IsAuthenticated;

        public static MembershipCreateStatus Register(string Username, string Password, string Email, bool IsApproved, string FirstName, string LastName)
        {
            MembershipCreateStatus CreateStatus;
            System.Web.Security.Membership.CreateUser(Username, Password, Email, null, null, IsApproved, null, out CreateStatus);

            if (CreateStatus == MembershipCreateStatus.Success)
            {
                
                var userdb = ServiceFactory.Get<IUserBusiness>().Get(Username);
                userdb.FirstName = FirstName;
                userdb.LastName = LastName;
                ServiceFactory.Get<IUserBusiness>().Update(userdb);

                if (IsApproved)
                {
                    FormsAuthentication.SetAuthCookie(Username, false);
                }
            }

            return CreateStatus;
        }

        public enum MembershipLoginStatus
        {
            Success, Failure
        }

        public static MembershipLoginStatus Login(string Username, string Password, bool RememberMe)
        {
            if (System.Web.Security.Membership.ValidateUser(Username, Password))
            {
                FormsAuthentication.SetAuthCookie(Username, RememberMe);
                return MembershipLoginStatus.Success;
            }
            else
            {
                return MembershipLoginStatus.Failure;
            }
        }

        public static void Logout()
        {
            FormsAuthentication.SignOut();
        }

        public static MembershipUser GetUser(string Username)
        {
            return System.Web.Security.Membership.GetUser(Username);
        }

        public static bool ChangePassword(string OldPassword, string NewPassword)
        {
            MembershipUser CurrentUser = System.Web.Security.Membership.GetUser(User.Identity.Name);
            return CurrentUser.ChangePassword(OldPassword, NewPassword);
        }

        public static bool DeleteUser(string Username)
        {
            return Membership.DeleteUser(Username);
        }

        public static List<MembershipUser> FindUsersByEmail(string Email, int PageIndex, int PageSize)
        {
            int totalRecords;
            return Membership.FindUsersByEmail(Email, PageIndex, PageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }

        public static List<MembershipUser> FindUsersByName(string Username, int PageIndex, int PageSize)
        {
            int totalRecords;
            return Membership.FindUsersByName(Username, PageIndex, PageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }

        public static List<MembershipUser> GetAllUsers(int PageIndex, int PageSize)
        {
            int totalRecords;
            return Membership.GetAllUsers(PageIndex, PageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }
    }
}