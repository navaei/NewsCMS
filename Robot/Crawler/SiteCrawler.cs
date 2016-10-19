using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Robot.Helper;
using Mn.NewsCms.Robot.Parser;
using Mn.NewsCms.Common.Share;
using Mn.NewsCms.Common.Config;
using Mn.Framework.Common;

namespace Mn.NewsCms.Robot.Crawler
{

    public static class SiteCrawler
    {

        private static bool StopCrawling = false;
        public static void Execute(StartUp InputParams)
        {
            TazehaContext entiti = new TazehaContext();
            TazehaContext entiti2 = new TazehaContext();
            GeneralLogs.WriteLog("Start Remove WWW...");
            var sites = entiti.Sites.Where(x => x.SiteUrl.StartsWith("www.")).OrderBy(x => x.Id).Skip(InputParams.StartIndex).Take(InputParams.TopCount).ToList();
            if (sites.Count == 0)
                return;
            foreach (var site in sites)
            {
                string link = site.SiteUrl.ReplaceX("www.", "");
                var sites2 = entiti.Sites.Where(x => x.SiteUrl.Contains(link) && x.Id != site.Id);
                if (sites2.Any())
                {
                    var site2 = sites2.First();
                    var feeds = entiti.Feeds.Where(x => x.Id == site.Id);
                    if (feeds.Any())
                        site2.HasFeed = HasFeed.Rss;
                    foreach (var feed in feeds)
                    {
                        feed.SiteId = site2.Id;
                    }
                    var deletedsite = entiti.Sites.SingleOrDefault(x => x.Id == site.Id);
                    entiti.Sites.Remove(deletedsite);
                    GeneralLogs.WriteLog("OK DeleteSite " + site.Id + " " + site.SiteUrl);
                }
                else
                {
                    site.SiteUrl = site.SiteUrl.ReplaceX("www.", "");
                    GeneralLogs.WriteLog("OK ChangeSiteURL " + site.Id + " " + site.SiteUrl);
                    //entiti2.SaveChanges();
                }
                entiti.SaveChanges();
            }
            Execute(InputParams);
        }
        public static void Start(StartUp inputParams)
        {
            TazehaContext entiti = new TazehaContext();
            GeneralLogs.WriteLog("Beginning crawl.");
            IEnumerable<long> sites;
            if (inputParams.StartUpConfig != null && inputParams.StartUpConfig.IndexOf("OrderByDescending") > -1)
            {
                sites = entiti.Sites.Where<Site>(x => (x.CrawledCount == 0 || !x.CrawledCount.HasValue) && (x.IsBlog == inputParams.IsBlog)).OrderByDescending(x => x.Id).Take(inputParams.TopCount).Select(x => x.Id).ToList();
            }
            else
                sites = entiti.Sites.Where<Site>(x => (x.CrawledCount == 0 || !x.CrawledCount.HasValue) && (x.IsBlog == inputParams.IsBlog)).OrderBy(x => x.Id).Take(inputParams.TopCount).Select(x => x.Id).ToList();
            if (sites.Count() == 0)
            {
                GeneralLogs.WriteLogInDB("OK...EndingSites " + DateTime.Now.ToString());
                Console.ReadKey();
                return;
            }
            foreach (var site in sites)
            {

                GeneralLogs.WriteLog("STARTING..." + site);
                if (StopCrawling)
                    break;
                try
                {
                    List<string> ExternalLinks = CrawleSite(site);
                    if (ExternalLinks.Count > 1)
                        CrawlerExternalSite(new object[] { ExternalLinks[0], ExternalLinks });
                }
                catch (Exception ex)
                {
                    GeneralLogs.WriteLog("Error Ln94: " + ex.Message.SubstringX(0, 50) + ex.InnerException);
                }
            }
            //try
            //{
            //    //----SAVE ALL CHANGES----------
            //    entiti.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //    EventLog.LogsBuffer.WriteLog(">Error SiteSaveExeption:" + " " + ex.InnerException.Message);
            //}
            //--------------CHECK KARDAN MOJOOD BOODANE SITE PEYMAYESH NASHODE-----------
            int NoCrawled = 0;
            if (inputParams.StartUpConfig != null && inputParams.StartUpConfig.IndexOf("OrderByDescending") > -1)
            {
                NoCrawled = entiti.Sites.Where<Site>(x => x.CrawledCount == 0 || !x.CrawledCount.HasValue).OrderByDescending(x => x.Id).Take(inputParams.TopCount).Count();
            }
            else
                NoCrawled = entiti.Sites.Where<Site>(x => x.CrawledCount == 0 || !x.CrawledCount.HasValue).OrderBy(x => x.Id).Take(inputParams.TopCount).Count();
            if (NoCrawled > (inputParams.TopCount / 2))
            {
                inputParams.StartIndex += inputParams.StartIndex + inputParams.TopCount;
                Start(inputParams);
            }
            GeneralLogs.WriteLog("@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@/ KOLAN VARES /@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");

        }
        public static void LinksCrawler(StartUp InputParams)
        {
            var IndexPageText = Requester.GetWebText(InputParams.StartUpConfig);
            List<string> Alllinks = LinkParser.getAllLinks(IndexPageText);
            var res = LinkParser.ParseLinks(Alllinks, InputParams.StartUpConfig);
            var Externallinks = res.ExternalLinks;

            CrawlerExternalSite(new object[] { Externallinks[0], Externallinks });

        }

        public static Page GetPage(string url)
        {
            var html = Requester.GetWebText(url);
            return new Page(html);
        }


        #region Private
        private static List<string> CrawleSite(decimal siteId)
        {
            TazehaContext entiti = new TazehaContext();
            List<string> Externallinks = new List<string>();
            var site = entiti.Sites.SingleOrDefault(x => x.Id == siteId);
            if (LinkParser.IsRestrictSite(site.SiteUrl))
            {
                GeneralLogs.WriteLog("Reject " + site.SiteUrl);
                site.CrawledCount = site.CrawledCount + 1;
                entiti.SaveChanges();
                return Externallinks;
            }
            if (string.IsNullOrEmpty(site.IndexPageText))
            {
                string link = "HTTP://" + site.SiteUrl.ReplaceX("http://", "");
                site.IndexPageText = Requester.GetWebText(link);
            }
            global::Mn.NewsCms.Robot.Parser.HtmlParser html = new global::Mn.NewsCms.Robot.Parser.HtmlParser();
            if (site.HasFeed != HasFeed.Rss)
            {
                #region SiteNotHasFeed
                if (site.IsBlog && site.SiteUrl.IndexOfX("BLOGFA.COM") > 3)
                {
                    Feed feed = new Feed();
                    feed.Link = "HTTP://" + site.SiteUrl.Replace("/", "") + "/RSS";
                    feed.Title = site.SiteTitle;
                    feed.SiteId = site.Id;
                    feed.UpdateDurationId = ServiceFactory.Get<IAppConfigBiz>().DefaultSearchDuration();
                    feed.CreationDate = DateTime.Now;
                    feed.FeedType = 0;
                    entiti.Feeds.Add(feed);
                    site.HasFeed = HasFeed.Rss;

                }
                else if (site.IsBlog && site.SiteUrl.IndexOfX("MIHANBLOG.COM") > 3)
                {
                    Feed feed = new Feed();
                    feed.Link = "HTTP://" + site.SiteUrl.Replace("/", "") + "/POST/ATOM";
                    feed.Title = site.SiteTitle;
                    feed.SiteId = site.Id;
                    feed.UpdateDurationId = ServiceFactory.Get<IAppConfigBiz>().DefaultSearchDuration();
                    feed.CreationDate = DateTime.Now;
                    feed.FeedType = FeedType.Atom;//SHAYAD 1 BASHE HAAAAA!
                    entiti.Feeds.Add(feed);
                    site.HasFeed = HasFeed.Atom;

                }
                else
                {
                    List<string> Alllinks = LinkParser.getAllLinks(site.IndexPageText);
                    if (Alllinks.Count > 0)
                    {
                        var res = LinkParser.ParseLinks(Alllinks, site.SiteUrl);
                        site.HasFeed = LinkParser.ParseRssLink(res.RssLinks, site);
                        Externallinks = res.ExternalLinks;
                    }
                    // site.SiteLogo = global::namespace Mn.NewsCms.Robot.SiteImprovement.SiteIcon.GetSiteIcon(site.SiteUrl);
                }
                #endregion
            }
            else
            {
                #region SiteHasFeed
                List<string> Alllinks = LinkParser.getAllLinks(site.IndexPageText);
                if (Alllinks.Count > 0)
                {
                    var res = LinkParser.ParseLinks(Alllinks, site.SiteUrl);
                    Externallinks = res.ExternalLinks;
                }
                #endregion
            }
            Externallinks.Insert(0, site.SiteUrl);
            site.CrawledCount = 1;
            entiti.SaveChanges();
            return Externallinks;
        }
        private static void CrawlerExternalSite(object param)
        {

            TazehaContext entiti = new TazehaContext();
            object[] array = param as object[];
            string ParentSite = array[0].ToString();
            List<string> PexternalUrls = ((List<string>)array[1]);
            PexternalUrls = PexternalUrls.Distinct().ToList();
            for (int i = 0; i < PexternalUrls.Count; i++)
            {
                if (LinkParser.IsRestrictSite(PexternalUrls[i]))
                    PexternalUrls.Remove(PexternalUrls[i]);
                else
                    PexternalUrls[i] = PexternalUrls[i][PexternalUrls[i].Length - 1] == '/' ? PexternalUrls[i].Remove(PexternalUrls[i].Length - 1) : PexternalUrls[i];
                PexternalUrls[i] = PexternalUrls[i].ReplaceX("www.", "");
                PexternalUrls[i] = PexternalUrls[i].ReplaceX("http://", "");
            }
            string Temstr = string.Join(",", new List<string>(PexternalUrls).ToArray());
            //PexternalUrls = entiti.Sites_CheckNotIn(Temstr, ",").ToList();
            throw new Exception("plz implament Sites_CheckNotIn");
            if (PexternalUrls.Count == 0)
                return;
            foreach (string str in PexternalUrls.ToList())
            {
                try
                {
                    if (str.IndexOf(".") == -1)
                        continue;
                    if (StopCrawling)
                    {
                        GeneralLogs.WriteLog("STOP CRAWLING");
                        Thread.CurrentThread.Abort();
                        return;
                    }
                    string htmlText = string.Empty;
                    Site externalsite = new Site();
                    externalsite.SiteUrl = LinkParser.ExtractDomainNameFromURL(str.ToUpper()).Replace("http://", "").Replace("/", "");
                    externalsite.SiteUrl = externalsite.SiteUrl[externalsite.SiteUrl.Length - 1] == '/' ? externalsite.SiteUrl.Remove(externalsite.SiteUrl.Length - 1) : externalsite.SiteUrl;
                    //------age urr moshkel dasht az list kharej mishavad----------
                    if (string.IsNullOrEmpty(externalsite.SiteUrl.Trim()))
                        continue;
                    htmlText = Requester.GetWebText("HTTP://" + externalsite.SiteUrl.ToUpper().ReplaceX("http://", ""));
                    Page page = new Page(htmlText);
                    page.Url = str;
                    if (string.IsNullOrEmpty(page.Content))
                    {
                        GeneralLogs.WriteLog("Reject " + str);
                        continue;
                    }
                    if (!Utility.HasFaWord(page.FullTitle))
                        if (!Utility.HasFaWord(page.Description))
                            if (!Utility.HasFaWord(page.Content))
                                if (str.IndexOfX(".IR") < 4)
                                {
                                    if (PexternalUrls.Where(x => x.ContainsX(str)).Count() > 1)
                                    {
                                        PexternalUrls.RemoveAll(x => x.ContainsX(str));
                                    }
                                    GeneralLogs.WriteLog("Reject " + str);
                                    continue;
                                }
                                else
                                    page.Title = page.Title.SubstringX(0, 150);

                    externalsite.SiteTitle = string.IsNullOrEmpty(page.Title) ? page.Url.ReplaceX("http://", "").ReplaceX("www.", "") : page.Title.SubstringX(0, 150);
                    //externalsite.SiteTags = string.IsNullOrEmpty(page.KeyWord) ? "" : page.KeyWord.SubstringX(0, 300);
                    externalsite.SiteDesc = string.IsNullOrEmpty(page.Description) ? "" : page.Description.SubstringX(0, 200);
                    //externalsite.IndexPageText = page.Content.SubstringX(0, 4000);
                    externalsite.LinkedCount = externalsite.LinkedCount.HasValue ? externalsite.LinkedCount + 1 : 1;
                    string blog = LinkParser.GetSubDomain(externalsite.SiteUrl);
                    externalsite.IsBlog = string.IsNullOrEmpty(blog) ? false : true;
                    if (externalsite.IsBlog && !bool.Parse(ServiceFactory.Get<IAppConfigBiz>().GetConfig("IsBlogInserting")))
                        continue;
                    entiti.Sites.Add(externalsite);
                }
                catch (Exception ex)
                {
                    GeneralLogs.WriteLog("SiteCrawler.cs ln265 " + ex.Message);
                }

            }
            try
            {
                entiti.SaveChanges();
            }
            catch
            {
                entiti.Dispose();
            }
        }
        #endregion
    }
}
