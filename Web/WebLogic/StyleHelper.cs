using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Mn.NewsCms.Web.WebLogic
{
    public class StyleHelper
    {
        public static string bingDailyWallPaper()
        {
            try
            {
                var bingWallPaperUrl = HttpContext.Current.Cache.Get("bingWallPaperUrl");
                if (bingWallPaperUrl != null)
                    return bingWallPaperUrl.ToString();
                XmlDocument doc = new XmlDocument();
                doc.Load("http://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=en-US");
                var link = doc.SelectSingleNode("//url").InnerText;
                link = "http://www.bing.com" + link;
                HttpContext.Current.Cache.AddToChache_Hours("bingWallPaperUrl", link, 4);
                return link;
            }
            catch
            {
                return string.Empty;
            }
        }
        //public static string TagWallPaper(int tagId)
        //{
        //    try
        //    {
                
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}
    }
}