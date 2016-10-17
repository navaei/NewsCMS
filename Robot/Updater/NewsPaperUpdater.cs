using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Tazeyab.Common;
using Tazeyab.Common.Models;
using Tazeyab.CrawlerEngine.Helper;
using Tazeyab.Common.Config;
using Mn.Framework.Common;
using CrawlerEngine.Helper;

namespace Tazeyab.CrawlerEngine.Updater
{
    public class NewsPaperUpdater : IStarter<StartUp>
    {
        public void Start(StartUp inputParams)
        {
            Start(inputParams.StartUpConfig);
        }
        public int Start(string NewsPaperCatCode)
        {
            string baseSiteAddress = "http://m.jaaar.com";

            HtmlDocument xhtml = Requester.GetXHtmlFromUri(baseSiteAddress);
            HtmlNodeCollection nods = xhtml.DocumentNode.SelectNodes(@"//ul[@id='frontpage']/li");
            var entiti = new TazehaContext();
            var currentCat = entiti.Categories.SingleOrDefault(x => x.Code == NewsPaperCatCode);
            var DefNewPaperPath = ServiceFactory.Get<IAppConfigBiz>().GetConfig("DefNewPaperPath");
            int Ret = 0;
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
                if (!Imager.SaveImage(img, DefNewPaperPath, filename + ".jpg"))
                    break;
                img = Imager.ResizeBitmap(img, 210, 220);
                Imager.SaveImage(img, DefNewPaperPath, "Th_" + filename + ".jpg");

                photoitem.PhotoThumbnail = "Th_" + filename + ".jpg";
                photoitem.PhotoURL = filename + ".jpg";
                photoitem.CatId = currentCat.Id;
                photoitem.CreationDate = DateTime.Now;
                entiti.PhotoItems.Add(photoitem);
                entiti.SaveChanges();
                Ret++;
            }
            return Ret;
        }
        public int StartNew(string NewsPaperCatCode)
        {
            string baseSiteAddress = "http://www.jaaar.com";
            string pageAddress = "http://www.jaaar.com/frontpage#!/table/all";

            HtmlDocument xhtml = Requester.GetXHtmlFromUri(pageAddress);
            var nods = xhtml.DocumentNode.SelectNodes(@"//div[@id='thumbs']//p[contains(@class,'all')]");
            var entiti = new TazehaContext();
            var currentCat = entiti.Categories.SingleOrDefault(x => x.Code == NewsPaperCatCode);
            var DefNewPaperPath = ServiceFactory.Get<IAppConfigBiz>().GetConfig("DefNewPaperPath");
            int Ret = 0;
            foreach (var item in nods)
            {
                var photoitem = new PhotoItem();
                var attribute = new NewspaperItemAttribute();

                photoitem.PhotoURL = baseSiteAddress + item.ChildNodes.FindFirst("img").GetAttributeValue("data-src", "").Replace("_thumb", "");
                photoitem.Title = item.ChildNodes.FindFirst("a").InnerText;
                attribute.Cat = item.GetAttributeValue("class", "").Replace("all", "").Trim();
                attribute.WebSite = item.GetAttributeValue("data-website", "");
                photoitem.Attribute = attribute;

                Stream stream = Requester.GetStreamFromUri(photoitem.PhotoURL, "");
                Bitmap img = Imager.getImageFromBStream(stream);
                if (img == null)
                    continue;

                string filename = DateTime.Now.ToString("yyyyMMddss") + "_" + photoitem.Title.RemoveBadCharacterInURL();
                if (!Imager.SaveImage(img, DefNewPaperPath, filename + ".jpg"))
                    break;

                //img = Imager.ResizeBitmap(img, 210, 220);
                //Imager.SaveImage(img, DefNewPaperPath, "Th_" + filename + ".jpg");

                //photoitem.PhotoThumbnail = "Th_" + filename + ".jpg";
                photoitem.PhotoURL = filename + ".jpg";
                photoitem.CatId = currentCat.Id;
                photoitem.CreationDate = DateTime.Now;
                entiti.PhotoItems.Add(photoitem);
                entiti.SaveChanges();
                Ret++;
            }
            return Ret;
        }
        /// <summary>
        /// baraye upload Newsaper rooye FTP yek folder hamname DefNewPaperPath rooye local va server ijad mishavad
        /// va pass az zakhire fileha rooye local,yekbare be ftp enteghal dade mishavad
        /// </summary>
        public void UploadNewsPaperImgToFtp()
        {

            var VirtulPath = ServiceFactory.Get<IAppConfigBiz>().GetConfig("DefNewPaperPath");

            string fullpath = AppDomain.CurrentDomain.BaseDirectory + VirtulPath;

            //string ftpfullpath = "ftp://ftp.tazeyab.com/NewsPaper/";
            //FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
            //ftp.Credentials = new NetworkCredential(Config.getConfig<string>("FtpUserName"), Config.getConfig<string>("FtpPassword"));
            ////userid and password for the ftp server to given  

            //ftp.KeepAlive = true;
            //ftp.UseBinary = true;
            //ftp.Method = WebRequestMethods.Ftp.UploadFile;

            DirectoryInfo directory = new DirectoryInfo(fullpath);
            foreach (var file in directory.GetFiles())
            {
                Utility.FileUploadToFtp(file, "NewsPaper/");
                //FileStream fs = file.OpenRead();
                //byte[] buffer = new byte[fs.Length];
                //fs.Read(buffer, 0, buffer.Length);
                //fs.Close();
                //ftpstream.Write(buffer, 0, buffer.Length);
                file.Delete();
            }
            //ftpstream.Close();
        }
    }
}
