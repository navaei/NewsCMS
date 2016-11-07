using System.Linq;
using Microsoft.AspNetCore.Http;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Membership;

namespace Mn.NewsCms.WebCore.WebLogic.Membership
{
    public class CmsMembership
    {
        private readonly IUserBusiness _userBusiness;
        User _CurrentUser;

        public CmsMembership(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        //public Guid? GetCurrentUserId()
        //{
        //    Guid? temp;
        //    if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
        //        temp = Guid.Parse(WebLogic.WebSecurity.GetUser(HttpContext.Current.User.Identity.Name).ProviderUserKey.ToString());
        //    else
        //        temp = null;
        //    return temp;
        //}
        public string GetCurrentUserTitle(HttpContext httpContext)
        {
            if (_CurrentUser == null)
                _CurrentUser = _userBusiness.Get(httpContext.User.Identity.Name);
            if (_CurrentUser.UserName.Contains('.'))
                return _CurrentUser.UserName.SubstringM(0, _CurrentUser.UserName.IndexOf('.'));
            if (_CurrentUser.UserName.Contains('_'))
                return _CurrentUser.UserName.SubstringM(0, _CurrentUser.UserName.IndexOf('_'));
            if (_CurrentUser.UserName.Contains('@'))
                return _CurrentUser.UserName.SubstringM(0, _CurrentUser.UserName.IndexOf('@'));
            return _CurrentUser.UserName;
        }
        public User GetCurrentUser(HttpContext httpContex)
        {
            if (_CurrentUser == null)
                _CurrentUser = _userBusiness.Get(httpContex.User.Identity.Name);
            return _CurrentUser;
        }
    }
}