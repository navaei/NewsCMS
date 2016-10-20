using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Helper;
using Mn.NewsCms.DomainClasses.UpdaterBusiness;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.Common.EventsLog;

namespace Mn.NewsCms.DomainClasses
{
    public class FeedItemBusiness : BaseBusiness<FeedItem, Guid>, IFeedItemBusiness
    {
        const int MinCacheTime = 3;
        const int DefaultCacheTime = 20;
        ITagBusiness TagBiz;
        ICategoryBusiness CatBiz;
        private readonly IAppConfigBiz _appConfigBiz;
        private readonly IRepositorySaver _repositorySaver;
        List<FeedItem> res;
        public FeedItemBusiness(IUnitOfWork unitOfWork, ITagBusiness tagBusiness, ICategoryBusiness categoryBusiness,
            IAppConfigBiz appConfigBiz, IRepositorySaver repositorySaver) : base(unitOfWork)
        {
            TagBiz = tagBusiness;
            CatBiz = categoryBusiness;
            _appConfigBiz = appConfigBiz;
            _repositorySaver = repositorySaver;
        }
        public FeedItem Get(string feedItemId)
        {
            long itemId;
            if (long.TryParse(feedItemId, out itemId))
                return GetList().SingleOrDefault(i => i.ItemId == itemId);
            else
            {
                Guid id;
                if (Guid.TryParse(feedItemId, out id))
                    return Get(id);
            }
            return null;
        }
        public FeedItem Get(long itemId)
        {
            return GetList().SingleOrDefault(i => i.ItemId == itemId);
        }
        public FeedItem Get(Guid feedItemId)
        {
            var item = base.GetList().SingleOrDefault(i => i.Id == feedItemId);
            if (item == null)
            {
                IRepositorySearcher _LuceneRepository = new LuceneSearcherRepository();
                item = _LuceneRepository.GetItem(feedItemId.ToString(), null, true);
            }
            return item;
        }
        public IQueryable<FeedItem> GetList()
        {
            return base.GetList();
        }
        public OperationStatus Update(FeedItem item)
        {
            return base.Update(item);
        }

        public List<FeedItem> DescriptClear(List<FeedItem> Items, string Key)
        {
            if (Items == null)
                return Items;
            foreach (var item in Items)
            {
                item.Description = HtmlRemoval.StripTagsCharArray(item.Description);
                item.Description = item.Description.SubstringX(0, _appConfigBiz.MaxDescriptionLength());
            }
            return Items;
        }

        public void IncreaseVisitCount(List<FeedItem> Items)
        {
            try
            {
                foreach (var item in Items.ToList())
                {
                    item.VisitsCount = item.VisitsCount + 1;
                    base.Update(item);
                }
            }
            catch
            {
            }
        }
        public List<FeedItem> FeedItemsBySite(long siteId, int PageSize, int PageIndex, int CacheTime = MinCacheTime)
        {
            return FeedItemsBySite(siteId, PageSize, PageIndex, null, CacheTime);
        }
        public List<FeedItem> FeedItemsBySite(long siteId, int PageSize, int PageIndex, bool? hasPhoto, int CacheTime = MinCacheTime)
        {
            var cacheName = hasPhoto.HasValue ? "ItemsSite" + siteId + hasPhoto + PageIndex + PageSize : "ItemsSite" + siteId + PageIndex + PageSize;
            var inCache = System.Web.HttpRuntime.Cache.Get(cacheName);
            if (inCache != null)
                return inCache as List<FeedItem>;

            List<FeedItem> res;
            if (hasPhoto.HasValue)
                res = base.GetList().Where(item => item.SiteId == siteId && item.HasPhoto == hasPhoto.Value)
                       .OrderByDescending(item => item.PubDate).Skip(PageIndex * PageSize).Take(PageSize).ToList();
            else
                res = base.GetList().Where(item => item.SiteId == siteId)
                       .OrderByDescending(item => item.PubDate).Skip(PageIndex * PageSize).Take(PageSize).ToList();

            System.Web.HttpRuntime.Cache.AddToCache(cacheName, res, CacheTime);
            return res;

        }
        public List<FeedItem> FeedItemsBySite(string SiteUrl, int PageSize, int PageIndex, int CacheTime = MinCacheTime)
        {
            var cacheName = "ItemsSite" + SiteUrl + PageIndex + PageSize;
            var inCache = System.Web.HttpRuntime.Cache.Get(cacheName);
            if (inCache != null)
                return inCache as List<FeedItem>;

            res = base.GetList().Where(item => item.SiteUrl == SiteUrl.ToLower())
                 .OrderByDescending(item => item.PubDate).Skip(PageIndex * PageSize).Take(PageSize).ToList();

            System.Web.HttpRuntime.Cache.AddToCache(cacheName, res, CacheTime);

            return res;

        }
        public List<FeedItem> FeedItemsByCat(long catId, int PageSize, int PageIndex, int CacheTime = MinCacheTime)
        {
            return FeedItemsByCat(catId, PageSize, PageIndex, null, CacheTime);
        }
        public List<FeedItem> FeedItemsByCat(string code, int PageSize, int PageIndex, bool? hasPhoto, int CacheTime = MinCacheTime)
        {
            var cat = CatBiz.Get(code);
            return FeedItemsByCat(cat.Id, PageSize, PageIndex, hasPhoto.HasValue ? hasPhoto : false, CacheTime);
        }
        public List<FeedItem> FeedItemsByCat(long catId, int PageSize, int PageIndex, bool? hasPhoto, int CacheTime = MinCacheTime)
        {
            var cacheName = hasPhoto.HasValue ? "ItemsCat" + catId + hasPhoto + PageSize + PageIndex : "ItemsCat" + catId + PageSize + PageIndex;
            var inCache = System.Web.HttpRuntime.Cache.Get(cacheName);
            if (inCache != null)
                return inCache as List<FeedItem>;

            List<FeedItem> res;
            if (hasPhoto.HasValue)
                res = base.GetList().Where(item => item.HasPhoto == hasPhoto && item.Feed.Categories.Any(c => c.Id == catId || c.ParentId == catId))
                      .OrderByDescending(item => item.PubDate).Skip(PageIndex * PageSize).Take(PageSize).ToList();
            else
                res = base.GetList().Where(item => item.Feed.Categories.Any(c => c.Id == catId || c.ParentId == catId))
                  .OrderByDescending(item => item.PubDate).Skip(PageIndex * PageSize).Take(PageSize).ToList();

            System.Web.HttpRuntime.Cache.AddToCache(cacheName, res, CacheTime);
            return res;
        }
        public List<FeedItem> FeedItemsByCat(ref string content, int PageSize, int PageIndex, int CacheTime = MinCacheTime)
        {
            var cat = CatBiz.Get(content);
            content = cat.Title;
            return FeedItemsByCat(cat.Id, PageSize, PageIndex, null);
        }
        public List<FeedItem> FeedItemsByTag(string Content, int PageSize, int PageIndex, ref string TagTitle, int CacheTime = MinCacheTime)
        {
            var TagCurrent = new Tag();
            TagCurrent = TagBiz.Get(Content.Trim());
            if (TagCurrent == null)
                if (TagBiz.GetList(Content).Any())
                    TagCurrent = TagBiz.GetList(Content).First();

            if (TagCurrent != null)
            {
                Content = TagCurrent.Value;
                TagTitle = TagCurrent.Title;
            }
            return ItemsByKey(Content, PageSize, PageIndex);
        }
        public List<FeedItem> FeedItemsByTag(Tag TagCurrent, int PageSize, int PageIndex, int CacheTime = MinCacheTime)
        {
            var Content = TagCurrent.Value;
            Content = TagCurrent.Value;
            return ItemsByKey(Content, PageSize, PageIndex);
        }
        public List<FeedItem> FeedItemsByTag(Tag TagCurrent, int PageSize, int PageIndex, bool HasPhoto, int CacheTime = MinCacheTime)
        {
            var Content = TagCurrent.Value;
            Content = TagCurrent.Value;
            return ItemsByKey(Content, PageSize, PageIndex, HasPhoto);
        }
        public List<FeedItem> FeedItemsByKey(string Content, int PageSize, int PageIndex, int CacheTime = MinCacheTime)
        {
            return ItemsByKey(Content, PageSize, PageIndex);
        }
        public List<FeedItem> FeedItemsByTime(DateTime dateTime, int PageSize, int PageIndex)
        {
            return base.GetList().Where(item => item.PubDate >= dateTime)
                  .OrderByDescending(item => item.PubDate).Skip(PageIndex * PageSize).Take(PageSize).ToList();
        }
        public List<FeedItem> FeedItemsByFeed(long feedId, int PageSize, int PageIndex)
        {
            return base.GetList().Where(item => item.FeedId == feedId)
             .OrderByDescending(item => item.PubDate).Skip(PageIndex * PageSize).Take(PageSize).ToList();
        }

        public void DeleteOldItems(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        private List<FeedItem> ItemsByKey(string Content, int PageSize, int PageIndex, int CacheTime = MinCacheTime)
        {
            return ItemsByKey(Content, PageSize, PageIndex, false, CacheTime);
        }
        private List<FeedItem> ItemsByKey(string Content, int PageSize, int PageIndex, bool HasPhoto, int CacheTime = MinCacheTime)
        {
            var cacheName = "Items" + Content + PageSize + PageIndex + HasPhoto;
            var inCache = System.Web.HttpRuntime.Cache.Get(cacheName);
            if (inCache != null)
                return inCache as List<FeedItem>;

            IRepositorySearcher _LuceneRepository = new LuceneSearcherRepository();
            var items = _LuceneRepository.Search(Content, PageSize, PageIndex, Mn.NewsCms.Common.LuceneBase.LuceneSearcherType.Key, Mn.NewsCms.Common.LuceneBase.LuceneSortField.PubDate, HasPhoto);
            res = GetList().Where(item => items.Contains(item.Id)).ToList();

            System.Web.HttpRuntime.Cache.AddToCache(cacheName, res, CacheTime);
            return res;
        }

        #region MostVisited

        public IEnumerable<FeedItem> LastItemVisited(int counter = 20, int index = 0)
        {
            var dudate = DateTime.Now.AddHours(-1);
            var res = base.GetList().Where(i => i.CreateDate > dudate).OrderByDescending(i => i.PubDate).Skip(index).Take(counter).ToList();
            if (res == null || !res.Any())
            {
                res = base.GetList().OrderByDescending(i => i.PubDate).Skip(index).Take(counter).ToList();
            }
            return res;
        }
        public IEnumerable<FeedItem> MostVisitedItems(string EntityCode, int EntityRef, int PageSize, int CacheTime = MinCacheTime)
        {
            List<FeedItem> res;
            var inCache = System.Web.HttpRuntime.Cache.Get("ItemsTop" + EntityCode);
            if (inCache != null)
            {
                res = inCache as List<FeedItem>;
                if (res.Count >= PageSize)
                    return res.Take(PageSize);
            }

            if (EntityCode.EqualsX("Any") || EntityCode.EqualsX("Key") || EntityRef == 0)
                res = base.GetList().OrderByDescending(x => x.VisitsCount)
                        .Take(PageSize).ToList();
            else if (EntityCode.EqualsX("today"))
            {
                var date = DateTime.Now.AddDays(-1);
                res = base.GetList().Where(i => i.PubDate > date).
                          OrderByDescending(x => x.VisitsCount).
                          Take(PageSize).ToList();
            }
            else //if (EntityCode.EqualsX("todayall"))
            {
                var date = DateTime.Now.AddDays(-1);
                res = base.GetList().Where(i => i.PubDate > date).
                          OrderByDescending(x => x.VisitsCount).
                          Take(PageSize).ToList();
            }

            System.Web.HttpRuntime.Cache.AddToCache("ItemsTop" + EntityCode, res, CacheTime);
            return res;
        }
        #endregion
        #region DeleteOldItems        
        #endregion
        public int AddItems(List<FeedItem> items)
        {
            var res = AddItemsToDataBase(items).Split(new char[] { ':' });
            if (res.Any())
            {
                items = items.Where(t => res.Contains(t.Id.ToString())).ToList();
                if (items.Any())
                    _repositorySaver.AddItems(items);
            }
            return res.Count();
        }

        private string AddItemsToDataBase(List<FeedItem> Items)
        {
            var res = string.Empty;
            try
            {
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["TazehaContext"].ConnectionString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand("exec sp_FeedItemsCreate @list", con))
                    {
                        var table = new DataTable();
                        table.Columns.Add("FeedItemId", typeof(string));
                        table.Columns.Add("Title", typeof(string));
                        table.Columns.Add("Link", typeof(string));
                        table.Columns.Add("HasPhoto", typeof(bool));
                        table.Columns.Add("Description", typeof(string));
                        table.Columns.Add("PubDate", typeof(DateTime));
                        table.Columns.Add("FeedId", typeof(int));
                        table.Columns.Add("SiteId", typeof(long));
                        table.Columns.Add("SiteUrl", typeof(string));
                        table.Columns.Add("SiteTitle", typeof(string));
                        table.Columns.Add("VisitsCount", typeof(int));
                        table.Columns.Add("ShowContentType", typeof(byte));
                        foreach (var item in Items)
                            table.Rows.Add(item.Id,
                                item.Title.SubstringM(0, 256).ApplyUnifiedYeKe(),
                                HttpUtility.UrlDecode(item.Link.SubstringM(0, 256)).ApplyUnifiedYeKe(),
                                item.HasPhoto,
                                item.Description.SubstringM(0, 512),
                                item.PubDate,
                                item.FeedId,
                                item.SiteId,
                                item.SiteUrl.SubstringM(0, 64),
                                item.SiteTitle.SubstringM(0, 64).ApplyUnifiedYeKe(),
                                item.VisitsCount,
                                item.ShowContentType);

                        var pList = new SqlParameter("@list", SqlDbType.Structured);
                        pList.TypeName = "dbo.FeedItemDictionary1";
                        pList.Value = table;
                        cmd.Parameters.Add(pList);
                        res = cmd.ExecuteScalar().ToString();
                        if (!string.IsNullOrEmpty(res))
                            res = res.Remove(0, 1);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                GeneralLogs.WriteLogInDB(ex, typeof(FeedItemBusiness));
            }
            return res;
        }

        public void IncreaseVisitCount(Guid feedItemId)
        {
            SqlCommandExecute($"update feeditems set visitscount=visitscount+1 where feeditemid='{feedItemId}'");
        }
    }
}