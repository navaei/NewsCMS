using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Extensions;
using Microsoft.AspNet.Identity;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Web.WebLogic;
using Mn.NewsCms.Web.WebLogic.Binder;

namespace Mn.NewsCms.Web.Areas.Dashboard
{
    [Authorize]
    public class BaseAdminController : MnBaseController
    {
        public int UserId { get; set; }

        public BaseAdminController()
        {
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
                filterContext.HttpContext.Response.Redirect("/login?ReturnUrl=" + Request.RawUrl);

            var userName = filterContext.RequestContext.HttpContext.User.Identity.Name;
            if (User.Identity.IsAuthenticated && ServiceFactory.Get<IUserBusiness>().IsInRole(userName, "admin"))
            {
                UserId = User.Identity.GetUserId<int>();
                ViewBag.Layout = Request.IsAjaxRequest() ? MVC.Dashboard.Shared.Views._LayoutModal : MVC.Dashboard.Shared.Views._Layout;
                base.OnAuthorization(filterContext);
            }
            else
                filterContext.HttpContext.Response.Redirect("/error/forbidden");

        }
    }
}