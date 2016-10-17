using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.DomainClasses.WebPartBusiness;
using Tazeyab.Web.Models;

namespace Tazeyab.Web.Controllers
{
    public partial class RemoteWebPartController : Controller
    {
        //
        // GET: /RemoteWebPart/

        [OutputCache(Duration = 300, VaryByParam = "wpcode;tryNow")]
        public virtual ActionResult Index(string wpcode, bool tryNow = false)
        {
            var webpart = Ioc.RemoteWpBiz.Get(wpcode, tryNow);
            if (webpart == null)
            {
                ViewBag.Content = "وب پارت مورد نظر یافت نشد";
                return View();
            }
            if (!webpart.Active)
            {
                webpart.InnerHtml = "<h2>این بخش موقتا از دسترس خارج شده است.</h2>";
                return View(webpart);
            }

            ViewBag.Content = webpart.InnerHtml;
            ViewBag.CssLink = " <link rel='stylesheet' type='text/css' href='" + webpart.CssLink + "'>";
            if (!string.IsNullOrEmpty(webpart.JavaScript) && !webpart.JavaScript.Contains("<script"))
            {

                var fakeJs = "<script type='text/javascript' >";
                fakeJs += webpart.JavaScript;
                fakeJs += "</script>";
                ViewBag.JavaScript = fakeJs;

            }
            else
                ViewBag.JavaScript = webpart.JavaScript;

            return View();
        }

        [OutputCache(Duration = 50, VaryByParam = "wpcode")]
        public virtual ActionResult WpFrame(string wpcode)
        {
            ViewBag.wpcode = wpcode;
            return View();
        }

    }
}
