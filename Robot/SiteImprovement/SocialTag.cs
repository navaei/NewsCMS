using System.Collections.Generic;
using System.Linq;
using CrawlerEngine;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Robot.Parser;
using Mn.NewsCms.Common;

namespace Mn.NewsCms.Robot.SiteImprovement
{

    //public class SocialTag : IStarter<StartUp>
    //{
    //    #region SetHasSocialTag
    //    public static void SetHasSocialTag(StartUp InputParams)
    //    {
    //        int StartIndex = InputParams.StartIndex;
    //        var context = new TazehaContext();
    //        Dictionary<long, long> feeds = context.Feeds.Where(x => x.Site.HasSocialTag == null && !x.Site.IsBlog)
    //            .OrderBy(x => x.Id).Skip(StartIndex).Take(1000)
    //            .Select(x => new { x.Id, x.SiteId }).ToDictionary(x => x.Id, x => x.SiteId);
    //        for (int i = 0; i < feeds.Count; i++)
    //        {
    //            try
    //            {
    //                decimal Key = feeds.ElementAt(i).Key, Value = feeds.ElementAt(i).Value;
    //                var itemlink = context.FeedItems.Where(x => x.FeedId == Key).OrderByDescending(x => x.Id).First().Link;
    //                var site = context.Sites.SingleOrDefault(x => x.Id == Value);
    //                if (!string.IsNullOrEmpty(LinkParser.HasSocialTags(itemlink)))
    //                    site.HasSocialTag = true;
    //                else
    //                    site.HasSocialTag = false;
    //                context.SaveChanges();
    //                if (feeds.Count(x => x.Value == Value) > 1)
    //                    feeds.RemoveAll(x => x.Value == Value && x.Key != Key);
    //                GeneralLogs.WriteLog("OK @SetHasSocialTag siteID:" + Value + " HasSocialTags:" + site.HasSocialTag);
    //            }
    //            catch { }
    //        }
    //        if (context.Feeds.Where(x => x.Site.HasSocialTag == null && (!x.Site.IsBlog)).OrderBy(x => x.Id).Skip(StartIndex).Count() > 100)
    //        {
    //            InputParams.StartIndex += 1000;
    //            SetHasSocialTag(InputParams);
    //        }
    //    }
    //    #endregion

    //    public void Start(StartUp inputParams)
    //    {
    //        SetHasSocialTag(inputParams);
    //    }
    //}

}
