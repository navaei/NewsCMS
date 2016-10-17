using HtmlAgilityPack;
using Mn.Framework.Business;
using Mn.Framework.Common;
using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using Tazeyab.Common;
using Tazeyab.Common.Models;

namespace Tazeyab.DomainClasses
{
    public class RwpBiz : BaseBusiness<RemoteWebPart>, IRwpBiz
    {
        //static List<RemoteWebPart> listCache = new List<RemoteWebPart>();
        public RemoteWebPart Get(string wpcode, bool tryNow = false)
        {
            wpcode = wpcode.ToLower();
            var cacheKey = "RWP_" + wpcode;
            if (!tryNow)
            {
                var cacheRWP = HttpContext.Current.Cache.Get(cacheKey);
                if (cacheRWP != null)
                    return cacheRWP as RemoteWebPart;
            }
            var webpart = base.GetList().SingleOrDefault(r => r.WebPartCode.ToLower() == wpcode);
            if (webpart == null)
                return null;
            if (!webpart.Active)
            {
                webpart.InnerHtml = "<h2>این بخش موقتا از دسترس خارج شده است.</h2>";
                return webpart;
            }
            else if (webpart.FailedRequestCount > 4)
            {
                webpart.FailedRequestCount = 0;
                base.Update(webpart);
            }

            if (webpart.RemoteWebPartRef.HasValue)
            {
                webpart.Pattern = webpart.ParentRemoteWebPart.Pattern;
                webpart.CacheTime = webpart.ParentRemoteWebPart.CacheTime;
                webpart.Css = webpart.ParentRemoteWebPart.Css;
                webpart.CssLink = webpart.ParentRemoteWebPart.CssLink;
                webpart.DisableLink = webpart.ParentRemoteWebPart.DisableLink;
                webpart.JavaScript = webpart.ParentRemoteWebPart.JavaScript;
                webpart.Keywords = webpart.ParentRemoteWebPart.Keywords;
                webpart.Replace = webpart.ParentRemoteWebPart.Replace;
                webpart.Active = webpart.Active == true ? webpart.ParentRemoteWebPart.Active : false;
            }
            try
            {
                HtmlDocument doc = Utility.GetXHtmlFromUri(webpart.Url);
                if (!string.IsNullOrEmpty(webpart.UrlPattern))
                {
                    var link = doc.DocumentNode.SelectSingleNode(webpart.UrlPattern).Attributes["href"].Value;
                    if (!link.ContainsX("http:") && !link.ContainsX("www."))
                    {
                        var url = new Uri(webpart.Url);
                        if (link[0] != '/')
                            link = "http://" + url.Host + "/" + link;
                        else
                            link = "http://" + url.Host + link;
                    }
                    doc = Utility.GetXHtmlFromUri(link);
                }
                var MainNode = doc.DocumentNode.SelectSingleNode(webpart.Pattern);
                MainNode.InnerHtml = MainNode.OuterHtml;
                if (!string.IsNullOrEmpty(webpart.Replace))
                {
                    var arr = webpart.Replace.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Count() > 0)
                    {
                        foreach (var item in arr)
                        {
                            MainNode.InnerHtml = MainNode.InnerHtml.Replace(item.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries)[0],
                            item.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries)[1]);
                        }
                    }
                }
                //----if href disable---
                if (webpart.DisableLink)
                {
                    MainNode.InnerHtml = Regex.Replace(MainNode.InnerHtml, @"\<[\/]*a[^\>]*\>", "");
                }
                else
                {
                    MainNode.InnerHtml = Regex.Replace(MainNode.InnerHtml, "<(a)([^>]+)>", @"<$1 target=""_blank""$2>");

                    string pattern = "((?:src|href)[\\s]*?)(?:\\=[\\s]*?[\\\"\\\'])[\\/*\\\\*]?(?!..+[s]?\\:[\\/]*)(.*?)(?:[\\s\\\"\\\'])";
                    var reg = new Regex(pattern, RegexOptions.IgnoreCase);
                    string prefix = @"http://" + new Uri(webpart.Url).Host + "/";
                    MainNode.InnerHtml = reg.Replace(MainNode.InnerHtml, "$1=\"" + prefix + "$2\"");
                    MainNode.InnerHtml = MainNode.InnerHtml.Replace("href='/", "href='http://" + new Uri(webpart.Url).Host + "/");
                    MainNode.InnerHtml = MainNode.InnerHtml.Replace("href=\"/", "href=\"http://" + new Uri(webpart.Url).Host + "/");

                    MainNode.InnerHtml = MainNode.InnerHtml.Replace("src='/", "src='http://" + new Uri(webpart.Url).Host + "/");
                    MainNode.InnerHtml = MainNode.InnerHtml.Replace("src=\"/", "src=\"http://" + new Uri(webpart.Url).Host + "/");
                }
                if (!string.IsNullOrEmpty(webpart.WebPartDesc))
                    MainNode.InnerHtml = MainNode.InnerHtml.Insert(0, "<div id='DWp_" + webpart.WebPartCode + "'><div class=DWpHeader>" + webpart.WebPartDesc + "</div><br />");

                if (!string.IsNullOrEmpty(webpart.Css))
                {
                    var fakeCss = "<style type='text/css' >";
                    fakeCss += webpart.Css;
                    fakeCss += "</style>";
                    MainNode.InnerHtml += fakeCss;
                }
                MainNode.InnerHtml += "</div>";

                webpart.InnerHtml = MainNode.InnerHtml;
                webpart.Active = true;

                HttpContext.Current.Cache.AddToCache(cacheKey, webpart, webpart.CacheTime);
                return webpart;
            }
            catch (Exception ex)
            {
                //-------when webpart request failed!!-----
                if (webpart.FailedRequestCount > 5)
                {
                    webpart.Active = false;
                    //context.SaveChanges();
                    base.Update(webpart);
                }
                else
                {
                    webpart.FailedRequestCount += 1;
                    base.Update(webpart);
                    //context.SaveChanges();
                }
                webpart.InnerHtml = "<h2 style='color:red'>وب پارت موقتا دچار مشکل شده است.</h2></br></br>" + ex.Message;
                return webpart;
            }
        }
        public IQueryable<RemoteWebPart> GetList()
        {
            return base.GetList();
        }
        public OperationStatus CreateEdit(RemoteWebPart rwp)
        {
            if (rwp.Id > 0)
                return base.Update(rwp);
            else
                return base.Create(rwp);
        }
        IQueryable<RemoteWebPart> IRwpBiz.GetByKeyword(string keyword)
        {
            return base.GetList().Where(w => w.Active == true && w.Keywords.ToLower().Contains(keyword.ToLower()));
        }
        public IQueryable<RemoteWebPart> GetByCats(List<long> catIds)
        {
            return (from p in base.GetList()
                    where
                    p.Active == true &&
                    p.Categories.Any(x => catIds.Contains(x.Id))
                    select p);

        }
        public IQueryable<RemoteWebPart> GetByTag(long tagId)
        {
            return (from p in base.GetList()
                    where
                    p.Active == true &&
                   p.Tags.Any(x => x.Id == tagId)
                    select p);

        }
    }
}