using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Mn.NewsCms.Common.Models;
using OperationStatus = Mn.NewsCms.Common.BaseClass.OperationStatus;

namespace Mn.NewsCms.Common
{
    public interface IFeedItemBusiness
    {
        FeedItem Get(string feedItemId);
        FeedItem Get(long itemId);
        FeedItem Get(Guid feedItemId);
        IQueryable<FeedItem> GetList();
        OperationStatus Update(FeedItem item);
        List<FeedItem> DescriptClear(List<FeedItem> Items, string Key);
        void IncreaseVisitCount(List<FeedItem> Items);
        void IncreaseVisitCount(Guid feedItemId);
        List<FeedItem> FeedItemsByTag(Tag TagCurrent, int PageSize, int PageIndex, int CacheTime = 0);
        List<FeedItem> FeedItemsByTag(Tag TagCurrent, int PageSize, int PageIndex, bool HasPhoto, int CacheTime = 0);
        List<FeedItem> FeedItemsByTag(string Content, int PageSize, int PageIndex, ref string TagTitle, int CacheTime = 0);
        List<FeedItem> FeedItemsByCat(long catId, int PageSize, int PageIndex, int CacheTime = 0);
        List<FeedItem> FeedItemsByCat(long catId, int PageSize, int PageIndex, bool? hasPhoto, int CacheTime = 0);
        List<FeedItem> FeedItemsByCat(string code, int PageSize, int PageIndex, bool? hasPhoto, int CacheTime = 0);
        List<FeedItem> FeedItemsByCat(ref string Content, int PageSize, int PageIndex, int CacheTime = 0);
        List<FeedItem> FeedItemsByKey(string Content, int PageSize, int PageIndex, int CacheTime = 0);
        List<FeedItem> FeedItemsBySite(long siteId, int PageSize, int PageIndex, int CacheTime = 0);
        List<FeedItem> FeedItemsBySite(long siteId, int PageSize, int PageIndex, bool? hasPhoto, int CacheTime = 0);
        List<FeedItem> FeedItemsBySite(string SiteUrl, int PageSize, int PageIndex, int CacheTime = 0);
        List<FeedItem> FeedItemsByTime(DateTime lastDateTime, int PageSize, int PageIndex);
        List<FeedItem> FeedItemsByFeed(long feedId, int PageSize, int PageIndex);
        void DeleteOldItems(DateTime startDate, DateTime endDate);
        IEnumerable<FeedItem> MostVisitedItems(string EntityCode, int EntityRef, int PageSize, int cacheTime = 0);
        IEnumerable<FeedItem> LastItemVisited(int counter = 20, int index = 0);
        int AddItems(List<FeedItem> items);
    }
}
