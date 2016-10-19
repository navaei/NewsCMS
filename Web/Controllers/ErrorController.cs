using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Web.WebLogic;

namespace Mn.NewsCms.Web.Controllers
{
    public partial class ErrorController : BaseController
    {
        //
        // GET: /Error/
        public virtual ActionResult Index(string aspxerrorpath, string msg)
        {
            try
            {
                if (!string.IsNullOrEmpty(msg) && msg.Contains("Illegal characters in path"))
                    if (aspxerrorpath.ToLower().Contains("/site/"))
                        Response.Redirect(string.Format("/site/{0}/{1}", aspxerrorpath.ToLower().Replace("http://", "").Split('/')[2], aspxerrorpath.ToLower().Replace("http://", "").Split('/')[3]));
                ViewBag.Content = " درخواست شما  با خطا برخورد کرد " + msg;
            }
            catch
            {
                ViewBag.Content = " درخواست شما  با خطا برخورد کرد ";
            }
            return View();
        }       
        [OutputCache(Duration = CmsConfig.Cache5Min, VaryByParam = "aspxerrorpath")]
        public virtual ActionResult notfound()
        {
            ViewBag.ErrorContent = "صفحه مورد نظر یافت نشد";
            return View();
        }

        [OutputCache(Duration = CmsConfig.Cache5Min)]
        public virtual ActionResult forbidden(string message)
        {
            ViewBag.ErrorContent = "اجازه دسترسی به این صفحه را ندارید";
            return View();
        }

        [OutputCache(Duration = CmsConfig.Cache5Min)]
        public virtual ActionResult badrequest()
        {            
            return View();
        }

    }
}
