using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Tazeyab.Common.Models;
using Lucene.Net.Documents;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Tazeyab.Common;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Tazeyab.DomainClasses.UpdaterBusiness
{   
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
