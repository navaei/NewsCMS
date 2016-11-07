using System;
using System.Collections.Generic;
using System.Linq;
using CrawlerEngine;
using Rss;
using System.ServiceModel.Syndication;
using System.Threading;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Robot.Helper;
using Mn.NewsCms.Robot.Indexer;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Robot.Repository;
using Mn.NewsCms.Common.Share;
using Mn.NewsCms.Common.Config;


namespace Mn.NewsCms.Robot
{

    public class FeedOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppConfigBiz _appConfigBiz;

        public FeedOperation(IUnitOfWork unitOfWork, IAppConfigBiz appConfigBiz)
        {
            _unitOfWork = unitOfWork;
            _appConfigBiz = appConfigBiz;
        }

        //public static void InsertRSSFeed(RssFeed feed, Site Site)
        //{
        //    RssChannel channel = (RssChannel)feed.Channels[0];
        //    var entiti = new TazehaContext();
        //    Feed dbfeed = new Feed();
        //    dbfeed.CopyRight = feed.Channels[0].Copyright;
        //    dbfeed.Description = feed.Channels[0].Description.SubstringX(0, 100);
        //    dbfeed.Link = feed.Url.SubstringX(0, 400);// feed.Channels[0].Link.AbsolutePath;
        //    dbfeed.Title = feed.Channels[0].Title.SubstringX(0, 148);
        //    dbfeed.Id = ServiceFactory.Get<IAppConfigBiz>().DefaultSearchDuration();
        //    dbfeed.Id = Site.Id;
        //    dbfeed.FeedType = 0;
        //    dbfeed.CreationDate = DateTime.Now;
        //    entiti.Feeds.Add(dbfeed);
        //    entiti.SaveChanges();
        //    GeneralLogs.WriteLog("OK  HasRSS " + Site.SiteUrl);
        //    try
        //    {
        //        ThreadPool.QueueUserWorkItem(FeedItemsOperation.InsertFeedItemsRss, new object[] { channel.Items, dbfeed.Id });
        //    }
        //    catch
        //    { }
        //}
        public void InsertAtomFeed(SyndicationFeed atomfeed, Site Site)
        {
            var entiti = new TazehaContext(ServiceFactory.Get<IAppConfigBiz>().ConnectionString());
            var dbfeed = new Feed();
            if (atomfeed.Copyright != null)
                dbfeed.CopyRight = atomfeed.Copyright.Text;
            if (atomfeed.Description != null)
                dbfeed.Description = atomfeed.Description.Text.SubstringX(0, 999);
            dbfeed.Link = atomfeed.BaseUri.ToString();// feed.Channels[0].Link.AbsolutePath;
            dbfeed.Title = atomfeed.Title.Text.SubstringX(0, 148);
            dbfeed.UpdateDurationId = _appConfigBiz.DefaultSearchDuration();
            dbfeed.SiteId = Site.Id;
            dbfeed.FeedType = FeedType.Atom;//-----for Atom----------
            dbfeed.CreationDate = DateTime.Now;
            entiti.Feeds.Add(dbfeed);
            entiti.SaveChanges();
            GeneralLogs.WriteLog("OK HasAtom " + Site.SiteUrl);
            try
            {
                new FeedItemsOperation(_appConfigBiz, _unitOfWork).InsertFeedItemsAtom(new object[] { atomfeed.Items, dbfeed.Id });
            }
            catch { }
        }
    }
    public class FeedItemsOperation
    {
        private readonly IAppConfigBiz _appConfigBiz;
        private readonly IUnitOfWork _unitOfWork;

        public FeedItemsOperation(IAppConfigBiz appConfigBiz, IUnitOfWork unitOfWork)
        {
            _appConfigBiz = appConfigBiz;
            _unitOfWork = unitOfWork;
        }

        static int _maxLength = 0;
        public int MaxDescLength
        {
            get
            {
                if (_maxLength == 0)
                    _maxLength = _appConfigBiz.MaxDescriptionLength();
                return _maxLength;
            }
        }
        public List<FeedItem> RssItemCollectionToFeedItemsContract(RssItemCollection items, FeedContract feed)
        {
            var listReturnBack = new List<FeedItem>();
            foreach (RssItem item in items)
            {

                if (!string.IsNullOrEmpty(feed.LastFeedItemUrl) && feed.LastFeedItemUrl.SubstringX(0, 399).Equals(item.Link.ToString().SubstringX(0, 399)))
                    return listReturnBack;
                if (listReturnBack.Any(x => x.Link == item.Link.ToString()))
                    continue;
                if (!Utility.HasFaWord(item.Title))
                    continue;

                var itemcontract = new FeedItem();
                itemcontract.Title = Helper.HtmlRemoval.StripTagsRegex(item.Title).Replace("\t", "").Replace("\n", "").Replace("\r", "");
                itemcontract.Link = item.Link.ToString();
                itemcontract.Description = HtmlRemoval.StripTagsRegex(item.Description).Replace("\t", "").Replace("\n", "").Replace("\r", "");
                //-------------------------Baray DB koochiK!!-----------------
                itemcontract.Description = itemcontract.Description.SubstringX(0, _appConfigBiz.MaxDescriptionLength());
                if (item.PubDate.Year > 1350 && item.PubDate < DateTime.Now.AddDays(2))
                    itemcontract.PubDate = item.PubDate;
                else
                    break;

                itemcontract.FeedId = feed.Id;
                itemcontract.SiteId = feed.SiteId;
                itemcontract.SiteUrl = feed.SiteUrl;
                itemcontract.SiteTitle = feed.SiteTitle;
                listReturnBack.Add(itemcontract);
            }
            return listReturnBack;
        }

        //public static void InsertFeedItemsRss(object param)
        //{
        //    object[] array = param as object[];
        //    RssItemCollection items = (RssItemCollection)array[0];
        //    decimal feedId = (decimal)array[1];
        //    InsertFeedItems(items, feedId);
        //}
        public List<FeedItem> RssItemsToFeedItems(RssItemCollection items, Feed feed)
        {
            var listReturnBack = new List<FeedItem>();
            foreach (RssItem item in items)
            {

                if (!string.IsNullOrEmpty(feed.LastUpdatedItemUrl) && feed.LastUpdatedItemUrl.SubstringX(0, 399).Equals(item.Link.ToString().SubstringX(0, 399)))
                    return listReturnBack;
                if (listReturnBack.Any(x => x.Link == item.Link.ToString()))
                    continue;
                if (!Utility.HasFaWord(item.Title))
                    continue;

                var dbitem = new FeedItem();
                dbitem.Id = Guid.NewGuid();
                dbitem.Title = HtmlRemoval.StripTagsRegex(item.Title).Replace("\t", "").Replace("\n", "").Replace("\r", "");
                dbitem.Link = item.Link.ToString();
                dbitem.Description = HtmlRemoval.StripTagsRegex(item.Description).Replace("\t", "").Replace("\n", "").Replace("\r", "");
                //-------------------------Baray DB koochiK!!-----------------
                dbitem.Description = dbitem.Description.SubstringX(0, MaxDescLength);
                dbitem.SiteId = feed.SiteId;
                dbitem.FeedId = feed.Id;
                if (item.PubDate.Year > 1350 && item.PubDate < DateTime.Now.AddHours(1))
                    dbitem.PubDate = item.PubDate.AddMinutes(feed.Site.TimeDifference);
                else
                    break;
                dbitem.CreateDate = DateTime.Now;
                //dbitem.Cats = feed.Categories.Select(x => x.Id).ToList();
                dbitem.SiteTitle = feed.Site.SiteTitle;
                dbitem.SiteUrl = feed.Site.SiteUrl;
                dbitem.SiteId = feed.SiteId;
                dbitem.ShowContentType = feed.Site.ShowContentType;
                dbitem.IndexedType = dbitem.IndexedType.HasValue ? dbitem.IndexedType + 1 : 1;
                var feedItem = new FeedItem { Link = dbitem.Link, Title = dbitem.Title, PubDate = dbitem.PubDate, CreateDate = DateTime.Now, FeedId = feed.Id };
                listReturnBack.Add(dbitem);
            }
            return listReturnBack;
        }
        public List<FeedItem> AtomItemsToFeedItems(IEnumerable<SyndicationItem> Items, Feed feed)
        {
            var listReturnBack = new List<FeedItem>();
            foreach (SyndicationItem item in Items)
            {

                if (!string.IsNullOrEmpty(feed.LastUpdatedItemUrl) && feed.LastUpdatedItemUrl.SubstringX(0, 399).Equals(item.Links[0].Uri.AbsoluteUri.SubstringX(0, 399)))
                    return listReturnBack;
                if (listReturnBack.Any(x => x.Link == item.Links[0].Uri.AbsoluteUri))
                    continue;
                if (!Utility.HasFaWord(item.Title.Text))
                    continue;

                var dbitem = new FeedItem();
                dbitem.Id = Guid.NewGuid();
                dbitem.Title = HtmlRemoval.StripTagsRegex(item.Title.Text).Replace("\t", "").Replace("\n", "").Replace("\r", "");
                dbitem.Link = item.Links[0].Uri.AbsoluteUri;
                dbitem.Description = HtmlRemoval.StripTagsRegex(item.Summary.Text).Replace("\t", "").Replace("\n", "").Replace("\r", "");
                //-------------------------Baray DB koochiK!!-----------------
                dbitem.Description = dbitem.Description.SubstringX(0, MaxDescLength);
                dbitem.SiteId = feed.SiteId;
                dbitem.FeedId = feed.Id;
                if (item.PublishDate.Year > 1350 && item.PublishDate < DateTime.Now.AddDays(2))
                    dbitem.PubDate = item.PublishDate.DateTime.AddMinutes(feed.Site.TimeDifference);
                else
                    break;
                dbitem.CreateDate = DateTime.Now;
                //dbitem.Cats = feed.Categories.Select(x => x.Id).ToList();
                dbitem.SiteTitle = feed.Site.SiteTitle;
                dbitem.SiteUrl = feed.Site.SiteUrl;
                dbitem.SiteId = feed.SiteId;
                dbitem.ShowContentType = feed.Site.ShowContentType;
                dbitem.IndexedType = dbitem.IndexedType.HasValue ? dbitem.IndexedType + 1 : 1;
                var feedItem = new FeedItem { Link = dbitem.Link, Title = dbitem.Title, PubDate = dbitem.PubDate, CreateDate = DateTime.Now, FeedId = feed.Id };
                listReturnBack.Add(dbitem);
            }
            return listReturnBack;
        }
        public void InsertFeedItemsAtom(object param)
        {
            object[] array = param as object[];
            IEnumerable<SyndicationItem> items = (IEnumerable<SyndicationItem>)array[0];
            decimal feedId = (decimal)array[1];
            InsertFeedItems(items, feedId);
        }
        //public static List<FeedItem> InsertFeedItems(RssItemCollection items, decimal feedId)
        //{
        //    var entiti = new TazehaContext();
        //    var feed = entiti.Feeds.SingleOrDefault(x => x.Id == feedId);
        //    return InsertFeedItems(items, feed);
        //}
        public List<FeedItem> InsertFeedItems(IEnumerable<SyndicationItem> items, decimal feedId)
        {
            var feed = _unitOfWork.Set<Feed>().SingleOrDefault(x => x.Id == feedId);
            return InsertFeedItems(items, feed);
        }
        public List<FeedItem> InsertFeedItems(IEnumerable<SyndicationItem> Items, Feed feed)
        {
            var listReturnBack = new List<FeedItem>();
            var erroroccur = 0;
            foreach (SyndicationItem item in Items)
            {
                if (erroroccur > 2)
                    return listReturnBack;
                try
                {
                    if (!Utility.HasFaWord(item.Title.Text))
                        continue;

                    FeedItem dbitem = new FeedItem();
                    dbitem.Title = HtmlRemoval.StripTagsRegex(item.Title.Text).Replace("\t", "").Replace("\n", "").Replace("\r", "");
                    dbitem.Link = item.Links[0].Uri.AbsoluteUri;
                    dbitem.Description = item.Summary == null ? string.Empty : HtmlRemoval.StripTagsRegex(item.Summary.Text).Replace("\t", "").Replace("\n", "").Replace("\r", "");
                    //-------------------------Baray DB koochi!!-----------------
                    dbitem.Description = dbitem.Description.SubstringX(0, 1000);
                    dbitem.CreateDate = DateTime.Now;
                    dbitem.SiteId = feed.SiteId;
                    dbitem.FeedId = feed.Id;
                    if (item.PublishDate != null)
                        dbitem.PubDate = item.PublishDate.DateTime;
                    dbitem.IndexedType = dbitem.IndexedType.HasValue ? dbitem.IndexedType + 1 : 1;
                    //dbitem.Cats = feed.Categories.Select(x => x.Id);
                    dbitem.SiteTitle = feed.Site.SiteTitle;
                    dbitem.SiteUrl = feed.Site.SiteUrl;
                    dbitem.SiteId = feed.SiteId;

                    var feedItem = new FeedItem { Link = dbitem.Link, Title = dbitem.Title, PubDate = dbitem.PubDate, CreateDate = DateTime.Now, FeedId = feed.Id };
                    _unitOfWork.Set<FeedItem>().Add(feedItem);
                    _unitOfWork.SaveAllChanges();
                    dbitem.Id = Guid.Parse(feedItem.Id.ToString());
                    LuceneRepositoryAsService lucene = new LuceneRepositoryAsService();
                    lucene.AddItem(dbitem);
                    //Helper.Utility.InsertItemToCloudant(dbitem);

                    listReturnBack.Add(dbitem);
                    Indexer.Indexer.FirstIndexing(dbitem);
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOfX("Inner Exception") > 0)
                        if (ex.InnerException.Message.IndexOfX("Cannot insert duplicate key") > -1)
                        {
                            //if (item.Links[0].Uri.AbsoluteUri.ContainsX(Crawler.BaseSite.SiteUrl))
                            break;
                        }
                        else
                            GeneralLogs.WriteLog("Errror @InsertFeedItems" + ex.InnerException.Message);
                    else
                        GeneralLogs.WriteLog("Errror @InsertFeedItems" + ex.Message);
                    erroroccur++;
                }
            }

            //baraye in biroon forech gharar dadam ke har seri baraye set shodane IndexType save anjam nashe
            //try
            //{
            //    entiti.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //    if (ex.Message.IndexOfX("Inner Exception") > 0)
            //        EventLog.LogsBuffer.WriteLog(">Errror @InsertFeedItems2" + ex.InnerException.Message);
            //    else
            //        EventLog.LogsBuffer.WriteLog(">Errror @InsertFeedItems2" + ex.Message);
            //}
            return listReturnBack;
        }

        public List<FeedItem> InsertItemsSqlLucene(RssItemCollection items, Feed feed)
        {
            List<FeedItem> listReturnBack = new List<FeedItem>();
            IRepositorySaver RepositorySql = new SqlRepository();
            IRepositorySaver RepositoryLucene = new LuceneRepositoryAsService();
            int erroroccur = 0;
            foreach (RssItem item in items)
            {
                if (erroroccur > 2)
                    return listReturnBack;
                if (!Utility.HasFaWord(item.Title))
                    continue;

                FeedItem dbitem = new FeedItem();
                dbitem.Title = HtmlRemoval.StripTagsRegex(item.Title).Replace("\t", "").Replace("\n", "").Replace("\r", "");
                dbitem.Link = item.Link.ToString();
                dbitem.Description = HtmlRemoval.StripTagsRegex(item.Description).Replace("\t", "").Replace("\n", "").Replace("\r", "");
                //-------------------------Baray DB koochiK!!-----------------
                dbitem.Description = dbitem.Description.SubstringX(0, _appConfigBiz.MaxDescriptionLength());
                dbitem.SiteId = feed.SiteId;
                dbitem.FeedId = feed.Id;
                if (item.PubDate.Year > 1350 && item.PubDate < DateTime.Now.AddDays(2))
                    dbitem.PubDate = item.PubDate;
                else
                    break;
                dbitem.CreateDate = DateTime.Now;
                //dbitem.Cats = feed.Categories.Select(x => x.Id).ToList();
                dbitem.SiteTitle = feed.Site.SiteTitle;
                dbitem.SiteUrl = feed.Site.SiteUrl;
                dbitem.SiteId = feed.SiteId;
                dbitem.IndexedType = dbitem.IndexedType.HasValue ? dbitem.IndexedType + 1 : 1;
                var feedItem = new FeedItem { Link = dbitem.Link, Title = dbitem.Title, PubDate = dbitem.PubDate, CreateDate = DateTime.Now, FeedId = feed.Id };

                if (RepositorySql.AddItem(feedItem))
                {
                    RepositoryLucene.AddItem(dbitem);
                    listReturnBack.Add(dbitem);
                    Indexer.Indexer.FirstIndexing(dbitem);
                }

            }
            return listReturnBack;
        }

    }
}
