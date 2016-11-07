using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using Rss;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Updater;
using System.Net;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Common.Config;

namespace Mn.NewsCms.Robot.Updater
{
    public class ClientUpdater : BaseUpdaterClient
    {
        const int RequestTimeOut = 5000;
        IBaseServer server;
        IFeedBusiness feedBiz;
        private readonly IAppConfigBiz _appConfigBiz;
        private readonly IUnitOfWork _unitOfWork;

        #region Constructor
        public ClientUpdater(IBaseServer baseservice, IFeedBusiness feedBusiness, IAppConfigBiz appConfigBiz, IUnitOfWork unitOfWork, bool? IsLocaly)
            : base(baseservice, IsLocaly)
        {
            server = baseservice;
            feedBiz = feedBusiness;
            _appConfigBiz = appConfigBiz;
            _unitOfWork = unitOfWork;
        }
        #endregion

        public void FeedsUpdat(List<FeedContract> feeds)
        {
            int itemcount = 0;
            List<FeedContract> listRes = new List<FeedContract>();
            string[] OldList = null;
            foreach (var feed in feeds)
            {
                try
                {
                    var temp = FeedUpdateAsService(feed, OldList != null ? OldList.ToList() : null);
                    if (temp != null)
                        listRes.Add(temp);
                    itemcount = listRes.Sum(x => x.FeedItems.Count);
                    if (itemcount > 100)
                    {
                        GeneralLogs.WriteLogInDB("SendFeeds...", TypeOfLog.Start);
                        server.SendFeeds(listRes);
                        OldList = listRes.SelectMany(x => x.FeedItems.Select(c => c.Link)).ToArray();
                        listRes.Clear();
                        itemcount = 0;
                    }
                }
                catch (Exception ex)
                {
                    feedBiz.DisableTemporary(feed.Id);
                    GeneralLogs.WriteLogInDB("FeedsUpdat problem " + ex.Message, TypeOfLog.Error);
                }
            }
            if (itemcount > 0)
                server.SendFeeds(listRes);

            ///--optimize dar in bakhsh va besoorate koli baede hame feed ha anjam mishavad ke feshar kamtari be server vared shavad
            ///harchand ke momkene baese feed haye TEKRARI beshe
            server.Optimize();
            //Indexer.Indexer.TagsTableSaveChanges();
        }
        public FeedContract FeedUpdateAsService(FeedContract feedAsService, List<string> listRes)
        {
            var insertedItems = new List<FeedItem>();
            RssItemCollection RssItems = new RssItemCollection();
            if (!feedAsService.IsAtom)
            {
                HttpWebRequest request = HttpWebRequest.CreateHttp(feedAsService.Link);
                request.Timeout = RequestTimeOut;
                var feed = RssFeed.Read(request);
                if (feed == null)
                    feedAsService.IsNull = true;

                if (!feed.Channels[0].Items.LatestPubDate().Equals(feed.Channels[0].Items[0].PubDate))
                {
                    if (feedAsService.Cats.Contains("27"))
                    {
                        var RssItemsT = feed.Channels[0].Items;
                        int count = 0;
                        foreach (RssItem ritem in RssItemsT)
                        {
                            if (count >= 10)
                                break;
                            RssItems.Insert(count++, ritem);
                        }

                    }
                    else
                        RssItems = feed.Channels[0].ItemsSorted;
                }
                else
                    RssItems = feed.Channels[0].Items;
            }
            else
            {
                //XmlReader reader = XmlReader.Create(feedAsService.Link);
                SyndicationFeed atom;
                WebRequest request = WebRequest.Create(feedAsService.Link);
                request.Timeout = RequestTimeOut;
                using (WebResponse response = request.GetResponse())
                using (XmlReader reader = XmlReader.Create(response.GetResponseStream()))
                {
                    atom = SyndicationFeed.Load(reader);
                    if (atom == null)
                        feedAsService.IsNull = true;
                }
                RssItems = atom.GetRssItemCollection();
            }

            //--------Feed has new items-----------            
            if (RssItems.Count > 0)
            {
                insertedItems = new FeedItemsOperation(_appConfigBiz, _unitOfWork).RssItemCollectionToFeedItemsContract(RssItems, feedAsService);
                if (insertedItems.Count() > 0)
                    feedAsService.LastFeedItemUrl = insertedItems[0].Link.SubstringX(0, 399);// RssItems[0].Link.ToString();

                GeneralLogs.WriteLog("OK updating feed " + feedAsService.Id + " Num:" + RssItems.Count + " " + feedAsService.Link);
            }

            //CrawlerLog.SuccessLog(feedAsService, insertedItems.Count);
            feedAsService.FeedItems = listRes != null ? insertedItems.Where(x => !listRes.Any(l => l == x.Link)).ToList() : insertedItems;
            return feedAsService;
        }

        public void AutoUpdateFromServer()
        {
            StopUpdater = false;
            var MaxFeedCountAsService = Config.GetConfig<int>("MaxFeedCountAsService");
            //GeneralLogs.WriteLog("Start AutoUpdate as service", TypeOfLog.Start);
            while (!StopUpdater)
            {
                try
                {
                    var feeds = server.getLatestFeeds(MaxFeedCountAsService, IsLocaly);
                    if (feeds.Count > 0)
                    {
                        GeneralLogs.WriteLog(string.Format("feeds.Count {0} > 0 So start FeedsUpdat", feeds.Count), TypeOfLog.Start);
                        FeedsUpdat(feeds);
                    }
                    else
                    {
                        StopUpdater = true;
                        GeneralLogs.WriteLog("End AutoUpdate as service count of feeds ", TypeOfLog.End);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    GeneralLogs.WriteLog("exption getLatestFeeds service: " + ex.Message, TypeOfLog.Error);
                    StopUpdater = true;
                    break;
                }
            }
        }
        public void NoneStopUpdateFromServer(string DurationCode)
        {

            var MaxFeedCountAsService = Config.GetConfig<int>("MaxFeedCountAsService");
            //GeneralLogs.WriteLog("Start AutoUpdate as service", TypeOfLog.Start);
            while (true)
            {
                if (DateTime.UtcNow.AddHours(Config.GetConfig<double>("UTCDelay")).Hour > Config.GetConfig<int>("StartNightly") &&
                DateTime.UtcNow.AddHours(Config.GetConfig<double>("UTCDelay")).Hour < Config.GetConfig<int>("EndNightly"))
                    return;
                try
                {
                    var feeds = server.getLatestFeedsByDuration(DurationCode, MaxFeedCountAsService, false);
                    if (feeds.Count > 0)
                    {
                        GeneralLogs.WriteLog(string.Format("feeds.Count {0} > 0 So start FeedsUpdat", feeds.Count), TypeOfLog.Start);
                        FeedsUpdat(feeds);
                        System.Threading.Thread.Sleep(1000 * 2);
                    }
                    else
                    {
                        GeneralLogs.WriteLog("End AutoUpdate as service count of feeds ", TypeOfLog.End);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    GeneralLogs.WriteLog("exption getLatestFeeds service: " + ex.Message, TypeOfLog.Error);
                }
            }
        }
        public void UpdateFromServerByTimer(string DurationCode)
        {

            var MaxFeedCountAsService = Config.GetConfig<int>("MaxFeedCountAsService");

            try
            {
                var feeds = server.getLatestFeedsByDuration(DurationCode, MaxFeedCountAsService, false);
                if (feeds.Count > 0)
                {
                    GeneralLogs.WriteLog(string.Format("feeds.Count {0} > 0 So start FeedsUpdat", feeds.Count), TypeOfLog.Start);
                    FeedsUpdat(feeds);
                }
                else
                {
                    GeneralLogs.WriteLog("End AutoUpdate as service count of feeds ", TypeOfLog.End);
                }
            }
            catch (Exception ex)
            {
                GeneralLogs.WriteLog("exption getLatestFeeds service: " + ex.Message, TypeOfLog.Error);
            }

        }
        public override void UpdateIsParting()
        {
            throw new NotImplementedException();
        }

        public override void Poke()
        {
            if (StopUpdater)
                AutoUpdateFromServer();
        }
    }
}
