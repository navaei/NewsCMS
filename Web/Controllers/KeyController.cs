using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common.Models;
using System.Threading.Tasks;
using System.Threading;
using Tazeyab.Common;
using Tazeyab.DomainClasses.ContentManagment;
using Tazeyab.Web.WebLogic;


namespace Tazeyab.Web.Controllers
{
    public partial class KeyController : BaseController
    {
        //
        // GET: /Key/
        [OutputCache(Duration = TazeyabConfig.Cache5Min, VaryByParam = "content;PageIndex")]
        public virtual ActionResult Index(string content, int PageIndex = 0)
        {
            if (string.IsNullOrEmpty(content))
                return RedirectToAction(MVC.Tag.All());
            var Content = content.Trim();
            var LastItemPubDate = DateTime.Now.AddMinutes(15);

            #region IfSite
            if (Utility.HasFaWord(Content) == false)
            {
                Content = Content.ReplaceX("www.", "");
                if (Content.CountStringOccurrences(".") == 1 || Content.CountStringOccurrences(".") == 2)
                    if (Utility.UrlIsValid(Content) || Utility.UrlIsValid("www." + Content))
                    {
                        return Redirect("/site/" + Content);
                        //return RedirectToAction(MVC.Site.ActionNames.Index, MVC.Site.Name, new { Content = Content[Content.Count() - 1] == '/' ? Content : Content + "/", PageSize = PageSize, LastItemPubDate = LastItemPubDate });
                    }
            }
            #endregion

            #region IfTag
            Tag TagCurrent = null;
            try
            {
                TagCurrent = Ioc.TagBiz.Get(Content.Replace("_", " "));
                if (TagCurrent == null)
                {
                    var tagCandidate = Ioc.TagBiz.GetList().Where(x => x.Value.Contains(content + "|") || x.Value.Contains("|" + content)).ToList();
                    if (tagCandidate.Any())
                    {
                        TagCurrent = tagCandidate.First();
                        return RedirectToAction(MVC.Tag.Index(TagCurrent.Title));
                    }
                }
                else
                    return RedirectToAction(MVC.Tag.Index(TagCurrent.Title));
            }
            catch
            {
                //TagCurrent = context.Tags.Where(x => x.Value.Contains(Content)).First();
            }

            #endregion

            #region IfNotFound

            var res = new List<FeedItem>();

            res = Ioc.ItemBiz.FeedItemsByKey(Content, PageSize, PageIndex).ToList();

            if (res.Count() < 3)
                return RedirectToAction(MVC.Search.ActionNames.Index, MVC.Search.Name, new { Content = Content });
            #endregion

            #region ViewBag
            ViewBag.PageSize = PageSize;
            ViewBag.Toggle = "1";
            ViewBag.SearchTextDir = "text-align:right;direction:rtl";
            ViewBag.Title = Content;
            ViewBag.Content = Content;
            ViewBag.SearchExpersion = Content;
            ViewBag.PageHeader = "نتیجه جستجو " + Content;
            ViewBag.RecentTags = Ioc.TagBiz.RelevantTags(Content).ToList();
            ViewBag.PageIndex = PageIndex + 1;
            #endregion

            if (string.IsNullOrEmpty(Content))
            {
                return RedirectToAction(MVC.Tag.Name, MVC.Tag.ActionNames.All, null);
            }

            ViewBag.TopSites = Ioc.SiteBiz.GetTopSites(20, 120);
            res = Ioc.ItemBiz.DescriptClear(res, Content).ToList();

            return View("Index." + TazeyabConfig.ThemeName, res);
        }

        [AjaxOnly]
        [OutputCache(Duration = 610, VaryByParam = "Content;PageIndex")]
        public virtual ActionResult FeedItems(string Content, int PageIndex)
        {
            #region viewBag
            ViewBag.Content = Content;
            ViewBag.PageIndex = PageIndex + 1;
            ViewBag.PageHeader = "تازه‌ترین های " + Content;
            #endregion

            var res = Ioc.ItemBiz.FeedItemsByKey(Content, PageSize, PageIndex);
            return PartialView("_FeedItems.Tazeyab", res);
        }

        public void SomeTaskAsync(int id)
        {
            AsyncManager.OutstandingOperations.Increment();
            Task.Factory.StartNew(taskId =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(200);
                    HttpContext.Application["task" + taskId] = i;
                }
                var result = "result";
                AsyncManager.OutstandingOperations.Decrement();
                AsyncManager.Parameters["result"] = result;
                return result;
            }, id);
        }
    }
}
