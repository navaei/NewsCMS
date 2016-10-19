using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Lucene.Net.Store;
using Lucene.Net.Index;
using Mn.NewsCms.Common.Models;
using Lucene.Net.Documents;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Mn.NewsCms.Common;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Mn.NewsCms.DomainClasses.UpdaterBusiness
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
