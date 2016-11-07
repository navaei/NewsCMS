using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Mn.NewsCms.Common;
// Add a ref. to System.ServiceModel.dll asm.
// Add a ref. to System.Web.dll asm.
// Add a ref. to C:\Program Files\Microsoft ASP.NET\ASP.NET MVC 4\Assemblies\System.Web.Mvc.dll asm.

namespace Mn.NewsCms.WebCore.WebLogic
{
    public class FeedResult : IActionResult
    {
        readonly string _feedTitle;
        readonly List<SyndicationItem> _allItems;
        readonly string _language;
        readonly DateTime _lastUpdatedTime;
        private ActionContext _actionContext;
        public FeedResult(string feedTitle, IList<FeedItem> rssItems, string language = "fa-IR")
        {
            _feedTitle = feedTitle;
            _allItems = mapToSyndicationItem(rssItems);
            _language = language;
            _lastUpdatedTime = rssItems.Count > 0 ? rssItems[0].CreateDate.ToUniversalTime() : DateTime.Now.ToUniversalTime();

        }

        private static List<SyndicationItem> mapToSyndicationItem(IList<FeedItem> rssItems)
        {
            var results = new List<SyndicationItem>();
            foreach (var item in rssItems)
            {
                var uri = new Uri("http://" + Resources.Core.SiteUrl + "/site/" + item.SiteUrl + "/" + item.Id);
                var feedItem = new SyndicationItem(
                        title: item.Title,//.CorrectRtl(),
                        content: item.Description.CorrectRtlBody(),
                        itemAlternateLink: uri,
                        id: uri.ToString(),
                        lastUpdatedTime: item.PubDate.HasValue ? item.PubDate.Value : item.CreateDate
                        );
                feedItem.PublishDate = feedItem.LastUpdatedTime;
                feedItem.Authors.Add(new SyndicationPerson(item.SiteTitle, item.SiteTitle, uri.Host));
                results.Add(feedItem);
            }
            return results;
        }

        //public override void ExecuteResult(ControllerContext context)
        //{
        //    if (context == null)
        //        throw new ArgumentNullException("context");

        //    var response = context.HttpContext.Response;
        //    writeToResponse(response);
        //}

        private void writeToResponse(HttpResponse response)
        {
            var feed = new SyndicationFeed
            {
                Title = new TextSyndicationContent(_feedTitle),//.CorrectRtl()
                Language = _language,
                Items = _allItems,
                LastUpdatedTime = _lastUpdatedTime,
                //Links = new System.Collections.ObjectModel.Collection<SyndicationLink>().Add(new SyndicationLink(new Uri("http://www.tazeyab.com"))),
                Description = new TextSyndicationContent("تازه ترین مطالب وب سایت های ایرانی-" + _feedTitle)
            };

            feed.Links.Add(new SyndicationLink(new Uri("http://" + Resources.Core.SiteUrl)));
            //response.ContentEncoding = Encoding.UTF8;
            response.ContentType = "application/rss+xml";
            using (var rssWriter = XmlWriter.Create(response.Body, new XmlWriterSettings { Indent = true }))
            {
                var formatter3 = new Rss20FeedFormatter(feed);
                formatter3.WriteTo(rssWriter);
                rssWriter.Close();
                //response.End();
            }
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _actionContext = context;

            var response = context.HttpContext.Response;

            return Task.Run(() => writeToResponse(response));
        }
    }
}
