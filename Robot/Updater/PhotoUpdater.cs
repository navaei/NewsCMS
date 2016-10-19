using System;
using System.Linq;
using HtmlAgilityPack;
using System.IO;
using CrawlerEngine;
using System.Drawing;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.EventsLog;
using Mn.NewsCms.Robot.Helper;
using Mn.NewsCms.Common;
using System.Net;
using System.Web;
using Mn.NewsCms.Common.Config;
using Mn.Framework.Common;
using Mn.NewsCms.Robot.Parser;
using CrawlerEngine.Helper;

namespace Mn.NewsCms.Robot.Updater
{

    public class PhotoUpdater
    {      
        public bool saveImageByPattern(string PageUrl, string Pattern, string AttributeName, string DirectoryPath, string FileName)
        {
            try
            {
                var doc = Requester.GetXHtmlFromUri(PageUrl);
                var path = HtmlParser.GetTagAttribute(doc, Pattern, AttributeName);
                var stream = Requester.GetStreamFromUri(path, "");
                var img = Imager.getImageFromBStream(stream);
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
