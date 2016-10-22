
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.DomainClasses.ContentManagment;
using Mn.NewsCms.Web.Models;
using Mn.NewsCms.Web.WebLogic;

namespace Mn.NewsCms.Web.Controllers.Api
{
    public class ItemsController : ApiController
    {
        public struct ItemResult
        {
            public string FeedItemId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string PersianDate { get; set; }
            public string SiteUrl { get; set; }
            public string SiteTitle { get; set; }
        }
        // GET: Items
        public FeedItem Get(string feedItemId)
        {
            return Ioc.ItemBiz.Get(feedItemId).ToViewModel<FeedItem>();
        }
        [OutputCache(Duration = 1000, VaryByParam = "content;pageIndex;pageSize")]
        public List<FeedItem> Get(string content, int pageIndex, int pageSize)
        {
            if (content.Contains("."))
            {
                var site = Ioc.SiteBiz.Get(content);
                if (site != null)
                    return Ioc.ItemBiz.FeedItemsBySite(site.Id, pageSize, pageIndex).ToList();
            }
            else
            {
                var cat = Ioc.CatBiz.Get(content);
                if (cat != null)
                    return Ioc.ItemBiz.FeedItemsByCat(cat.Id, pageSize, pageIndex).ToList();

                var tag = Ioc.TagBiz.Get(content);
                if (tag != null)
                    return Ioc.ItemBiz.FeedItemsByTag(tag, pageSize, pageIndex).ToList();
                else
                    return Ioc.ItemBiz.FeedItemsByKey(content, pageSize, pageIndex).ToList();
            }
            return null;
        }
        public List<ItemResult> Get(string content, int pageIndex, int pageSize, string type)
        {
            var result = new List<FeedItem>();
            if (type.ToLower() == "cat")
            {
                var cat = Ioc.CatBiz.Get(content);
                result = Ioc.ItemBiz.FeedItemsByCat(cat.Id, pageSize, pageIndex).ToList();
            }
            else if (type.ToLower() == "tag")
            {
                var tag = Ioc.TagBiz.Get(content);
                result = Ioc.ItemBiz.FeedItemsByTag(tag, pageSize, pageIndex).ToList();
            }
            else if (type.ToLower() == "site")
            {
                var site = Ioc.SiteBiz.Get(content);
                result = Ioc.ItemBiz.FeedItemsBySite(site.Id, pageSize, pageIndex).ToList();
            }
            else
            {
                result = Ioc.ItemBiz.FeedItemsByKey(content, pageSize, pageIndex).ToList();
            }

            return result.Select(item => new ItemResult
             {
                 FeedItemId = item.Id.ToString(),
                 Title = item.Title,
                 Description = item.Description,
                 PersianDate = Utility.GetDateTimeHtml(item.PubDate),
                 SiteTitle = item.SiteTitle,
                 SiteUrl = item.SiteUrl
             }).ToList();

        }

    }
}