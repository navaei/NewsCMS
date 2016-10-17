using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using Rss;
using Tazeyab.Common;
using Tazeyab.Common.EventsLog;
using Tazeyab.Common.Models;
using Tazeyab.CrawlerEngine.Repository;

namespace Tazeyab.CrawlerEngine.Updater
{

    public class UpdaterByDuration
    {
        #region property
        private string UpdaterName
        {
            get
            {
                return "IntegralUpdater";
            }
        }
        private int StartOfEndDate { get { return 1; } }
        private int EndOfEndDate { get { return 6; } }
        public bool StopUpdater
        {
            get
            {
                return (bool)System.Web.HttpRuntime.Cache["StopUpdater"];
            }
            set
            {
                System.Web.HttpRuntime.Cache["StopUpdater"] = value;
            }
        }
        #endregion

        #region Fields
        protected TazehaContext Context;
        int NumberOfNewItemsToday;
        UpdateDuration duration;
        #endregion

        #region PublicMethod
        private UpdaterByDuration()
        {
        }
        public UpdaterByDuration(UpdateDuration BaseDuration)
        {
            duration = BaseDuration;
        }
        public bool UpdatersIsRun()
        {
            if (System.Web.HttpRuntime.Cache[UpdaterName] != null)
            {
                return (bool)System.Web.HttpRuntime.Cache[UpdaterName];
            }
            else
            {
                return false;
            }

        }
        public void setIsRun(bool state)
        {
            System.Web.HttpRuntime.Cache[UpdaterName] = state;
        }
        public bool StartUpdater(StartUp inputParams)
        {
            if (!duration.IsParting.HasValue && duration.IsParting.Value)
                throw new Exception("Is parting set in " + UpdaterName);
            if (duration == null || string.IsNullOrEmpty(inputParams.StartUpConfig))
                throw new Exception(UpdaterName + " duration or inputParams  not set");

            GeneralLogs.WriteLogInDB("Integral StartUptare PriorityCode:" + inputParams.StartUpConfig + " StartIndex:" + inputParams.StartIndex);


            List<Feed> arr = Context.Feeds.Where<Feed>(x => x.UpdateDurationId.Value == duration.Id &&
                   (x.Site.IsBlog == inputParams.IsBlog) &&
                   (x.Deleted == 0 || (int)x.Deleted > 10)).OrderBy(x => x.Id).Skip(inputParams.StartIndex).Take<Feed>(inputParams.TopCount).ToList();


            if (arr.Count() == 0)
                return true;

            var feedContracts = arr.ConvertToFeedContract().ToList();
            for (int i = 0; i < feedContracts.Count; i++)
                feedContracts[i] = getNewItems(feedContracts[i]);
            UpdateItemFeeds(feedContracts);
            GeneralLogs.WriteLog(string.Format("Add {0} Feed to Buffer.", arr.Count()), TypeOfLog.Info);

            inputParams.StartIndex = inputParams.StartIndex + inputParams.TopCount;
            if (inputParams.StartIndex < duration.FeedsCount)
                StartUpdater(inputParams);
            return true;
        }

        #endregion

        #region PrivateMethod
        //one call is fuctionality for update duration that has many feeds            
        private Feed UpdatingFeed(long feedId)
        {
            int NumberOfNewItem = 0;
            var entiti = new TazehaContext();
            Feed dbfeed = entiti.Feeds.Single<Feed>(x => x.Id == feedId);
            dbfeed.UpdatingCount = dbfeed.UpdatingCount == null ? 1 : dbfeed.UpdatingCount + 1;
            dbfeed.LastUpdaterVisit = DateTime.Now;
            try
            {
                if (dbfeed.FeedType == 0 || !dbfeed.FeedType.HasValue)
                {
                    #region Feed
                    RssFeed feed = RssFeed.Read(dbfeed.Link);
                    //-----------shart check kardane inke feed aslan update shode dar site marja ya kheir------------
                    if (feed == null)
                    {
                        dbfeed.Deleted = Common.Share.DeleteStatus.NotWork;
                    }
                    else if (feed.Channels.Count > 0)
                    {
                        bool Exist = false;

                        if (Exist)
                        {
                            GeneralLogs.WriteLog("NoUpdated(last item exist) feed: " + feedId, TypeOfLog.Info);
                        }
                        else
                        {
                            //--------Feed has new items-----------
                            if (feed.Channels.Count > 0)
                            {
                                RssChannel channel = (RssChannel)feed.Channels[0];
                                List<FeedItemSP> listReturnBack;
                                if (channel.Items.LatestPubDate() != channel.Items[0].PubDate)
                                    listReturnBack = FeedItemsOperation.InsertFeedItems(channel.ItemsSorted, dbfeed);
                                else
                                    listReturnBack = FeedItemsOperation.InsertFeedItems(channel.Items, dbfeed);

                                GeneralLogs.WriteLog("Updating feed " + dbfeed.Id + " Num:" + listReturnBack.Count + " " + dbfeed.Link, TypeOfLog.OK);
                            }
                        }
                    }
                    #endregion
                }
                else if (dbfeed.FeedType.HasValue && dbfeed.FeedType.Value == Common.Share.FeedType.Atom)
                {
                    #region Atom
                    //-----------------atom feed--------------
                    XmlReader reader = XmlReader.Create(dbfeed.Link);
                    SyndicationFeed atomfeed = SyndicationFeed.Load(reader);
                    int i = 0;
                    if (atomfeed == null)
                    {
                        dbfeed.Deleted = Common.Share.DeleteStatus.NotWork;
                    }
                    else if (atomfeed.Items.Any())
                    {
                        foreach (SyndicationItem item in atomfeed.Items)
                        {
                            i++;
                            break;
                        }
                        if (i > 0)
                        {
                            List<FeedItem> listReturnBack = FeedItemsOperation.InsertFeedItems(atomfeed.Items, dbfeed);

                            GeneralLogs.WriteLog("OK updating atom " + dbfeed.Id + " Num:" + listReturnBack.Count + " " + dbfeed.Link);
                        }

                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                #region Exception
                if (ex.Message.IndexOf("404") > 0)
                {
                    dbfeed.Deleted = Common.Share.DeleteStatus.NotFound;
                }
                else if (ex.Message.IndexOf("403") > 0)
                {
                    dbfeed.Deleted = Common.Share.DeleteStatus.Forbidden;
                }
                else if (ex.Message.IndexOfX("timed out") > 0)
                {
                    //------request time out
                    dbfeed.Deleted = Common.Share.DeleteStatus.RequestTimeOut;
                }
                //-----------log error-----
                if (ex.Message.IndexOfX("Inner Exception") > 0)
                    CrawlerLog.FailLog(dbfeed, ex.InnerException.Message.SubstringX(0, 1020));
                else
                    GeneralLogs.WriteLog("Info " + ex.Message);
                #endregion
            }
            #region LASTFLOW
            try
            {
                CheckForChangeDuration(dbfeed, NumberOfNewItem > 0 ? true : false);
                CrawlerLog.SuccessLog(dbfeed, NumberOfNewItem);
                entiti.SaveChanges();
            }
            catch (Exception ex)
            {
                GeneralLogs.WriteLog(ex.Message);
            }
            #endregion

            return dbfeed;
        }
        private static void CheckForChangeDuration(Feed feed, bool hasNewFeedItem)
        {
            TazehaContext Context = new TazehaContext();
            if (!feed.LastUpdateDateTime.HasValue)
            {
                feed.LastUpdateDateTime = DateTime.Now;
                Context.SaveChanges();
                return;
            }
            TimeSpan lastupdatetime = DateTime.Now.Subtract(feed.LastUpdateDateTime.Value);
            TimeSpan delaytime = TimeSpan.Parse(feed.UpdateDuration.DelayTime);

            //---agar dar baze zamani kamtar az delay tarif shode feed update shode bood
            if (lastupdatetime < delaytime && hasNewFeedItem)
            {
                SpeedUP(feed);
            }
            else if (lastupdatetime > delaytime && !hasNewFeedItem)
            {
                SpeedDOWN(feed);
            }
            else
            {
                //---------dar baze zamani kamtar az delay update nashode bashe
                //------agar fasle akharin update kamtar az 2barabare delay bood bazham speed afzayesh yabad

                delaytime += delaytime;
                if (lastupdatetime < delaytime && feed.UpdateSpeed > 2)
                    SpeedUP(feed);
                // else
                //EventLog.GeneralLogs.WriteLog("Info ElseFeedUpdating..." + feed.Id + "|" + feed.Link);
            }


        }
        private static void SpeedDOWN(Feed feed)
        {
            TazehaContext Context = new TazehaContext();
            feed.LastUpdateDateTime = DateTime.Now;
            //-----feed hanooz update nashode ast------------
            if (feed.UpdateSpeed < -5)
            {
                UpdateDuration newduration = Context.UpdateDurations.FromHttpCache<UpdateDuration>().Where<UpdateDuration>(x => x.PriorityLevel > feed.UpdateDuration.PriorityLevel).OrderBy(x => x.PriorityLevel).First();
                feed.Id = newduration.Id;
                feed.UpdateSpeed = 4;
            }
            else
                feed.UpdateSpeed = feed.UpdateSpeed.HasValue ? feed.UpdateSpeed - 1 : -1;
        }
        private static void SpeedUP(Feed feed)
        {
            TazehaContext Context = new TazehaContext();
            feed.LastUpdateDateTime = DateTime.Now;
            if (feed.UpdateSpeed > 5)
            {
                //------feed ro saritar pooyesh kon(TAghire level)----
                var newdurations = Context.UpdateDurations.FromHttpCache<UpdateDuration>().Where<UpdateDuration>(x => x.PriorityLevel < feed.UpdateDuration.PriorityLevel);
                if (newdurations.Count() == 0)
                {
                    //-------agar be kamtarin baze updater resid---1Hour-----
                    feed.UpdateSpeed = 0;
                }
                else
                {
                    UpdateDuration newduration = newdurations.OrderByDescending(x => x.PriorityLevel).First();
                    feed.Id = newduration.Id;
                    feed.UpdateSpeed = 0;
                }
            }
            else
                feed.UpdateSpeed = feed.UpdateSpeed.HasValue ? feed.UpdateSpeed + 1 : 1;
        }

        private FeedContract getNewItems(FeedContract feedAsService)
        {
            var insertedItems = new List<FeedItemSP>();
            RssItemCollection RssItems = new RssItemCollection();
            if (!feedAsService.IsAtom)
            {
                RssFeed feed = RssFeed.Read(feedAsService.Link);
                if (feed == null)
                    feedAsService.IsNull = true;


                if (feed.Channels[0].Items.LatestPubDate() != feed.Channels[0].Items[0].PubDate)
                    RssItems = feed.Channels[0].ItemsSorted;
                else
                    RssItems = feed.Channels[0].Items;
            }
            else
            {
                XmlReader reader = XmlReader.Create(feedAsService.Link);
                SyndicationFeed atom = SyndicationFeed.Load(reader);
                if (atom == null)
                    feedAsService.IsNull = true;
                RssItems = atom.GetRssItemCollection();
            }

            //--------Feed has new items-----------            
            if (RssItems.Count > 0)
            {
                insertedItems = FeedItemsOperation.RssItemCollectionToFeedItemsContract(RssItems, feedAsService);
                if (insertedItems.Count() > 0)
                    feedAsService.LastFeedItemUrl = insertedItems[0].Link.SubstringX(0, 399);// RssItems[0].Link.ToString();

                GeneralLogs.WriteLog("OK updating feed " + feedAsService.Id + " Num:" + RssItems.Count + " " + feedAsService.Link);
            }

            //CrawlerLog.SuccessLog(feedAsService, insertedItems.Count);
            feedAsService.FeedItems = insertedItems;
            return feedAsService;
        }

        private bool UpdateItemFeeds(IEnumerable<FeedContract> feeds)
        {
            #region send items
            List<FeedItemSP> items = feeds.SelectMany(feed => feed.FeedItems.Select(item => new FeedItemSP
            {
                Cats = !string.IsNullOrEmpty(feed.Cats) ? feed.Cats.Split(' ').Select(x => int.Parse(x)) : null,
                CreateDate = DateTime.Now,
                Description = item.Description,
                Link = item.Link,
                PubDate = item.PubDate,
                FeedId = feed.Id,
                SiteId = feed.SiteId,
                SiteTitle = feed.SiteTitle,
                SiteUrl = feed.SiteUrl,
                Title = item.Title
            })).ToList();

            IRepositorySaver saver = new LuceneSaverRepository();
            saver.AddItems(items);
            NumberOfNewItemsToday += items.Count;
            if (NumberOfNewItemsToday > 100)
                Optimize();
            #endregion

            #region UpdateFeed
            var ids = feeds.Select(x => x.Id).ToList();
            var dbFeeds = Context.Feeds.Where(x => ids.Any(f => f == x.Id));
            foreach (var dbfeed in dbFeeds)
            {
                dbfeed.LastUpdatedItemUrl = feeds.FirstOrDefault(x => x.Id == dbfeed.Id).LastFeedItemUrl;
                if (!string.IsNullOrEmpty(dbfeed.LastUpdatedItemUrl))
                {
                    dbfeed.UpdatingCount = dbfeed.UpdatingCount == null ? 1 : dbfeed.UpdatingCount + 1;
                    dbfeed.LastUpdaterVisit = DateTime.Now;
                    CheckForChangeDuration(dbfeed, true);
                }
                else
                    CheckForChangeDuration(dbfeed, false);

                dbfeed.LastUpdaterVisit = DateTime.Now;
            }
            Context.SaveChanges();
            GeneralLogs.WriteLog("UpdateFeeds[" + UpdaterName + "] : " + string.Join("[br /]", dbFeeds.Select(x => x.Link)), TypeOfLog.OK);
            #endregion

            return true;
        }
        private void Optimize()
        {
            LuceneBase _LuceneRepository = new LuceneSaverRepository();
            _LuceneRepository.Optimize();
            GeneralLogs.WriteLog("Optimize[AsService] ", TypeOfLog.Info);
        }
        #endregion
    }
}
