using System;
using System.Collections.Generic;
using CrawlerEngine;
using Rss;
using System.IO;
using System.Net;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Linq;
using HtmlAgilityPack;
using System.Text;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Config;
using Mn.Framework.Common;
using static System.String;

namespace Mn.NewsCms.Robot.Parser
{
    public class LinkParser
    {
        public class ParseLinksStruct
        {
            public List<string> RssLinks = new List<string>();
            public List<string> ExternalLinks = new List<string>();
            public int SpamCount;
        }
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LinkParser() { }

        #endregion
        #region Constants

        private const string _LINK_REGEX = "href=\"[a-zA-Z./:&\\d_-]+\"";

        #endregion
        #region Private Instance Fields

        private List<string> _goodUrls = new List<string>();
        private List<string> _rssurls = new List<string>();
        private List<string> _badUrls = new List<string>();
        private List<string> _otherUrls = new List<string>();
        private List<string> _externalUrls = new List<string>();
        private List<string> _exceptions = new List<string>();

        #endregion
        #region Public Properties

        public List<string> GoodUrls
        {
            get { return _goodUrls; }
            set
            {
                _goodUrls = value;
            }
        }
        public List<string> RssUrls
        {
            get { return _rssurls; }
            set { _rssurls = value; }
        }
        public List<string> BadUrls
        {
            get { return _badUrls; }
            set { _badUrls = value; }
        }

        public List<string> OtherUrls
        {
            get { return _otherUrls; }
            set { _otherUrls = value; }
        }

        public List<string> ExternalUrls
        {
            get { return _externalUrls; }
            set { _externalUrls = value; }
        }

        public List<string> Exceptions
        {
            get { return _exceptions; }
            set { _exceptions = value; }
        }

        #endregion

        public static List<string> getAllLinks(string Content)
        {
            List<string> listLinks = new List<string>();
            HtmlDocument htmlDoc = new HtmlDocument()
            {
                OptionCheckSyntax = true,
                OptionFixNestedTags = true,
                OptionAutoCloseOnEnd = true,
                OptionDefaultStreamEncoding = Encoding.UTF8
            };
            htmlDoc.LoadHtml(Content);
            HtmlNodeCollection nods = htmlDoc.DocumentNode.SelectNodes(@"//a");
            // var nnodes = nods.Where(x => !string.IsNullOrEmpty(x.Attributes["href"].Value)).Select(x => x.Attributes["href"].Value);
            // var nnodes2 = nods.Select(x => x.Attributes["href"].Value);
            if (nods != null)
                foreach (var item in nods)
                {
                    try
                    {
                        if (!IsNullOrEmpty(item.Attributes["href"].Value))
                            listLinks.Add(item.Attributes["href"].Value);
                    }
                    catch { }
                }
            //var matches = Regex.Matches(Content, @"href=""(.*?)\""", RegexOptions.Singleline).OfType<Match>().Select(m => m.Groups[0].Value).Distinct();
            //List<string> listLinks2 = matches.ToList<string>();
            return listLinks;
        }

        public static ParseLinksStruct ParseLinks(List<string> listLinks, string sourceUrl)
        {
            var _LinkParser = new ParseLinksStruct();
            foreach (var item in listLinks.ToList())
            {
                try
                {
                    string anchorMatch = item;
                    if (anchorMatch == Empty)
                    {
                        continue;
                    }

                    string foundHref = null;
                    try
                    {
                        foundHref = anchorMatch.Replace("href=\"", "");
                    }
                    catch (Exception exc)
                    {
                        continue;
                    }
                    if (IsFeedCandidateUrl(foundHref) && IsAWebPage(foundHref))
                    {
                        if (foundHref.IndexOfX("http://") > -1)
                        {
                            if (foundHref.IndexOfX("feedburner.com") > 0)
                            {
                                _LinkParser.RssLinks.Add(foundHref);
                            }
                        }
                        else
                            foundHref = FixPath(sourceUrl, foundHref, sourceUrl);
                        _LinkParser.RssLinks.Add(foundHref);
                    }
                    else if (IsExternalUrl(sourceUrl, foundHref))
                    {
                        if (!foundHref.Contains(ExtractDomainNameFromURL(sourceUrl)))
                        {
                            _LinkParser.ExternalLinks.Add(ExtractDomainNameFromURL(foundHref));
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return _LinkParser;

        }

        private static bool IsExternalUrl(string BaseUrl, string url)
        {

            try
            {
                if (url.IndexOfX(BaseUrl.ReplaceX("www.", "").ReplaceX("http://", "")) > -1)
                {
                    return false;
                }
                else if ((url.Substring(0, 7) == "http://" || url.Substring(0, 3) == "www" || url.Substring(0, 7) == "https://") &&
                    !url.Contains(BaseUrl))
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        private static bool IsFeedCandidateUrl(string url)
        {
            try
            {
                if (url.IndexOfX("rss") > -1)
                {
                    if (url.CountStringOccurrences("rss") < 3)
                    {
                        return true;
                    }
                }
                if (url.IndexOfX("feed") > -1)
                {
                    if (url.CountStringOccurrences("feed") < 3)
                    {
                        return true;
                    }

                }
                if (url.IndexOfX("atom") > -1)
                {
                    if (url.CountStringOccurrences("atom") < 3)
                    {
                        return true;
                    }

                }
                return false;

            }
            catch
            {
                return false;
            }
            return false;
        }
        public static HasFeed ParseRssLink(List<string> RssLinks, Site Site)
        {
            int numberOfRss = 0;
            int SpamCount = 0;
            HasFeed res = HasFeed.No;
            RssLinks = RssLinks.Distinct().ToList();
            foreach (var foundHref in RssLinks)
            {
                res = IsFeedUrl(foundHref, Site);
                if (res == HasFeed.Rss)
                    numberOfRss++;
                else if (res == 0)
                    numberOfRss++;
                else if (res == HasFeed.No)
                {
                    SpamCount++;
                    if (SpamCount > 10)
                    {
                        GeneralLogs.WriteLog("Errror $$$$$$$$$$$$$$$$$$ Reject For repeat error....$$$$$$$$$$$$$");
                        break;
                    }
                }
            }
            return res;
        }
        static HasFeed IsFeedUrl(string url, Site Site)
        {

            try
            {
                RssFeed feed;
                WebClient wc = new WebClient();
                if (url.IndexOfX("http://") == -1)
                    url = "http://" + url;
                url = url[url.Length - 1] == '/' ? url.Remove(url.Length - 1) : url;

                MemoryStream ms = new MemoryStream(wc.DownloadData(url));
                var sr = new StreamReader(ms);
                var myStr = sr.ReadToEnd();
                if (IsNullOrEmpty(myStr))
                    return HasFeed.No;
                HtmlDocument htmlDoc = new HtmlDocument()
                {
                    OptionCheckSyntax = true,
                    OptionFixNestedTags = true,
                    OptionAutoCloseOnEnd = true,
                    OptionDefaultStreamEncoding = Encoding.UTF8
                };
                htmlDoc.LoadHtml(myStr);

                //HtmlNodeCollection nods = htmlDoc.DocumentNode.SelectNodes(@"//a");

                if (htmlDoc.DocumentNode.SelectNodes(@"//rss") != null ||
                    htmlDoc.DocumentNode.SelectNodes(@"//Rss") != null ||
                    htmlDoc.DocumentNode.SelectNodes(@"//RSS") != null)
                {
                    feed = RssFeed.Read(url);
                    if (feed.Channels.Count > 0)
                    {
                        //global::namespace Mn.NewsCms.Robot.FeedOperation.InsertRSSFeed(feed, Site);
                        Site.HasFeed = HasFeed.Rss;
                        return HasFeed.Rss;
                    }
                    else
                        return HasFeed.No;
                }
                else if (htmlDoc.DocumentNode.SelectNodes(@"//feed") != null ||
                    htmlDoc.DocumentNode.SelectNodes(@"//Feed") != null ||
                    htmlDoc.DocumentNode.SelectNodes(@"//FEED") != null)
                {
                    XmlReader reader = XmlReader.Create(url);
                    SyndicationFeed atomfeed = SyndicationFeed.Load(reader);
                    if (atomfeed.Items != null && atomfeed.Items.Any())
                    {
                        global::Mn.NewsCms.Robot.FeedOperation.InsertAtomFeed(atomfeed, Site);
                        Site.HasFeed = HasFeed.Rss;
                        return HasFeed.Rss;
                    }
                    else
                        return HasFeed.No;
                }
                else
                {
                    //-------------in shart baraye ineke crawker tooye ye site gir nayofte--------
                    if (Site.IndexPageText == myStr || myStr.IndexOf("<html") == -1)
                        return HasFeed.No;
                    else
                    {
                        return HasFeed.No;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOfX("Inner Exception") > 0)
                    GeneralLogs.WriteLog(Format(">Errror @IsFeedUrl {0} {1}", url, ex.InnerException));
                else
                    GeneralLogs.WriteLog(Format(">Errror @IsFeedUrl {0} {1}", url, ex.Message));

                return HasFeed.No;
            }
            return HasFeed.No;
        }
        private static bool IsAWebPage(string foundHref)
        {
            if (foundHref.IndexOf("javascript:") == 0)
                return false;

            string extension = foundHref.Substring(foundHref.LastIndexOf(".") + 1, foundHref.Length - foundHref.LastIndexOf(".") - 1);
            switch (extension)
            {
                case "jpg":
                case "css":
                case "ico":
                case "png":
                case "gif":
                case "swf":
                    return false;
                default:
                    return true;
            }

        }

        public static string ExtractDomainNameFromURL(string Url)
        {
            try
            {
                if (!Url.Contains("://"))
                    Url = "http://" + Url;

                return new Uri(Url).Host;
            }
            catch
            {
                try
                {
                    Url = Url.Replace("/", "").ReplaceX("http:", "HTTP://");
                    return new Uri(Url).Host;
                }
                catch
                {
                    return Empty;
                }
            }
        }
        public static bool IsRestrictSite(string url)
        {
            //url = url.ReplaceX("www.", "").ReplaceX("http://", "");
            string[] arr = ServiceFactory.Get<IAppConfigBiz>().RestrictSites();
            foreach (var item in arr)
            {
                if (url.IndexOfX(item) > -1)
                    return true;
            }
            return false;
        }
        internal static string GetSubDomain(string url)
        {
            url = url.IndexOfX("http://") < 0 ? "http://" + url : url;
            Uri uri = new Uri(url);
            return GetSubDomain(uri);
        }
        internal static string GetSubDomain(Uri url)
        {

            if (url.HostNameType == UriHostNameType.Dns)
            {
                string host = url.Host;
                if (host.Split('.').Length > 2)
                {

                    int lastIndex = host.LastIndexOf(".");

                    int index = host.LastIndexOf(".", lastIndex - 1);

                    string ret = host.Substring(0, index);
                    if (ret.EqualsX("www") || ret.EqualsX(".www"))
                        return null;
                    return ret;
                }
            }
            return null;

        }
        public static string HasSocialTags(string PageLink)
        {
            WebClient wc = new WebClient();
            MemoryStream ms = new MemoryStream(wc.DownloadData(PageLink));
            var htmlContent = new StreamReader(ms);
            string content = htmlContent.ReadToEnd();
            string imageURL = global::CrawlerEngine.Helper.HtmlMetaParser.getOGImageMetaContent(content);
            if (imageURL.ContainsX(".jpg") || imageURL.ContainsX(".gif") || imageURL.ContainsX(".png"))
            {
                return imageURL;
            }
            return null;
        }
        public static string FixPath(string originatingUrl, string link, string BaseUrl)
        {
            string formattedLink = Empty;
            if (link.IndexOf("../") > -1)
            {
                formattedLink = ResolveRelativePaths(link, originatingUrl);
            }
            else if (originatingUrl.IndexOfX(BaseUrl) > -1
                && link.IndexOfX(BaseUrl) == -1)
            {
                if (!IsNullOrEmpty(link))
                    if (link[0] == '/')
                        link = link.Remove(0, 1);
                if (link.Equals(originatingUrl, StringComparison.CurrentCultureIgnoreCase))
                    return originatingUrl;
                if (originatingUrl.LastIndexOf("/") == originatingUrl.Length - 1)
                    formattedLink = originatingUrl.Substring(0, originatingUrl.LastIndexOf("/") + 1) + link;
                else
                    formattedLink = originatingUrl.Substring(0, originatingUrl.Length) + "/" + link;
            }
            else if (link.IndexOfX(BaseUrl) == -1)
            {
                formattedLink = BaseUrl + link;
            }
            else if (link.IndexOfX(LinkParser.ExtractDomainNameFromURL(originatingUrl)) > -1)
                return link;
            return formattedLink;
        }

        private static string ResolveRelativePaths(string relativeUrl, string originatingUrl)
        {
            string resolvedUrl = Empty;

            string[] relativeUrlArray = relativeUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string[] originatingUrlElements = originatingUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            int indexOfFirstNonRelativePathElement = 0;
            for (int i = 0; i <= relativeUrlArray.Length - 1; i++)
            {
                if (relativeUrlArray[i] != "..")
                {
                    indexOfFirstNonRelativePathElement = i;
                    break;
                }
            }

            int countOfOriginatingUrlElementsToUse = originatingUrlElements.Length - indexOfFirstNonRelativePathElement - 1;
            for (int i = 0; i <= countOfOriginatingUrlElementsToUse - 1; i++)
            {
                if (originatingUrlElements[i] == "http:" || originatingUrlElements[i] == "https:")
                    resolvedUrl += originatingUrlElements[i] + "//";
                else
                    resolvedUrl += originatingUrlElements[i] + "/";
            }

            for (int i = 0; i <= relativeUrlArray.Length - 1; i++)
            {
                if (i >= indexOfFirstNonRelativePathElement)
                {
                    resolvedUrl += relativeUrlArray[i];

                    if (i < relativeUrlArray.Length - 1)
                        resolvedUrl += "/";
                }
            }

            return resolvedUrl;
        }
    }
}
