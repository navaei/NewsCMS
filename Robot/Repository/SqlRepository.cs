using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tazeyab.Common;
using Tazeyab.Common.Models;

namespace Tazeyab.CrawlerEngine.Repository
{
    public class SqlRepository : IRepositorySaver
    {
        
        public bool AddItem(FeedItem item)
        {

            var context = new TazehaContext();
            var itemdb = context.FeedItems.FirstOrDefault(x => x.Link == item.Link);
            if (itemdb == null)
            {
                context.FeedItems.Add(item);
                context.SaveChanges();
            }
            else if (itemdb.FeedId != item.FeedId)
            {
                itemdb.AlternativeFeedId = item.FeedId;
                context.SaveChanges();
            }
            else
                return false;
            return true;
        }      

        public void AddItems(List<FeedItem> items)
        {
            throw new NotImplementedException();
        }                
    }
}
