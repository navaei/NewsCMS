using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tazeyab.Common;
using Tazeyab.Common.EventsLog;
using Tazeyab.Common.Models;
using Tazeyab.Common.Updater;
using Tazeyab.DomainClasses.UpdaterBusiness;

namespace Tazeyab.WinUpdater
{
    public class WinBaseServer : IBaseServer
    {
        #region Properties
        static int NumberOfNewItemsToday = 0;
        static int Last_NumberOfNewItemsToday = 0;
        TazehaContext context = new TazehaContext();
        #endregion
        public bool SendFeedItems(List<FeedItemSP> items)
        {
            IRepositorySaver _LuceneRepository = new LuceneSaverRepository();
            _LuceneRepository.AddItems(items.ToList());
            return true;
        }
        public bool SendFeeds(List<FeedContract> feeds)
        {
            List<FeedItemSP> items = feeds.SelectMany(feed => feed.FeedItems.Select(item => new FeedItemSP
            {
                Cats = !string.IsNullOrEmpty(feed.Cats) ? feed.Cats.Split(' ').Select(x => int.Parse(x)) : null,
                CreateDate = DateTime.Now,
                Description = item.Description,
                Link = item.Link,
                PubDate = item.PubDate,
                SiteId = feed.SiteId,
                SiteTitle = feed.SiteTitle,
                SiteUrl = feed.SiteUrl,
                Title = item.Title
            })).ToList();

            IRepositorySearcher luceneSearcher = new LuceneSearcherRepository();
            IRepositorySaver saver = new LuceneSaverRepository();
            saver.AddItems(items);
            //var dics = feeds.ToDictionary(x => x.FeedId, c => c.LastFeedItemUrl);
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
            LuceneBase _LuceneRepository = new LuceneSaverRepository();
            _LuceneRepository.Optimize();
            GeneralLogs.WriteLogInDB("Optimize[AsService] ", TypeOfLog.Info);
        }

        #region UpdateFeed&Item
        public bool UpdateFeeds(Dictionary<long, string> feeds)
        {
            throw new NotImplementedException();
            //var dbFeeds = context.Feeds.Where(x => feeds.Any(f => f.Key == x.FeedId)).ToList();
            //foreach (var dbfeed in dbFeeds)
            //{
            //    dbfeed.LastUpdatedItemUrl = feeds.Single(x => x.Key == dbfeed.FeedId).Value;
            //    if (!string.IsNullOrEmpty(dbfeed.LastUpdatedItemUrl))
            //    {
            //        dbfeed.UpdatingCount = dbfeed.UpdatingCount == null ? 1 : dbfeed.UpdatingCount + 1;
            //        dbfeed.LastUpdaterVisit = DateTime.Now;
            //        base.CheckForChangeDuration(dbfeed, true);
            //    }
            //    else
            //        base.CheckForChangeDuration(dbfeed, false);

            //    dbfeed.LastUpdaterVisit = DateTime.Now;
            //    context.SaveChanges();
            //    GeneralLogs.WriteLog("UpdateFeed[AsService] " + dbfeed.Link, TypeOfLog.OK);
            //}
            //return true;
        }
        public bool UpdateFeeds(List<FeedContract> feeds)
        {

            var ids = feeds.Select(x => x.FeedId).ToList();
            var dbFeeds = context.Feeds.Where(x => ids.Any(f => f == x.FeedId));
            foreach (var dbfeed in dbFeeds)
            {
                dbfeed.LastUpdatedItemUrl = feeds.FirstOrDefault(x => x.FeedId == dbfeed.FeedId).LastFeedItemUrl;
                if (!string.IsNullOrEmpty(dbfeed.LastUpdatedItemUrl) && feeds.SingleOrDefault(f => f.FeedId == dbfeed.FeedId).FeedItems.Count > 0)
                {
                    dbfeed.UpdatingCount = dbfeed.UpdatingCount == null ? 1 : dbfeed.UpdatingCount + 1;
                    dbfeed.LastUpdateDateTime = DateTime.Now;
                    BaseUpdater.CheckForChangeDuration(dbfeed, true);
                }
                else
                    BaseUpdater.CheckForChangeDuration(dbfeed, false);

                dbfeed.LastUpdaterVisit = DateTime.Now;
            }
            context.SaveChanges();
            GeneralLogs.WriteLog("UpdateFeeds[AsService] : " + string.Join("[br /]", dbFeeds.Select(x => x.Link)), TypeOfLog.OK);
            return true;
        }
        #endregion
        public List<FeedContract> getLatestFeeds(int MaxSize, bool? IsLocaly)
        {

            GeneralLogs.WriteLog("GetLatestFeeds[AsService] MaxSize=" + MaxSize + "IsLocaly:" + IsLocaly, TypeOfLog.Info);
            return BaseUpdaterServer<string>.getCandidateFeeds(MaxSize, IsLocaly);


        }
        public List<FeedContract> getLatestFeedsByDuration(string DurationCode, int MaxSize, int IsBlog)
        {
            UpdateDuration duration = UpdateDurationManager.getLast(DurationCode, MaxSize);
            TazehaContext context = new TazehaContext();
            GeneralLogs.WriteLog("getLatestFeedsByDuration :" + DurationCode + " StartIndex:" + duration.StartIndex, TypeOfLog.Info);
            List<Feed> arr = new List<Feed>();

            arr = context.Feeds.Where<Feed>(x => x.UpdateDurationId.Value == duration.UpdateDurationId &&
                  (x.Site.IsBlog == IsBlog || IsBlog == 2) &&
                  (x.Deleted == 0 || x.Deleted > 10)).OrderBy(x => x.FeedId).Skip(duration.StartIndex).Take<Feed>(MaxSize).ToList();

            var res = arr.ConvertToFeedContract().ToList();
            //GeneralLogs.WriteLogInDB(string.Format("Return {0} Feed to remote client,start index {1}", res.Count(), duration.StartIndex), TypeOfLog.Info);
            //System.Web.HttpRuntime.Cache.AddToChache_Hours("Duration_" + DurationCode, duration.StartIndex, 12);
            return res;
        }
    }
}
