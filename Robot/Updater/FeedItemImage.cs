using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Robot.Parser;
using System.Drawing;
using Mn.NewsCms.Common;
using Mn.NewsCms.Robot.Helper;
using CrawlerEngine.Helper;
using Mn.NewsCms.Common.Config;

namespace Mn.NewsCms.Robot.Updater
{
    public class FeedItemImage
    {
        private readonly IAppConfigBiz _appConfigBiz;

        public FeedItemImage(IAppConfigBiz appConfigBiz)
        {
            _appConfigBiz = appConfigBiz;
        }

        static string _itemsPhotoPath;
        public string VisualItemsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_itemsPhotoPath))
                    _itemsPhotoPath = _appConfigBiz.VisualItemsPath();
                return _itemsPhotoPath;
            }
        }
        public void SetItemsImage(List<FeedItem> items, Feed feed)
        {
            var error = 0;
            var success = 0;
            if (feed.Site.HasImage != HasImage.NotSupport)
                foreach (var item in items)
                {
                    if (feed.Site.HasImage == HasImage.OpenGraph)
                    {
                        break;
                    }
                    else if (feed.Site.HasImage == HasImage.HtmlPattern)
                    {
                        try
                        {
                            var image = FetchImage(item.Link, feed.Site.ImagePattern, feed.Site.SiteUrl);
                            if (!string.IsNullOrEmpty(image))
                            {
                                item.HasPhoto = Imager.SaveImage(image, Path.Combine(VisualItemsPath, item.Id.ToString()));
                                if (success++ == 6)
                                    return;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (error++ == 3)
                                return;

                            GeneralLogs.WriteLog(ex, typeof(Imager));

                        }
                    }
                }
        }
        public static List<string> FetchImages(string link, string pattern)
        {
            return FetchImages(link, pattern, string.Empty);
        }
        public static List<string> FetchImages(string link, string pattern, string siteUrl)
        {
            var list = new List<string>();

            if (string.IsNullOrEmpty(link))
                return list;
            var content = Requester.GetWebText(link);
            if (string.IsNullOrEmpty(content))
                return list;
            var tags = HtmlParser.GetTags(content, pattern);
            foreach (var tag in tags)
            {
                var src = tag.GetAttributeValue("src", "");
                if (!string.IsNullOrEmpty(src))
                {
                    if (src.ToLower().StartsWith("http://") || src.ToLower().StartsWith("https://"))
                        list.Add(src);
                    else
                        list.Add("http://" + siteUrl + "/" + src);
                }
            }
            return list;
        }
        public static string FetchImage(string link, string pattern, string siteUrl)
        {

            if (string.IsNullOrEmpty(link))
                return string.Empty;
            var content = Requester.GetWebText(link);
            if (string.IsNullOrEmpty(content))
                return string.Empty;
            var tags = HtmlParser.GetTags(content, pattern);
            if (tags == null)
                return string.Empty;
            foreach (var tag in tags)
            {
                var src = tag.GetAttributeValue("src", "");
                if (!string.IsNullOrEmpty(src))
                {
                    if (src.ToLower().StartsWith("http://") || src.ToLower().StartsWith("https://"))
                        return src;
                    else
                        return "http://" + siteUrl + "/" + src;
                }
            }


            return string.Empty;
        }

    }
}
