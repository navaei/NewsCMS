using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Mn.NewsCms.Common;

namespace Mn.NewsCms.Web.Controllers.Api
{
    public class ItemsController : ApiController
    {
        private readonly IFeedItemBusiness _feedItemBusiness;
        private readonly ISiteBusiness _siteBusiness;
        private readonly ICategoryBusiness _categoryBusiness;
        private readonly ITagBusiness _tagBusiness;

        public ItemsController(IFeedItemBusiness feedItemBusiness, ISiteBusiness siteBusiness, ICategoryBusiness categoryBusiness, ITagBusiness tagBusiness)
        {
            _feedItemBusiness = feedItemBusiness;
            _siteBusiness = siteBusiness;
            _categoryBusiness = categoryBusiness;
            _tagBusiness = tagBusiness;
        }

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
            return _feedItemBusiness.Get(feedItemId).ToViewModel<FeedItem>();
        }
        [OutputCache(Duration = 1000, VaryByParam = "content;pageIndex;pageSize")]
        public List<FeedItem> Get(string content, int pageIndex, int pageSize)
        {
            if (content.Contains("."))
            {
                var site = _siteBusiness.Get(content);
                if (site != null)
                    return _feedItemBusiness.FeedItemsBySite(site.Id, pageSize, pageIndex).ToList();
            }
            else
            {
                var cat = _categoryBusiness.Get(content);
                if (cat != null)
                    return _feedItemBusiness.FeedItemsByCat(cat.Id, pageSize, pageIndex).ToList();

                var tag = _tagBusiness.Get(content);
                if (tag != null)
                    return _feedItemBusiness.FeedItemsByTag(tag, pageSize, pageIndex).ToList();
                else
                    return _feedItemBusiness.FeedItemsByKey(content, pageSize, pageIndex).ToList();
            }
            return null;
        }
        public List<ItemResult> Get(string content, int pageIndex, int pageSize, string type)
        {
            var result = new List<FeedItem>();
            if (type.ToLower() == "cat")
            {
                var cat = _categoryBusiness.Get(content);
                result = _feedItemBusiness.FeedItemsByCat(cat.Id, pageSize, pageIndex).ToList();
            }
            else if (type.ToLower() == "tag")
            {
                var tag = _tagBusiness.Get(content);
                result = _feedItemBusiness.FeedItemsByTag(tag, pageSize, pageIndex).ToList();
            }
            else if (type.ToLower() == "site")
            {
                var site = _siteBusiness.Get(content);
                result = _feedItemBusiness.FeedItemsBySite(site.Id, pageSize, pageIndex).ToList();
            }
            else
            {
                result = _feedItemBusiness.FeedItemsByKey(content, pageSize, pageIndex).ToList();
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