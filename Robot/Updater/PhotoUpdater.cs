using System;
using System.Linq;
using HtmlAgilityPack;
using System.IO;
using CrawlerEngine;
using System.Drawing;
using Tazeyab.Common.Models;
using Tazeyab.Common.EventsLog;
using Tazeyab.CrawlerEngine.Helper;
using Tazeyab.Common;
using System.Net;
using System.Web;
using Tazeyab.Common.Config;
using Mn.Framework.Common;
using Tazeyab.CrawlerEngine.Parser;
using CrawlerEngine.Helper;

namespace Tazeyab.CrawlerEngine.Updater
{

    public class PhotoUpdater
    {
        public void startNewsPaper(string NewsPaperCatCode)
        {
            //TazehaContext entiti = new TazehaContext();
            //var links = entiti.MMLinks.Where(x => 1 == 1);
            //var Defpath = Config.getConfig("DefNewPaperPath");
            //foreach (var link in links)
            //{
            //    saveImageByPattern(link.MMLink, link.MMPattern, link.AttributeName, Defpath, link.MMLinkId.ToString());
            //}
            string baseSiteAddress = "http://m.jaaar.com";

            HtmlDocument xhtml = Requester.GetXHtmlFromUri(baseSiteAddress);
            HtmlNodeCollection nods = xhtml.DocumentNode.SelectNodes(@"//ul[@id='frontpage']/li");
            var entiti = new TazehaContext();
            var Id = entiti.Categories.SingleOrDefault(x => x.Code == NewsPaperCatCode).Id;
            var Defpath = ServiceFactory.Get<IAppConfigBiz>().GetConfig("DefNewPaperPath");
            foreach (var item in nods)
            {
                PhotoItem photoitem = new PhotoItem();
                photoitem.PhotoURL = baseSiteAddress + item.ChildNodes.FindFirst("a").GetAttributeValue("href", "");
                photoitem.Title = item.ChildNodes.FindFirst("a").GetAttributeValue("title", "");
                //photoitem.PhotoThumbnail = item.ChildNodes.FindFirst("a").ChildNodes.FindFirst("img").GetAttributeValue("src", string.Empty).ToString();

                Stream stream = Requester.GetStreamFromUri(photoitem.PhotoURL, "");
                global::CrawlerEngine.Helper.Imager Uimg = new global::CrawlerEngine.Helper.Imager();
                Bitmap img = Imager.getImageFromBStream(stream);
                if (img == null)
                    continue;
                // img.SaveThumbnail(stream, @"..\Photo\NewPaper\", "Th_" + FileName + ".jpg", 220);
                string filename = Requester.GetFileNameFromURL(photoitem.PhotoURL) + DateTime.Now.ToShortDateString().Replace("/", "");
                Imager.SaveImage(img, Defpath, filename + ".jpg");
                img = Imager.ResizeBitmap(img, 210, 220);
                Imager.SaveImage(img, Defpath, "Th_" + filename + ".jpg");

                photoitem.PhotoThumbnail = "Th_" + filename + ".jpg";
                photoitem.PhotoURL = filename + ".jpg";
                photoitem.Id = Id;
                photoitem.CreationDate = DateTime.Now;
                entiti.PhotoItems.Add(photoitem);
                entiti.SaveChanges();
            }

        }
        public void startNewsPaperFull(string NewsPaperCatCode)
        {            
            string baseSiteAddress = "http://www.jaaar.com/frontpage#!/table/all";

            HtmlDocument xhtml = Requester.GetXHtmlFromUri(baseSiteAddress);
            HtmlNodeCollection nods = xhtml.DocumentNode.SelectNodes(@"//div[@id='thumbs']//p[contains(@class,'all')]/");
            var entiti = new TazehaContext();
            var Id = entiti.Categories.SingleOrDefault(x => x.Code == NewsPaperCatCode).Id;
            var Defpath = ServiceFactory.Get<IAppConfigBiz>().GetConfig("DefNewPaperPath");
            foreach (var item in nods)
            {
                PhotoItem photoitem = new PhotoItem();
                photoitem.PhotoURL = baseSiteAddress + item.ChildNodes.FindFirst("a").GetAttributeValue("href", "");
                photoitem.Title = item.ChildNodes.FindFirst("a").GetAttributeValue("title", "");
                //photoitem.PhotoThumbnail = item.ChildNodes.FindFirst("a").ChildNodes.FindFirst("img").GetAttributeValue("src", string.Empty).ToString();

                Stream stream = Requester.GetStreamFromUri(photoitem.PhotoURL, "");
                Bitmap img = Imager.getImageFromBStream(stream);
                if (img == null)
                    continue;
                // img.SaveThumbnail(stream, @"..\Photo\NewPaper\", "Th_" + FileName + ".jpg", 220);
                string filename = Requester.GetFileNameFromURL(photoitem.PhotoURL) + DateTime.Now.ToShortDateString().Replace("/", "");
                Imager.SaveImage(img, Defpath, filename + ".jpg");
                img = Imager.ResizeBitmap(img, 210, 220);
                Imager.SaveImage(img, Defpath, "Th_" + filename + ".jpg");

                photoitem.PhotoThumbnail = "Th_" + filename + ".jpg";
                photoitem.PhotoURL = filename + ".jpg";
                photoitem.Id = Id;
                photoitem.CreationDate = DateTime.Now;
                entiti.PhotoItems.Add(photoitem);
                entiti.SaveChanges();
            }

        }
        public bool saveImageByPattern(string PageUrl, string Pattern, string AttributeName, string DirectoryPath, string FileName)
        {
            try
            {
                var doc = Requester.GetXHtmlFromUri(PageUrl);
                string path = HtmlParser.GetTagAttribute(doc, Pattern, AttributeName);
                Stream stream = Requester.GetStreamFromUri(path, "");
                Bitmap img = Imager.getImageFromBStream(stream);
                var img_th = Imager.ResizeBitmap(img, 220, 220);
                Imager.SaveImage(img_th, DirectoryPath, "Th_" + FileName + ".jpg");
                Imager.SaveImage(img, DirectoryPath, FileName + ".jpg");
                return true;

            }
            catch (Exception ex)
            {
                GeneralLogs.WriteLog("GetImageStreamByPattern" + ex.Message, TypeOfLog.Error);
            }
            return false;
        }
    }

}
