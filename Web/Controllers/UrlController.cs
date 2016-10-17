using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common.Models;

namespace Tazeyab.Web.Controllers
{
    public partial class UrlController : Controller
    {
        //
        // GET: /SNShare/

        public virtual ActionResult Index(long Item, Int16 SocialNetwork, string GotoLink)
        {
            //TazehaContext context = new TazehaContext();
            ////context.FeedItems.FirstOrDefault(x => x.FeedItemId == Item).Vote++;
            //context.SocialTrackers.Add(new SocialTracker() { FeedItemRef = Item, CreationDate = DateTime.Now, SocialNetworkRef = SocialNetwork });
            //context.SaveChanges();           
            return Redirect(GotoLink);
        }

        public virtual ActionResult Share(string item, SocialNetwork.SocialNetworkItems sn)
        {
            TazehaContext context = new TazehaContext();
            //context.FeedItems.FirstOrDefault(x => x.FeedItemId == Item).Vote++;
            if (item.Length > 10)
            {
                var id = Guid.Parse(item);
                context.SocialTrackers.Add(new SocialTracker() { FeedItemRef = id, CreationDate = DateTime.Now, SocialNetworkRef = (int)sn });
                context.SaveChanges();
            }
            else if (item.Length > 3)
            {
                var id = long.Parse(item);
                context.SocialTrackers.Add(new SocialTracker() { ItemRef = id, CreationDate = DateTime.Now, SocialNetworkRef = (int)sn });
                context.SaveChanges();
            }

            var url = "http://www.facebook.com/share.php?v=4&src=bm&u=";
            switch (sn)
            {
                case SocialNetwork.SocialNetworkItems.FaceBook:
                    url = "http://www.facebook.com/share.php?v=4&src=bm&u=";
                    break;
                case SocialNetwork.SocialNetworkItems.GooglePlus:
                    url = "https://plus.google.com/share?url=";
                    break;
                case SocialNetwork.SocialNetworkItems.Twitter:
                    url = "http://twitter.com/home?status";
                    break;
                case SocialNetwork.SocialNetworkItems.LinkedIn:
                    url = "http://www.linkedin.com/shareArticle?mini=true&url=";
                    break;
                case SocialNetwork.SocialNetworkItems.Balatarin:
                    url = "http://www.balatarin.com/links/submit?phase=2&url=";
                    break;
                case SocialNetwork.SocialNetworkItems.Cloob:
                    url = "http://www.cloob.com/share/link/add?url=";
                    break;
            }
            //add entity to social tracker
            return Redirect(url + Request.UrlReferrer.ToString());
        }

    }
}
