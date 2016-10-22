using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Entities.SiteMap;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.DomainClasses.ContentManagment;
using Mn.NewsCms.Web.WebLogic;

namespace Mn.NewsCms.Web.Controllers
{
    public partial class SitemapController : BaseController
    {
        private readonly IFeedItemBusiness _feedItemBusiness;

        public SitemapController(IFeedItemBusiness feedItemBusiness)
        {
            _feedItemBusiness = feedItemBusiness;
        }

        [OutputCache(Duration = CmsConfig.Cache3Hour)]
        public virtual XmlSitemapResult sitemap()
        {
            List<SitemapItem> items = new List<SitemapItem>();
            var inCache = HttpContext.Cache.Get("sitemapItems");
            if (inCache != null)
                return new XmlSitemapResult((List<SitemapItem>)inCache);
            TazehaContext context = new TazehaContext();
            var baseAddress = "http://" + Resources.Core.SiteUrl;

            items.Add(new SitemapItem(baseAddress, DateTime.UtcNow.AddMinutes(-10), ChangeFrequency.Hourly, .8f));
            items.Add(new SitemapItem(baseAddress + "/home/about", DateTime.UtcNow.AddDays(-15), ChangeFrequency.Monthly, .7f));
            items.Add(new SitemapItem(baseAddress + "/home/contact", DateTime.UtcNow.AddDays(-15), ChangeFrequency.Monthly, .5f));
            items.Add(new SitemapItem(baseAddress + "/home/ads", DateTime.UtcNow.AddDays(-15), ChangeFrequency.Monthly, .4f));
            items.Add(new SitemapItem(baseAddress + "/home/techs", DateTime.UtcNow.AddDays(-15), ChangeFrequency.Monthly, .4f));
            items.Add(new SitemapItem(baseAddress + "/account/register", DateTime.UtcNow.AddDays(-15), ChangeFrequency.Monthly, .5f));
            items.Add(new SitemapItem(baseAddress + "/toolbox/demo", DateTime.UtcNow.AddDays(-15), ChangeFrequency.Weekly, .7f));
            items.Add(new SitemapItem(baseAddress + "/toolbox", DateTime.UtcNow.AddDays(-15), ChangeFrequency.Weekly, .6f));
            items.Add(new SitemapItem(baseAddress + "/site/all", DateTime.UtcNow.AddDays(-15), ChangeFrequency.Weekly, .7f));
            items.Add(new SitemapItem(baseAddress + "/tag/all", DateTime.UtcNow.AddDays(-15), ChangeFrequency.Weekly, .8f));
            items.Add(new SitemapItem(baseAddress + "/items/now", DateTime.UtcNow.AddMinutes(-1), ChangeFrequency.Always, .3f));
            items.Add(new SitemapItem(baseAddress + "/items/todaymostvisited", DateTime.UtcNow.AddHours(-1), ChangeFrequency.Hourly, .5f));
            items.Add(new SitemapItem(baseAddress + "/photo/today", DateTime.UtcNow.AddMinutes(-30), ChangeFrequency.Hourly, .5f));

            foreach (var cat in context.Categories.Where(c => c.ViewMode == Common.Share.ViewMode.Menu || c.ViewMode == Common.Share.ViewMode.MenuIndex))
            {
                items.Add(new SitemapItem(baseAddress + "/cat/" + cat.Code, DateTime.UtcNow.AddMinutes(-10), ChangeFrequency.Hourly, .7f));
            }
            foreach (var tag in context.Tags)
            {
                items.Add(new SitemapItem(baseAddress + "/tag/" + HttpUtility.UrlEncode(tag.Title), DateTime.UtcNow.AddMinutes(-15), ChangeFrequency.Daily, .6f));
            }

            var feedtop = context.Feeds.Where(x => !x.Site.SkipType.HasValue || x.Site.SkipType == 0).OrderBy(x => x.Id).Take(100).Select(x => x.SiteId).ToList();
            var sites = context.Sites.Where(x => feedtop.Contains(x.Id)).Distinct().Shuffle().ToList();
            foreach (var site in sites)
            {
                items.Add(new SitemapItem(baseAddress + "/site/" + site.SiteUrl, DateTime.UtcNow.AddHours(5), ChangeFrequency.Daily, .5f));
            }

            var Hour = DateTime.UtcNow.NowHour();
            var maxItems = 100;
            var posts = _feedItemBusiness.FeedItemsByTime(DateTime.UtcNow.AddHours(Hour), maxItems, 0);
            for (int i = 1; i < 8; i++)
            {
                if (posts.Count() < (maxItems * 2) / 3)
                {
                    Hour = DateTime.UtcNow.NowHour() - i;
                    var res2 = _feedItemBusiness.FeedItemsByTime(DateTime.UtcNow.AddHours(Hour), maxItems - posts.Count(), 0);
                    posts = posts.Concat(res2).ToList();
                }
                else
                    break;
            }
            foreach (FeedItem post in posts)
            {
                items.Add(new SitemapItem(baseAddress + "/site/" + post.SiteUrl + "/" + post.Id + "/" + HttpUtility.UrlEncode(post.Title.RemoveBadCharacterInURL()), post.PubDate, ChangeFrequency.Yearly, .3f));
            }

            HttpContext.Cache.Insert("sitemapItems", items, null, DateTime.UtcNow.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
            return new XmlSitemapResult(items);
        }
        public class XmlSitemapResult : ActionResult
        {
            private static readonly XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            private IEnumerable<ISitemapItem> _items;

            public XmlSitemapResult(IEnumerable<ISitemapItem> items)
            {
                _items = items;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                string encoding = context.HttpContext.Response.ContentEncoding.WebName;
                XDocument sitemap = new XDocument(new XDeclaration("1.0", encoding, "yes"),
                new XElement(xmlns + "urlset",
                from item in _items
                select CreateItemElement(item)
                )
                );

                context.HttpContext.Response.ContentType = "application/rss+xml";
                context.HttpContext.Response.Flush();
                context.HttpContext.Response.Write(sitemap.Declaration + sitemap.ToString());
            }

            private XElement CreateItemElement(ISitemapItem item)
            {
                XElement itemElement = new XElement(xmlns + "url", new XElement(xmlns + "loc", item.Url.ToLower()));

                if (item.LastModified.HasValue)
                    itemElement.Add(new XElement(xmlns + "lastmod", item.LastModified.Value.ToString("yyyy-MM-dd")));

                if (item.ChangeFrequency.HasValue)
                    itemElement.Add(new XElement(xmlns + "changefreq", item.ChangeFrequency.Value.ToString().ToLower()));

                if (item.Priority.HasValue)
                    itemElement.Add(new XElement(xmlns + "priority", item.Priority.Value.ToString(CultureInfo.InvariantCulture)));

                return itemElement;
            }
        }
    }
}
