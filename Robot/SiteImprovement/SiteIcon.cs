using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using CrawlerEngine;
using Tazeyab.Common.Models;
using Tazeyab.Common.EventsLog;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Tazeyab.CrawlerEngine.SiteImprovement
{

    public class SiteIcon
    {
        #region SiteIcons
        public static void StartSetSiteIcons(int StartIndex = 0)
        {           
            //Dictionary<long, string> sites = entiti.Sites.Where(x => x.SiteLogo == null && (x.IsBlog == null || x.IsBlog == 0)).OrderBy(x => x.Id).Skip(StartIndex).Take(1000).Select(x => new { x.Id, x.SiteUrl }).ToDictionary(x => x.Id, x => x.SiteUrl);
            //foreach (var site in sites)
            //{
            //    SetSiteIcon(site.Key, site.Value);
            //}
            //if (entiti.Sites.Where(x => x.SiteLogo == null && (x.IsBlog == null || x.IsBlog == 0)).OrderBy(x => x.Id).Skip(StartIndex).Count() > 100)
            //    StartSetSiteIcons(StartIndex + 1000);
        }

        private static Uri GetFaviconUrl(string siteURL)
        {
            siteURL = siteURL.IndexOfX("www.") > -1 || siteURL.IndexOfX("http://") > -1 ? siteURL : "www." + siteURL;
            siteURL = siteURL.IndexOfX("http://") > -1 ? siteURL : "http://" + siteURL;
            var url = new Uri(siteURL);
            var faviconUrl = new Uri(string.Format("{0}://{1}/favicon.ico", url.Scheme, url.Host));
            try
            {
                using (var httpWebResponse = WebRequest.Create(faviconUrl).GetResponse() as HttpWebResponse)
                {
                    if (httpWebResponse != null && httpWebResponse.StatusCode == HttpStatusCode.OK)
                    {
                        // Log("Found a /favicon.ico file for {0}", url);
                        return faviconUrl;
                    }
                }
            }
            catch (WebException)
            {
            }
            String htmlText = "  ";
            try
            {
                using (var httpWebResponse = WebRequest.Create(siteURL).GetResponse() as HttpWebResponse)
                {
                    if (httpWebResponse != null && httpWebResponse.StatusCode == HttpStatusCode.OK)
                    {
                        Stream stream = httpWebResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(stream);
                        htmlText = reader.ReadToEnd();
                        //site.IndexPageText = htmlText;
                    }
                }
            }
            catch (Exception ex)
            {
                GeneralLogs.WriteLog("Error @SetIcons GetWebText" + ex.Message.SubstringX(0, 150));
            }


            foreach (Match match in Regex.Matches(htmlText, "<link .*? href=\"(.*?.ico)\""))
            {
                siteURL = siteURL[siteURL.Length - 1] == '/' ? siteURL.Remove(siteURL.Length - 1, 1) : siteURL;
                return new Uri(siteURL + "/" + match.Groups[1].Value.Replace("~/", ""));
            }
            return null;
        }
        private static void SetSiteIcon(decimal siteID, string SiteURL)
        {
            //try
            //{
            //    Site site = entiti.Sites.Where(x => x.Id == siteID).First();
            //    site.SiteLogo = GetSiteIcon(SiteURL);
            //    entiti.SaveChanges();
            //    GeneralLogs.WriteLog("OK @SetIcons siteID:" + siteID + " ICON:" + SiteURL);
            //}
            //catch (Exception ex)
            //{
            //    GeneralLogs.WriteLog("Error GetWebText" + ex.Message);
            //}
        }
        public static byte[] GetSiteIcon(string SiteURL)
        {
            try
            {
                Uri FavIconUri = GetFaviconUrl(SiteURL);
                if (FavIconUri != null)
                    if (string.IsNullOrEmpty(FavIconUri.ToString()))
                        return null;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(FavIconUri.ToString());
                request.UserAgent = "PersianFeedCrawler";

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                //byte[] arr = memoryStream.ToArray();
                return RenderSiteIconByIconFormat(16, memoryStream);
            }
            catch
            {
                return null;
            }
        }

        private static byte[] RenderSiteIconByIconFormat(int MaxSideSize, Stream Buffer)
        {
            Bitmap bitMap = new Bitmap(Buffer);
            var iconHandle = bitMap.GetHicon();// new Icon(Buffer, MaxSideSize, MaxSideSize);
            var icon = System.Drawing.Icon.FromHandle(iconHandle);
            MemoryStream ms = new MemoryStream();
            icon.Save(ms);
            return ms.ToArray();
        }
        private static byte[] ResizeFromStream(int MaxSideSize, Stream Buffer, string fileName)
        {
            byte[] byteArray = null;  // really make this an error gif

            try
            {
                Bitmap bitMap = new Bitmap(Buffer);
                int intOldWidth = bitMap.Width;
                int intOldHeight = bitMap.Height;

                int intNewWidth;
                int intNewHeight;

                int intMaxSide;

                if (intOldWidth >= intOldHeight)
                {
                    intMaxSide = intOldWidth;
                }
                else
                {
                    intMaxSide = intOldHeight;
                }

                if (intMaxSide > MaxSideSize)
                {
                    //set new width and height
                    double dblCoef = MaxSideSize / (double)intMaxSide;
                    intNewWidth = Convert.ToInt32(dblCoef * intOldWidth);
                    intNewHeight = Convert.ToInt32(dblCoef * intOldHeight);
                }
                else
                {
                    intNewWidth = intOldWidth;
                    intNewHeight = intOldHeight;
                }

                Size ThumbNailSize = new Size(intNewWidth, intNewHeight);
                System.Drawing.Image oImg = System.Drawing.Image.FromStream(Buffer);
                System.Drawing.Image oThumbNail = new Bitmap(ThumbNailSize.Width, ThumbNailSize.Height);

                Graphics oGraphic = Graphics.FromImage(oThumbNail);
                oGraphic.CompositingQuality = CompositingQuality.HighQuality;
                oGraphic.SmoothingMode = SmoothingMode.HighQuality;
                oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //oGraphic.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, MaxSideSize, MaxSideSize));
                //Rectangle oRectangle = new Rectangle
                //    (0, 0, ThumbNailSize.Width, ThumbNailSize.Height);

                //oGraphic.DrawImage(oImg, oRectangle);

                MemoryStream ms = new MemoryStream();
                oThumbNail.Save(ms, ImageFormat.Icon);
                byteArray = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(byteArray, 0, Convert.ToInt32(ms.Length));

                oGraphic.Dispose();
                oImg.Dispose();
                ms.Close();
                ms.Dispose();
            }
            catch (Exception)
            {
                int newSize = MaxSideSize - 20;
                Bitmap bitMap = new Bitmap(newSize, newSize);
                Graphics g = Graphics.FromImage(bitMap);
                g.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(0, 0, newSize, newSize));

                Font font = new Font("Courier", 8);
                SolidBrush solidBrush = new SolidBrush(Color.Red);
                g.DrawString("Failed File", font, solidBrush, 10, 5);
                g.DrawString(fileName, font, solidBrush, 10, 50);

                MemoryStream ms = new MemoryStream();
                bitMap.Save(ms, ImageFormat.Icon);
                byteArray = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(byteArray, 0, Convert.ToInt32(ms.Length));

                ms.Close();
                ms.Dispose();
                bitMap.Dispose();
                solidBrush.Dispose();
                g.Dispose();
                font.Dispose();

            }
            return byteArray;
        }

        //public static Bitmap Resize(Bitmap image, int newWidth, int newHeight)
        //{
        //    try
        //    {
        //        Bitmap newImage = new Bitmap(newWidth, Calculations(image.Width, image.Height, newWidth));

        //        using (Graphics gr = Graphics.FromImage(newImage))
        //        {
        //            gr.SmoothingMode = SmoothingMode.AntiAlias;
        //            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //            gr.DrawImage(image, new Rectangle(0, 0, newImage.Width, newImage.Height));

        //            var myBrush = new SolidBrush(Color.FromArgb(70, 205, 205, 205));

        //            double diagonal = Math.Sqrt(newImage.Width * newImage.Width + newImage.Height * newImage.Height);

        //            Rectangle containerBox = new Rectangle();

        //            containerBox.X = (int)(diagonal / 10);
        //            //float messageLength = (float)(diagonal / message.Length * 1);
        //            //containerBox.Y = -(int)(messageLength / 1.6);

        //            //Font stringFont = new Font("verdana", messageLength);

        //            StringFormat sf = new StringFormat();

        //            float slope = (float)(Math.Atan2(newImage.Height, newImage.Width) * 180 / Math.PI);

        //            gr.RotateTransform(slope);
        //            //gr.DrawString(message, stringFont, myBrush, containerBox, sf);
        //            return newImage;
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        throw exc;
        //    }
        //}

        //public static int Calculations(decimal w1, decimal h1, int newWidth)
        //{
        //    decimal height = 0;
        //    decimal ratio = 0;


        //    if (newWidth < w1)
        //    {
        //        ratio = w1 / newWidth;
        //        height = h1 / ratio;

        //        return height.To<int>();
        //    }

        //    if (w1 < newWidth)
        //    {
        //        ratio = newWidth / w1;
        //        height = h1 * ratio;
        //        return height.To<int>();
        //    }

        //    return height.To<int>();
        //}
        #endregion

    }

}
