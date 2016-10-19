using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Membership;

namespace Mn.NewsCms.Web.WebLogic
{
    public class CmsMembership
    {
        User _CurrentUser;

        public Guid? GetCurrentUserId()
        {
            Guid? temp;
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                temp = Guid.Parse(WebLogic.WebSecurity.GetUser(HttpContext.Current.User.Identity.Name).ProviderUserKey.ToString());
            else
                temp = null;
            return temp;
        }
        public string GetCurrentUserTitle()
        {
            if (_CurrentUser == null)
                _CurrentUser = Ioc.UserBiz.Get(HttpContext.Current.User.Identity.Name);
            if (_CurrentUser.UserName.Contains('.'))
                return _CurrentUser.UserName.SubstringM(0, _CurrentUser.UserName.IndexOf('.'));
            if (_CurrentUser.UserName.Contains('_'))
                return _CurrentUser.UserName.SubstringM(0, _CurrentUser.UserName.IndexOf('_'));
            if (_CurrentUser.UserName.Contains('@'))
                return _CurrentUser.UserName.SubstringM(0, _CurrentUser.UserName.IndexOf('@'));
            return _CurrentUser.UserName;
        }
        public User GetCurrentUser()
        {
            if (_CurrentUser == null)
                _CurrentUser = Ioc.UserBiz.Get(HttpContext.Current.User.Identity.Name);
            return _CurrentUser;
        }
    }
}