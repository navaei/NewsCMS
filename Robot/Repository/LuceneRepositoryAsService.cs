using System;
using System.Collections.Generic;
//using ACorns.WCF.DynamicClientProxy;
using Mn.NewsCms.Common.Models;
using System.ServiceModel;
using Mn.NewsCms.Common;
using System.Linq;
using Mn.NewsCms.Common.EventsLog;
using System.IO;


namespace Mn.NewsCms.Robot.Repository
{
    public class LuceneRepositoryAsService : Mn.NewsCms.Common.LuceneBase, IRepositorySaver
    {
        string _lucenedir;
        public static int CallOptimize = 0;
        public static List<FeedItem> listofItems = new List<FeedItem>();        

        public void AddItems(List<FeedItem> items)
        {
            //ServiceFactory<IBaseService>.Create().SendFeedItems(items);
            base.AddFeedItems(items);
            if (++CallOptimize > 5)
            {
                //ServiceFactory<IBaseService>.Create().Optimize();
                base.Optimize();
                CallOptimize = 0;
                GeneralLogs.WriteLog("Optimize data" + DateTime.Now, TypeOfLog.OK);
                //-----------Save tags changes---------
                // Indexer.Indexer.TagsTableSaveChanges();
            }

        }

        public bool AddItem(FeedItem item)
        {
            lock (listofItems)
                listofItems.Add(item);
            if (listofItems.Count > 50)
            {
                FeedItem[] listtemp = null;
                lock (listofItems)
                {
                    listtemp = listofItems.ToArray();
                    listofItems.Clear();
                    //Indexer.LuceneIndexer lucene = new global::namespace Mn.NewsCms.Robot.Indexer.LuceneIndexer();
                    AddItems(listtemp.ToList());
                }
            }
            return true;
        }                   
    }

    public class LuceneSaverRepository : LuceneBase, IRepositorySaver
    {


        public void AddItems(List<FeedItem> items)
        {
            base.AddFeedItems(items);
        }       

        public bool AddItem(FeedItem item)
        {
            throw new NotImplementedException();
        }
    }
}
