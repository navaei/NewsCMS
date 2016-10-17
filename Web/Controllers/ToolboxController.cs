using AutoMapper;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common;
using Tazeyab.Common.Models;
using Tazeyab.Common.Share;
using Tazeyab.Common.ViewModels;
using Tazeyab.Web.Models;
using Tazeyab.Web.WebLogic;

namespace Tazeyab.Web.Controllers
{
    public partial class ToolboxController : BaseController
    {
        //TazehaContext DbContext = new TazehaContext();

        //
        // GET: /Toolbox/
        [OutputCache(Duration = 6000)]
        public virtual ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 6000, VaryByParam = "src")]
        public virtual ActionResult script(string src)
        {
            if (src.CountStringOccurrences("/") == 1)
                src = string.Format("http://{0}/{1}/itemsremote/{2}", Resources.Core.SiteUrl, src.Split('/')[0], src.Split('/')[1]);
            else
                src = src.IndexOfX("http://") < 0 ? "http://" + src : src;
            ViewBag.Src = src;
            ViewBag.Code = src.Replace("http://", string.Empty).Replace("/", "_");
            return PartialView("_Script");
        }

        [HttpGet]
        public bool IsWebReady(string url)
        {
            try
            {
                if (!url.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
                    url.Insert(0, "http://");
                WebRequest request = WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var status = response.StatusCode;
                if (status == HttpStatusCode.OK)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("403"))
                    return false;
                return true;
            }
        }

        [OutputCache(Duration = 6000, VaryByParam = "Content")]
        public virtual ActionResult Demo(string Content, string PagePath, string Title)
        {
            //TazehaContext context = new TazehaContext();
            ToolBoxDemo model = new ToolBoxDemo();
            if (string.IsNullOrEmpty(Content))
            {

            }
            if (string.IsNullOrEmpty(PagePath))
                PagePath = "/cat/politics";
            model.Tags = Ioc.TagBiz.GetList().ToList();
            model.Tags.Insert(0, new Tag() { Title = "همه موارد", EnValue = "AllItems" });
            model.Cats = Ioc.CatBiz.GetList().ToList();
            var src = string.Empty;
            if (!string.IsNullOrEmpty(PagePath))
                src = string.Format("http://{0}/toolbox/script/?src={1}", Resources.Core.SiteUrl, PagePath[0] == '/' ? PagePath.Remove(0, 1) : PagePath);
            model.Script = string.Format("<a href='http://" + Resources.Core.SiteUrl + "/cat/politics'>{0}</a><script type='text/javascript' src='{1}'></script>", "تازه ترین خبرهای سیاسی", src);
            model.Content = Content;
            model.Title = Title;
            model.PagePath = PagePath;
            if (!string.IsNullOrEmpty(PagePath))
                model.IframeSrc = string.Format("{0}", PagePath.ReplaceAnyCase("cat/", "cat/itemsremote/").ReplaceAnyCase("tag/", "tag/itemsremote/").ReplaceAnyCase("site/", "site/itemsremote/"));
            else
                model.IframeSrc = "http://" + Resources.Core.SiteUrl + "/cat/itemsremote/news";
            return View(model);
        }

        [HttpGet]
        public virtual ActionResult AddWebSite()
        {
            return View("AddWebSite");
        }

        [HttpPost]
        public virtual ActionResult AddWebSite(AddWebSiteViewModel model)
        {
            return View("AddWebSite");
        }

        [HttpGet]
        public virtual ActionResult AddEditFeed(long siteId)
        {
            var site = Ioc.SiteBiz.GetList().SingleOrDefault(s => s.Id == siteId);
            ViewBag.Id = siteId;
            var model = new List<FeedViewModel>();
            if (site == null)
                return View("AddWebSite", model);
            else
            {
                model = site.Feeds.Select(f => new FeedViewModel()
                    {
                        FeedId = f.Id,
                        Title = f.Title,
                        Description = f.Description,
                        FeedType = f.FeedType.HasValue ? f.FeedType.Value : Common.Share.FeedType.Rss,
                        Link = f.Link,
                        SiteId = f.SiteId
                    }).ToList();
                return View("AddFeedsToSite", model);
            }
        }

        [HttpPost]
        public virtual ActionResult AddEditFeed([DataSourceRequest] DataSourceRequest request, FeedViewModel model)
        {
            var feed = Mapper.Map<FeedViewModel, Feed>(model);
            //if (!DbContext.Feeds.Any(f => f.Link == feed.Link))
            //{ }

            return View("AddFeedsToSite");
        }

        [AjaxOnly]
        public virtual JsonResult GetSite(string url)
        {
            url = url.ReplaceAnyCase("www.", "").ReplaceAnyCase("http://", "").ReplaceAnyCase("https://", "");
            var site = Ioc.SiteBiz.GetList().SingleOrDefault(s => s.SiteUrl.Contains(url));
            if (site != null)
                return Json(new { id = site.Id, url = site.SiteUrl, title = site.SiteTitle, desc = site.SiteDesc }, JsonRequestBehavior.AllowGet);
            return null;
        }
        [AjaxOnly]
        public virtual JsonResult GetFeeds(int siteId)
        {
            var context = new TazehaContext();
            var feeds = context.Sites.SingleOrDefault(s => s.Id == siteId).Feeds;
            if (feeds != null)
                return Json(feeds.Select(f => new { title = f.Title, link = f.Link }).ToList(), JsonRequestBehavior.AllowGet);
            return null;
        }

        [AjaxOnly]
        public virtual JsonResult GetCats(ViewMode viewMode)
        {
            var context = new TazehaContext();
            var cats =
                context.Categories.Where(c => viewMode.ToString().Contains(c.ViewMode.ToString()))
                    .Select(c => new { title = c.Title, code = c.Code });
            return Json(cats.ToList(), JsonRequestBehavior.AllowGet);
            return null;
        }
    }
}
