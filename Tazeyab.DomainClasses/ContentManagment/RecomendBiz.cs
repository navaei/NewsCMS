using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tazeyab.Common;
using Tazeyab.Common.Models;
using Tazeyab.Common.Share;

namespace Tazeyab.DomainClasses.ContentManagment
{
    public class RecomendBiz : IRecomendBiz
    {
        int DefaultHourTime = 1;
        int MaxSelectItem = 3;
        int atLeastVisitCount = 20;
        #region private
        private List<SiteOnlyTitle> getSites(int TopCount)
        {
            if (HttpContext.Current.Cache["Recommendation_Sites"] != null)
            {
                var sites = HttpContext.Current.Cache["Recommendation_Sites"] as List<SiteOnlyTitle>;
                return sites.Take(TopCount).ToList();
            }
            TazehaContext context = new TazehaContext();
            DateTime date = DateTime.Now.AddDays(-1);
            var res = context.SearchHistories.Where(x => x.SiteId.HasValue && x.CreationDate >= date).
                GroupBy(x => x.SiteId).OrderByDescending(x => x.Count()).Select(x => x.Key);
            var temp = res.Take(TopCount).ToList();
            var siteList = context.Sites.Where(x => temp.Any(c => c.Value == x.Id)).Select(x => new SiteOnlyTitle { Id = x.Id, SiteUrl = x.SiteUrl, SiteTitle = x.SiteTitle }).ToList();//, SiteLogo = x.SiteLogo
            HttpContext.Current.Cache.AddToChache_Hours("Recommendation_Sites", siteList, DefaultHourTime);
            return siteList;
        }
        private List<CategoryModel> getCats(int TopCount)
        {
            if (HttpContext.Current.Cache["Recommendation_Cats"] != null)
            {
                var cats = HttpContext.Current.Cache["Recommendation_Cats"] as List<CategoryModel>;
                return cats.Take(TopCount).ToList();
            }
            //TazehaContext context = new TazehaContext();
            //DateTime date = DateTime.Now.AddDays(-1);
            //var res = context.SearchHistories.Where(x => x.CatId.HasValue && x.CreationDate >= date).
            //    GroupBy(x => x.Id).OrderByDescending(x => x.Count()).Select(x => x.Key);
            //var temp = res.Take(TopCount).ToList();
            //var list = context.Categories.Where(x => temp.Any(c => c == x.Id)).ToList();
            var list = ServiceFactory.Get<ICategoryBusiness>().GetList().Select(i => new CategoryModel()
            {
                Id = i.Id,
                Code = i.Code,
                Color = i.Color,
                Title = i.Title,
            }).Shuffle().Take(TopCount).ToList();
            HttpContext.Current.Cache.AddToChache_Hours("Recommendation_Cats", list, DefaultHourTime);
            return list;
        }
        private List<Tag> getTags(int TopCount)
        {
            if (HttpContext.Current.Cache["Recommendation_Tags"] != null)
            {
                var tags = HttpContext.Current.Cache["Recommendation_Tags"] as List<Tag>;
                return tags.Take(TopCount).ToList();
            }
            TazehaContext context = new TazehaContext();
            DateTime date = DateTime.Now.AddDays(-2);
            var res = context.SearchHistories.Where(x => x.TagId.HasValue && x.CreationDate >= date).
                GroupBy(x => x.TagId).OrderByDescending(x => x.Count()).Take(TopCount).Select(x => x.Key.Value).ToList();
            //var temp = res.Take(TopCount).ToList();
            var list = (from x in res
                        join t in context.Tags on x equals t.Id
                        select t).ToList();
            HttpContext.Current.Cache.AddToChache_Hours("Recommendation_Tags", list, DefaultHourTime);
            return list;
        }
        #endregion

        #region public
        public List<SiteOnlyTitle> Sites(int TopCount)
        {
            return getSites(TopCount * MaxSelectItem).Shuffle().Take(TopCount).ToList();

        }
        public List<CategoryModel> Cats(int TopCount)
        {
            return getCats(TopCount * MaxSelectItem).Shuffle().Take(TopCount).ToList();
        }
        public List<Tag> Tags(int TopCount)
        {
            return getTags(TopCount * MaxSelectItem).Shuffle().Take(TopCount).ToList();
        }
        #endregion

        #region User
        public List<SiteOnlyTitle> Sites(int TopCount, List<long> userSites)
        {
            var temps = getSites(TopCount * MaxSelectItem);
            temps.RemoveAll(x => userSites.Contains(x.Id));
            return temps.Take(TopCount).ToList();
        }
        public List<CategoryModel> Cats(int TopCount, List<long> userCats)
        {
            var temps = getCats(TopCount * MaxSelectItem);
            temps.RemoveAll(x => userCats.Contains(x.Id));
            return temps.Take(TopCount).ToList();
        }
        public List<Tag> Tags(int TopCount, List<long> userTags)
        {
            var temps = getTags(TopCount * MaxSelectItem);
            temps.RemoveAll(x => userTags.Contains(x.Id));
            return temps.Take(TopCount).ToList();
        }
        #endregion

        public List<FeedItems_Index> FeedItemsIndex(int TopCount)
        {

            var temp = FeedItems(TopCount);
            return temp.Select(x => new FeedItems_Index { Link = x.Link, Title = x.Title, SiteTitle = x.SiteTitle, PubDate = x.PubDate, FeedItemId = x.Id.ToString(), CreateDate = x.CreateDate }).ToList();
        }
        public List<FeedItem> FeedItems(int TopCount)
        {

            if (HttpContext.Current.Cache["Recommendation_FeedItems"] != null)
            {
                var items = HttpContext.Current.Cache["Recommendation_FeedItems"] as List<FeedItem>;
                return items.Shuffle().Take(TopCount).ToList();
            }
            TazehaContext context = new TazehaContext();
            DateTime date = DateTime.Now.AddDays(-3);
            var feeditems = context.FeedItems.Where(x => x.CreateDate >= date && x.VisitsCount > atLeastVisitCount).OrderByDescending(x => x.VisitsCount).Take(TopCount).ToList();
            HttpContext.Current.Cache.AddToChache_Hours("Recommendation_FeedItems", feeditems, DefaultHourTime);
            return feeditems.Take(TopCount).ToList();

        }
    }
}