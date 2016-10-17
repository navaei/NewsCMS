using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Tazeyab.Common;
//using Divan;
using Tazeyab.Common.Models;

namespace Tazeyab.CrawlerEngine.Repository
{
    public class CloudantRepository : IRepositorySaver
    {
        public bool AddItem(FeedItem item)
        {
            //CouchServer server = new CouchServer("xamfia.cloudant.com", 5984, "xamfia", "123400");
            //ICouchDatabase database = server.GetDatabase("docs");          
            //database.WriteDocument(ItemToJson(item), item.FeedItemId.ToString());
            return true;
        }


        public void AddItems(List<FeedItem> items)
        {
            foreach (var item in items)
                AddItem(item);
        }

        private string ItemToJson(FeedItem item)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string JsonData = ser.Serialize(new { item.Id, item.Title, item.Description, item.SiteTitle, item.SiteId, item.Link, item.PubDate, string.Empty });
            return JsonData;

            //cats = string.IsNullOrEmpty(cats) ? "00" : cats;
            string json = string.Format(@"{ 'FeedItemId':'{0}','Title' : '{1}' ,'Description':'{2}','SiteTitle' : '{3}' ",
                item.Id, item.Title, item.Description, item.SiteTitle);
            json += string.Format(" 'SiteId':'{0}' , 'Link':'{1}' ,'PubDate' : '{2}' , 'Cats':'{3}'  }", item.SiteId, item.Link, item.PubDate.Value.ToString("yyyyMMddHH"), string.Empty);
            return json;
        }
    }
}
