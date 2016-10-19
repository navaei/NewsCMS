using System;
using System.Linq;
using System.Web.Mvc;
using Mn.NewsCms.Common.Models;
using System.Diagnostics;
using System.Collections.Generic;
using Mn.NewsCms.Common;
using System.Net;
using System.Data.Entity.Validation;
using Mn.NewsCms.DomainClasses.ContentManagment;
using Mn.NewsCms.DomainClasses.UpdaterBusiness;
using Mn.NewsCms.Web.WebLogic;

namespace Mn.NewsCms.Web.Controllers
{
    public partial class SiteController : BaseController
    {
        //
        // GET: /Site/

        [OutputCache(Duration = CmsConfig.Cache10Min, VaryByParam = "Content;PageIndex")]
        public virtual ActionResult Index(string Content, int PageIndex)
        {
            Content = Content.ReplaceX("www.", "");
            var siteCurrents = Ioc.SiteBiz.GetList(Content);
            if (siteCurrents.Count() == 0)
            {
                var startw = Content.Remove(Content.LastIndexOf(".") + 1, Content.Length - Content.LastIndexOf(".") - 1);
                siteCurrents = Ioc.SiteBiz.GetList(startw);
                if (!siteCurrents.Any())
                    return RedirectToAction(MVC.Error.notfound());
            }
            var siteCurrent = siteCurrents.First();
            #region ViewBag
            //ViewBag.EntityCode = "Site";
            ViewBag.EnTityRef = siteCurrent.Id;

            ViewBag.Title = siteCurrent.SiteTitle + " " + siteCurrent.SiteUrl;
            ViewBag.Content = siteCurrent.SiteUrl;
            //ViewBag.KeyWords = siteCurrent.SiteTags;
            //ViewBag.PageSize = PageSize;            
            ViewBag.SearchTextDir = "text-align:left;direction:ltr";
            ViewBag.siteCurrent = siteCurrent;
            ViewBag.PageHeader = "تازه ترین های " + siteCurrent.SiteTitle + " (" + siteCurrent.SiteUrl + ")";
            ViewBag.PageIndex = PageIndex + 1;
            ViewBag.KeyWords = "," + siteCurrent.SiteUrl + "," + siteCurrent.SiteTitle + ",اخبار روز,تازه ترین خبرها,خبرخوان";
            ViewBag.Discription = "تازه ترین مطالب سایت " + siteCurrent.SiteTitle + " - " + siteCurrent.SiteUrl + " - " + siteCurrent.SiteDesc;
            #endregion
            var LastItemPubDate = DateTime.Now.AddMinutes(-15);
            var res = Ioc.ItemBiz.FeedItemsBySite(siteCurrent.Id, PageSize, PageIndex, false);

            if (res.Count == 0)
                res = Ioc.ItemBiz.FeedItemsByCat(6, PageSize, PageIndex, false);

            if (siteCurrent.HasImage != HasImage.NotSupport)
                ViewBag.VisualItems = Ioc.ItemBiz.FeedItemsBySite(siteCurrent.Id, Ioc.AppConfigBiz.GetVisualPostCount() + Ioc.AppConfigBiz.GetVisualPostCount(), PageIndex, true);

            #region Related Site
            //----------------Related Site by this site-------     
            ViewBag.TopSites = siteCurrent.Feeds.SelectMany(f => f.Categories)
                .SelectMany(cf => cf.Feeds).Select(f2 => f2.Site).Take(20).ToList()
                .Select(s => new SiteOnlyTitle() { SiteUrl = s.SiteUrl, SiteTitle = s.SiteTitle }).DistinctBy(x => x.SiteUrl).ToList();
            #endregion
            if (!(ViewBag.TopSites as List<SiteOnlyTitle>).Any())
            {
                ViewBag.TopSites = Ioc.SiteBiz.GetTopSites(15, 120);
            }
            #region Tabs
            //var cats = siteCurrent.Feeds.SelectMany(f => f.Categories.Select(cf => cf.Id)).ToList();
            //ViewBag.RemoteWebParts = Ioc.RemoteWpBiz.GetByCats(cats).ToList();
            //ViewBag.Pages = Ioc.PostBiz.GetList().Where(p => p.PostType == PostType.Tab && p.Categories.Any(pc => cats.Contains(pc.Id))).ToList();
            #endregion
            return View("Index." + CmsConfig.ThemeName, res);
        }

        [AjaxOnly]
        [OutputCache(Duration = CmsConfig.Cache10Min, VaryByParam = "Content;PageIndex")]
        public virtual ActionResult FeedItems(string Content, int PageIndex)
        {
            if (PageIndex > 5)
                PageIndex = 5;

            Content = Content.ReplaceX("www.", "");
            var siteCurrent = Ioc.SiteBiz.Get(Content);
            if (siteCurrent == null)
            {
                return RedirectToAction(MVC.Error.notfound());
            }
            var LastDateTime = DateTime.Now.AddMinutes(15);

            ViewBag.Content = Content;
            ViewBag.PageIndex = PageIndex + 1;
            ViewBag.PageHeader = "تازه‏ترین مطالب سایت " + siteCurrent.SiteTitle + " (" + siteCurrent.SiteUrl + ")";

            var res = Ioc.ItemBiz.FeedItemsBySite(siteCurrent.Id, PageSize, PageIndex);
            return PartialView("_FeedItems.Tazeyab", res);
        }

        [OutputCache(Duration = CmsConfig.Cache10Min, VaryByParam = "Content")]
        public virtual ActionResult FeedItemsRemote(string Content, int PageSize)
        {
            Content = Content.ReplaceX("www.", "");
            var siteCurrent = Ioc.SiteBiz.Get(Content);
            if (siteCurrent == null)
                return PartialView("_FeedItemsRemote", new List<FeedItem>());

            Content = siteCurrent.SiteTitle.SubstringETC(0, 20);
            ViewBag.PageHeader = "تازه ترین مطالب سایت " + siteCurrent.SiteTitle.SubstringETC(0, 50);
            ViewBag.BaseAddress = "http://" + Resources.Core.SiteUrl + "/site/" + siteCurrent.SiteUrl;

            var res = Ioc.ItemBiz.FeedItemsBySite(siteCurrent.Id, PageSize, 0);
            Ioc.BlogService.InsertRemoteRequestLog(this.Name, Content);
            ViewBag.Content = Content;
            return PartialView("_FeedItemsRemote", res);
        }

        //[OutputCache(Duration = CmsConfig.Cache5Min, VaryByParam = "FeedItemId", VaryByCustom = "IsMobile")]
        public virtual ViewResult Page(string SiteLink, string FeedItemId, string ItemTitle)
        {
            var item = Ioc.ItemBiz.Get(FeedItemId);
            if (item == null)
                Response.Redirect("~/error/notfound");
            else
            {
                item.VisitsCount++;
                Ioc.ItemBiz.Update(item);
            }

            if (!ItemsController.VisitedItems.Any(i => i.Id == item.Id))
            {
                try
                {
                    if (!string.IsNullOrEmpty(Request.UserAgent) && !Request.UserAgent.Contains("bot"))
                    {
                        if (ItemsController.VisitedItems == null)
                            ItemsController.VisitedItems = new List<FeedItem>();

                        ItemsController.VisitedItems.Add(item);
                    }
                }
                catch { }
            }
            if (item.ShowContentType == ShowContent.OriginalSite)
                Response.Redirect(item.Link);

            return View("Page." + CmsConfig.ThemeName, item);
        }

        [OutputCache(Duration = CmsConfig.Cache10Min, VaryByParam = "u;c;q")]
        public virtual ActionResult All(string u, string c = "all", int q = 0)
        {
            ViewBag.Count = q;
            ViewBag.Cat = c;
            int Count = ViewBag.MaxCount = 30;
            var cats = new List<long>();
            if (!string.IsNullOrEmpty(c) && c != "all")
            {
                var catId = Ioc.CatBiz.Get(c).Id;
                cats = Ioc.CatBiz.GetList(catId).Select(cc => cc.Id).ToList();
                cats.Add(catId);
            }

            var query = Ioc.SiteBiz.GetList();
            if (cats.Any())
                query = query.Where(s => s.Feeds.Any(f => f.Categories.Any(cc => cats.Contains(cc.Id))));

            //var feedtop = Ioc.FeedBiz.GetList().Where(x => !x.Site.SkipType.HasValue || x.Site.SkipType == 0)
            //    .OrderBy(x => x.UpdateDurationId).Skip(q).Take(Count).Select(x => x.Id).ToList();
            var res = query.Where(s => s.Feeds.Any(x => !x.Site.SkipType.HasValue || x.Site.SkipType == 0))
                .OrderBy(x => x.PageRank)
                .Skip(q)
                .Take(Count)
                .Distinct()
                .ToList();
            if (res.Any())
                res.Last().CrawledCount = q + Count;


            //View(res);

            return View("All." + CmsConfig.ThemeName, res);
        }

    }
}
