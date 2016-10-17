using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common;
using Tazeyab.Common.Models;
using Tazeyab.Web.Models;

namespace Tazeyab.Web.Controllers
{
    public partial class PageController : BaseController
    {

        [OutputCache(Duration = 500, VaryByParam = "pageName;tab")]
        public virtual ActionResult Index(string pageName, bool? tab)
        {

            var post = Ioc.PostBiz.Get(pageName);
            if (!tab.HasValue)
            {
                var model = post.ToViewModel<PostModel>();
                model.Content = model.Content.Replace("[[", "<").Replace("]]", ">");
                return View(MVC.Post.Views.Page, model);
            }
            else
            {
                if (tab.Value)
                {
                    ViewBag.q = pageName;
                    return View(MVC.Page.ActionNames.Tab);
                }
            }

            ViewBag.Body = post.Content.Replace("[[", "<").Replace("]]", ">");
            ViewBag.Title = post.Title;

            return View();
        }
        [OutputCache(Duration = 50, VaryByParam = "q")]
        public virtual ActionResult Tab(string q)
        {
            ViewBag.q = q;
            return View();
        }

    }
}
