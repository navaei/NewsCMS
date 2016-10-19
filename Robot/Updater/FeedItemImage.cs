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
using Mn.Framework.Common;
using Mn.NewsCms.Common.Config;

namespace Mn.NewsCms.Robot.Updater
{
    public class FeedItemImage
    {
        static string _itemsPhotoPath;
        public static string VisualItemsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_itemsPhotoPath))
                    _itemsPhotoPath = ServiceFactory.Get<IAppConfigBiz>().VisualItemsPath();
                return _itemsPhotoPath;
            }
        }
        public static void setFeedItemsImage(Feed dbfeed, List<FeedItem> items, int MaxNumberOfItems)
        {
            //var context = new TazehaContext();
            //foreach (var item in items.Take(MaxNumberOfItems))
            //{
            //    string imageURL = LinkParser.HasSocialTags(item.Link);
            //    if (!string.IsNullOrEmpty(imageURL))
            //    {
            //        // var Item_Index2 = context.FeedItems.Where(x => x.FeedItemId == item.FeedItemId).ToList().Select(x => new FeedItems_Index { Description = x.Description, FeedItemId = x.FeedItemId, Link = x.Link, PubDate = x.PubDate, SiteTitle = x.Feeds.Sites.SiteTitle, SiteURL = x.Feeds.Sites.SiteUrl, Title = x.Title, VisitsCount = x.VisitsCount });
            //        //var Item_Index2 = item.Select(x => new FeedItems_Index { Description = x.Description, FeedItemId = x.FeedItemId, Link = x.Link, PubDate = x.PubDate, SiteTitle = x.Feeds.Sites.SiteTitle, SiteURL = x.Feeds.Sites.SiteUrl, Title = x.Title, VisitsCount = x.VisitsCount });
            //        if (item.Title.Length < 10 || item.Description.Length < 150 || !item.PubDate.HasValue || item.PubDate.Value.AddDays(2) < DateTime.Now)
            //            continue;
            //        FeedItems_Index Item_Index = new FeedItems_Index();
            //        //if (item.ItemId != 0)
            //        //    Item_Index.ItemId = item.ItemId;
            //        Item_Index.Title = item.Title;
            //        Item_Index.Link = item.Link;
            //        Item_Index.PubDate = item.PubDate;
            //        Item_Index.Description = item.Description;
            //        Item_Index.SiteTitle = dbfeed.Site.SiteUrl;
            //        Item_Index.SiteURL = dbfeed.Site.SiteUrl;
            //        Item_Index.CatIdDefault = dbfeed.CatIdDefault;

            //        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(imageURL);
            //        request.UserAgent = "PersianFeedCrawler";
            //        try
            //        {
            //            WebResponse response = request.GetResponse();
            //            Stream stream = response.GetResponseStream();
            //            var memoryStream = new MemoryStream();
            //            stream.CopyTo(memoryStream);
            //            byte[] arr = memoryStream.ToArray();
            //            Image img = arr.ToImage().GetThumbnailImage(250, 250, null, IntPtr.Zero);
            //            var ImageUrl = @"~\Images\LogicalImage\Index\" + Item_Index.FeedItemId;
            //            img.Save(ImageUrl);
            //            Item_Index.ImageURL = ImageUrl;
            //            //Item_Index.Image = arr;
            //            context.FeedItems_Index.Add(Item_Index);
            //            context.SaveChanges();
            //        }
            //        catch (Exception ex)
            //        {
            //            GeneralLogs.WriteLog("Error setFeedItemsImage" + ex.InnerException.InnerException == null ? ex.Message : ex.InnerException.InnerException.Message);
            //        }

            //    }
            //}
        }
        public static void SetItemsImage(List<FeedItem> items, Feed feed)
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
