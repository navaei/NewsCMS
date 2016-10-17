using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tazeyab.Common.Models;
using Tazeyab.Common.EventsLog;
using Tazeyab.Common;

namespace Tazeyab.CrawlerEngine
{
    class CrawlerLog
    {
        public static void FailLog(Feed feed, string ErrorMessage, int type = 2)
        {
            WriteLog(feed, type, ErrorMessage);
        }
        public static void SuccessLog(Feed feed, int NumberOfNewItem, int type = 1)
        {
            WriteLog(feed, type, string.Empty, NumberOfNewItem);
        }
        public static void WriteLog(Feed feed, int type)
        {
            WriteLog(feed, type, string.Empty);
        }
        static void WriteLog(Feed feed, int type, string Message, int NumberOfNewItem = 0)
        {
            string propertyname = "FeedId:" + feed.Id.ToString().Length + "Link:" + feed.Link.Length + "  Date:" + DateTime.Now.ToShortDateString().Length + "Time:" + DateTime.Now.ToShortTimeString().Length;
            string propertyvalue = "FeedId:" + feed.Id + "Link:" + feed.Link + "  Date:" + DateTime.Now.ToShortDateString() + "Time:" + DateTime.Now.ToShortTimeString();
            var entiti = new TazehaContext();
            Crawler_Log log = new Crawler_Log();
            log.PropertyNames = propertyname;
            log.PropertyValues = propertyvalue;
            log.Type = type;
            log.FeedId = feed.Id;
            log.Message = Message;
            log.CreationDateTime = DateTime.Now;
            log.NumberOfNewItem = NumberOfNewItem;
            entiti.Crawler_Log.Add(log);
            entiti.SaveChanges();
            GeneralLogs.WriteLog("OK DurationId:" + feed.Id + " FeedId:" + feed.Id + "Link:" + feed.Link + "\n" + "NumberOfnewItems " + NumberOfNewItem);
        }
    }
}
