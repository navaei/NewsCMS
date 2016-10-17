using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication; // Add a ref. to System.ServiceModel.dll asm.
using System.Text;
using System.Web; // Add a ref. to System.Web.dll asm.
using System.Web.Mvc; // Add a ref. to C:\Program Files\Microsoft ASP.NET\ASP.NET MVC 4\Assemblies\System.Web.Mvc.dll asm.
using System.Xml;
using Tazeyab.Common;
using Tazeyab.Common.Models;

namespace Tazeyab.WebLogic
{
    public class FeedResult : ActionResult
    {
        readonly string _feedTitle;
        readonly List<SyndicationItem> _allItems;
        readonly string _language;
        readonly DateTime _lastUpdatedTime;
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
                var uri = new Uri("http://" + HttpContext.Current.Request.Url.Host + "/site/" + item.SiteUrl + "/" + item.Id);
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

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;
            writeToResponse(response);
        }

        private void writeToResponse(HttpResponseBase response)
        {
            var feed = new SyndicationFeed
            {
                Title = new TextSyndicationContent(_feedTitle),//.CorrectRtl()
                Language = _language,
                Items = _allItems,
                LastUpdatedTime = _lastUpdatedTime,
                //BaseUri = new Uri("http://www.tazeyab.com"),                
                //Links = new System.Collections.ObjectModel.Collection<SyndicationLink>().Add(new SyndicationLink(new Uri("http://www.tazeyab.com"))),
                Description = new TextSyndicationContent("تازه ترین مطالب وب سایت های ایرانی-تازه یاب-" + _feedTitle)
            };

            //SyndicationLink link = new SyndicationLink(new Uri("http://www.tazeyab.com" + "/myfeed.xml"));
            //link.RelationshipType = "self";
            //link.MediaType = "text/html";
            //link.Title = "Cambia Research Feed";
            //feed.Links.Add(link);

            feed.Links.Add(new SyndicationLink(new Uri("http://www.tazeyab.com")));
            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = "application/rss+xml";
            using (var rssWriter = XmlWriter.Create(response.Output, new XmlWriterSettings { Indent = true }))
            {
                var formatter3 = new Rss20FeedFormatter(feed);
                formatter3.WriteTo(rssWriter);
                rssWriter.Close();
                response.End();
            }
        }
    }
}
