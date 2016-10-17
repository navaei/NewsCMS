using System;
using System.Collections.Generic;
using ACorns.WCF.DynamicClientProxy;
using Tazeyab.Common.Models;
using System.ServiceModel;
using Tazeyab.Common;
using System.Linq;
using Tazeyab.Common.EventsLog;
using System.IO;


namespace Tazeyab.CrawlerEngine.Indexer
{
    public class LuceneIndexer : Tazeyab.Common.LuceneBase
    {
        string _lucenedir;
        public static int CallOptimize = 0;
        public static List<FeedItemSP> listofItems = new List<FeedItemSP>();
        //public static void GenerateData()
        //{

        //    TazehaContext context = new TazehaContext();
        //    try
        //    {
        //        for (int i = 1; i < 200; i++)
        //        {
        //            var query = context.FeedItems.Select(x => new FeedItemSP { FeedItemId = x.FeedItemId, Title = x.Title, Description = x.Description, PubDate = x.PubDate, SiteTitle = x.Feed.Site.SiteTitle, SiteUrl = x.Feed.Site.SiteUrl, Cats = x.Feed.CatsFeeds.Select(c => c.CatdId) }).OrderBy(x => x.FeedItemId).Skip(i * 3000).Take(3000);
        //            foreach (var item in query)
        //            {
        //                indexWriter.AddDocument(FeedItemToDocument(item));
        //            }
        //            indexWriter.Optimize();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //oops
        //        //  Log.Error(this, e.Message);
        //    }
        //    finally
        //    {
        //        //we need to make sure that we close this!
        //        if (indexWriter != null)
        //        {
        //            //close the index
        //            indexWriter.Close();
        //        }
        //    }

        //}

        public void AddFeedItem(List<FeedItemSP> items)
        {
            //ServiceFactory<IBaseService>.Create().SendFeedItems(items);
            base.AddFeedItems(items);
            if (++CallOptimize > 5)
            {
                //ServiceFactory<IBaseService>.Create().Optimize();
                base.Optimize();
                CallOptimize = 0;
                GeneralLogs.WriteLog("Optimize data" + DateTime.Now,  TypeOfLog.OK);
                //-----------Save tags changes---------
                // Indexer.Indexer.TagsTableSaveChanges();
            }

        }

        public void AddFeedItemToLucene(FeedItemSP item)
        {
            lock (listofItems)
                listofItems.Add(item);
            if (listofItems.Count > 50)
            {
                FeedItemSP[] listtemp = null;
                lock (listofItems)
                {
                    listtemp = listofItems.ToArray();
                    listofItems.Clear();
                    //Indexer.LuceneIndexer lucene = new global::Tazeyab.CrawlerEngine.Indexer.LuceneIndexer();
                    AddFeedItem(listtemp.ToList());
                }
            }
        }
        
    }
}
