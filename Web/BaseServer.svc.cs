using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Updater;
using System.Web;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.DomainClasses.UpdaterBusiness;



namespace Mn.NewsCms.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BaseService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BaseService.svc or BaseService.svc.cs at the Solution Explorer and start debugging.
    public class BaseServer : IBaseServer
    {
        private readonly IAppConfigBiz _appConfigBiz;
        private readonly IFeedBusiness _feedBusiness;
        private readonly IFeedItemBusiness _feedItemBusiness;
        private readonly IUpdaterDurationBusiness _updaterDurationBusiness;

        #region Properties
        static int NumberOfNewItemsToday = 0;
        static int Last_NumberOfNewItemsToday = 0;
        //TazehaContext context = new TazehaContext();
        #endregion

        public BaseServer(IAppConfigBiz appConfigBiz, IFeedBusiness feedBusiness, IFeedItemBusiness feedItemBusiness, IUpdaterDurationBusiness updaterDurationBusiness)
        {
            _appConfigBiz = appConfigBiz;
            _feedBusiness = feedBusiness;
            _feedItemBusiness = feedItemBusiness;
            _updaterDurationBusiness = updaterDurationBusiness;
        }

        public bool SendFeedItems(List<FeedItem> items)
        {
            IRepositorySaver _LuceneRepository = new LuceneSaverRepository();
            _LuceneRepository.AddItems(items.ToList());
            return true;
        }
        public bool SendFeeds(List<FeedContract> feeds)
        {
            List<FeedItem> items = feeds.SelectMany(feed => feed.FeedItems.Select(item => new FeedItem
            {
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                Description = item.Description,
                Link = item.Link,
                PubDate = item.PubDate,
                SiteId = feed.SiteId,
                FeedId = feed.Id,
                SiteTitle = feed.SiteTitle,
                SiteUrl = feed.SiteUrl,
                Title = item.Title,
                ShowContentType = feed.ShowContentType
            })).ToList();

            //IRepositorySaver saver = new LuceneSaverRepository();           
            //saver.AddItems(items);
            _feedItemBusiness.AddItems(items);

            UpdateFeeds(feeds);

            NumberOfNewItemsToday += items.Count;

            //if (NumberOfNewItemsToday - Last_NumberOfNewItemsToday > 50)
            //{
            //    Optimize();
            //    Last_NumberOfNewItemsToday = NumberOfNewItemsToday;
            //}
            //Optimize();
            return true;

        }
        public void Optimize()
        {
            var _LuceneRepository = new LuceneSaverRepository();
            _LuceneRepository.Optimize();
            GeneralLogs.WriteLogInDB("Optimize[AsService] ", TypeOfLog.Info);
        }

        #region UpdateFeed&Item
        public bool UpdateFeeds(Dictionary<long, string> feeds)
        {
            throw new NotImplementedException();
        }
        public bool UpdateFeeds(List<FeedContract> feeds)
        {
            //var context = new TazehaContext();
            var ids = feeds.Select(x => x.Id).ToList();
            var dbFeeds = _feedBusiness.GetList().Where(x => ids.Any(f => f == x.Id)).ToList();
            foreach (var dbfeed in dbFeeds)
            {
                dbfeed.LastUpdatedItemUrl = feeds.FirstOrDefault(x => x.Id == dbfeed.Id).LastFeedItemUrl;
                if (!string.IsNullOrEmpty(dbfeed.LastUpdatedItemUrl) && feeds.SingleOrDefault(f => f.Id == dbfeed.Id).FeedItems.Count > 0)
                {
                    dbfeed.UpdatingCount = dbfeed.UpdatingCount == null ? 1 : dbfeed.UpdatingCount + 1;
                    dbfeed.LastUpdateDateTime = DateTime.Now;
                    _feedBusiness.CheckForChangeDuration(dbfeed, true);
                }
                else
                    _feedBusiness.CheckForChangeDuration(dbfeed, false);

                dbfeed.LastUpdaterVisit = DateTime.Now;

                _feedBusiness.Edit(dbfeed);
            }
            //context.SaveChanges();
            GeneralLogs.WriteLog("UpdateFeeds[AsService] : " + string.Join("[br /]", dbFeeds.Select(x => x.Link)), TypeOfLog.OK);
            return true;
        }
        #endregion
        public List<FeedContract> getLatestFeeds(int MaxSize, bool? IsLocaly)
        {
            GeneralLogs.WriteLog("GetLatestFeeds[AsService] MaxSize=" + MaxSize + "IsLocaly:" + IsLocaly, TypeOfLog.Info);
            return BaseUpdaterServer<string>.getCandidateFeeds(MaxSize, IsLocaly);
        }
        public List<FeedContract> getLatestFeedsByDuration(string DurationCode, int MaxSize, bool IsBlog)
        {
            var duration = _updaterDurationBusiness.GetLast(DurationCode, MaxSize);
            var context = new TazehaContext();
            GeneralLogs.WriteLog("getLatestFeedsByDuration :" + DurationCode + " StartIndex:" + duration.StartIndex, TypeOfLog.Info);
            var arr = new List<Feed>();

            arr = context.Feeds.Where<Feed>(x => x.UpdateDurationId.Value == duration.Id &&
                  (x.Site.IsBlog == IsBlog) &&
                  (x.Deleted == 0 || (int)x.Deleted > 10)).OrderBy(x => x.Id).Skip(duration.StartIndex).Take<Feed>(MaxSize).ToList();

            var res = arr.ConvertToFeedContract().ToList();
            System.Web.HttpRuntime.Cache.AddToChache_Hours("Duration_" + DurationCode, duration.StartIndex, 12);
            return res;
        }
    }
}
