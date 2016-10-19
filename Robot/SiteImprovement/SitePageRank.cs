using System;
using System.Linq;
using System.Data.Linq;
using System.Data.Entity;
using CrawlerEngine;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common;

namespace Mn.NewsCms.Robot.SiteImprovement
{

    public class SitePageRank : IStarter<StartUp>
    {
        #region SitePageRank
        public static void SetSitePageRank(int StartIndex = 0)
        {
            var context = new TazehaContext();
            int TopCount = 100;
            var sites = context.Sites.Where(x => !x.PageRank.HasValue && !x.IsBlog).OrderBy(x => x.Id).Skip(StartIndex).Take(TopCount).Select(x => new { x.Id, x.SiteUrl }).ToDictionary(x => x.Id, x => x.SiteUrl);
            foreach (var site in sites)
            {
                try
                {
                    string siteUrl = site.Value;
                    siteUrl = siteUrl.IndexOfX("www.") > -1 || siteUrl.IndexOfX("http://") > -1 ? siteUrl : "www." + siteUrl;
                    siteUrl = siteUrl.IndexOfX("http://") > -1 ? siteUrl : "http://" + siteUrl;
                    byte PageRank = GooglePageRank.GetPageRank(siteUrl);
                    setPageRank(site.Key, PageRank);
                    GeneralLogs.WriteLog("OK @SetSitePageRank " + siteUrl + " " + PageRank);
                }
                catch (Exception ex)
                {
                    setPageRank(site.Key, 0);
                    GeneralLogs.WriteLog("Error @setPageRank SiteId:" + site.Key + " " + ex.Message);
                }
            }
            if (context.Sites.Where(x => x.PageRank == null && !x.IsBlog).OrderBy(x => x.Id).Skip(StartIndex).Count() > 0)
            {
                SetSitePageRank(StartIndex + TopCount);
            }
        }
        static void setPageRank(decimal SiteID, byte PageRank)
        {
            var context = new TazehaContext();
            var site = context.Sites.SingleOrDefault(x => x.Id == SiteID);
            site.PageRank = PageRank;
            context.SaveChanges();
        }
        #endregion

        public void Start(StartUp inputParams)
        {
            SetSitePageRank();
        }
    }

}
