using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common;
using Tazeyab.Web.WebLogic;

namespace Tazeyab.Web.Controllers
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

        //[HandleError]
        //public virtual ActionResult index(string message)
        //{
        //    if (message.IndexOf("Sequence contains no elements") > 0)
        //    {
        //        ViewBag.ErrorContent = "Error...";
        //    }
        //    else
        //        ViewBag.Content = "در درخواست شما خطا رخ داد";
        //    return View();
        //}
        [OutputCache(Duration = TazeyabConfig.Cache5Min, VaryByParam = "aspxerrorpath")]
        public virtual ActionResult notfound()
        {
            //try
            //{
            //    string query = Request.QueryString["aspxerrorpath"] ?? Request.RawUrl;
            //    string item = "/site/";
            //    if (query.ContainsX(item))
            //    {

            //        //return RedirectToAction(MVC.Site.Index(query.Substring(query.IndexOfX(item) + 6, query.Length - (query.IndexOfX(item) + 6)), 15, 0, null));
            //        return RedirectToAction(MVC.Site.ActionNames.Index, MVC.Site.Name,
            //            new { Content = query.Substring(query.IndexOfX(item) + 6, query.Length - (query.IndexOfX(item) + 6)) });
            //    }
            //    else
            //    {
            //        ViewBag.ErrorContent = "صفحه مورد نظر یافت نشد";
            //        return View();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ViewBag.ErrorContent = ex.Message;
            //    return View();
            //}

            ViewBag.ErrorContent = "صفحه مورد نظر یافت نشد";
            return View();
        }

        [OutputCache(Duration = TazeyabConfig.Cache5Min)]
        public virtual ActionResult forbidden(string message)
        {
            ViewBag.ErrorContent = "اجازه دسترسی به این صفحه را ندارید";
            return View();
        }

        [OutputCache(Duration = TazeyabConfig.Cache5Min)]
        public virtual ActionResult badrequest()
        {
            //var badreq = Request.UrlReferrer.ToString();
            //if (badreq.IndexOfX("site") > 5)
            //{
            //    var feedItem = badreq.Replace("http://", "").Split('/')[3];
            //}
            return View();
        }

    }
}
