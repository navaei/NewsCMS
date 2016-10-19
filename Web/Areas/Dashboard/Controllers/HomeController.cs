using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Web.Areas.Dashboard.Models.Share;
using Mn.NewsCms.Web.Areas.Dashboard.Models;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class HomeController : BaseAdminController
    {
        [OutputCache(Duration = 600)]
        public virtual ActionResult Index(int page = 1)
        {
            var model = new DashboardModel();
            var today = DateTime.Today;

            model.SitesCount = Ioc.SiteBiz.GetList().Count();

            model.Feeds = Ioc.FeedBiz.GetList().OrderByDescending(f => f.LastUpdateDateTime).Take(10).ToList();
            model.ActiveFeedsCount = Ioc.FeedBiz.GetList().Where(f => f.Deleted == Common.Share.DeleteStatus.Active).Count();
            model.TodayFeedsCount = Ioc.FeedBiz.GetList().Where(f => f.LastUpdateDateTime >= today).Count();

            model.Comments = Ioc.CommentBiz.GetList().OrderByDescending(c => c.Id).Take(8).ToList();
            model.Posts = Ioc.PostBiz.GetList().Where(p => p.PostType == PostType.News).OrderByDescending(p => p.Id).Take(8).ToList();
            model.PostsCount = Ioc.PostBiz.GetList().Count();

            model.UsersCount = Ioc.UserBiz.GetList().Count();
            model.Users = Ioc.UserBiz.GetList().OrderByDescending(u => u.Id).Take(8).ToList();

            model.MessagesCount = Ioc.ContactBiz.GetList().Count();

            model.TodayItemsCount = Ioc.ItemBiz.GetList().Where(t => t.CreateDate >= today).Count();

            return View(model);
        }

        public virtual ActionResult Header()
        {
            Header model = new Models.Share.Header();
            model.Messsages = Ioc.ContactBiz.GetList().OrderByDescending(m => m.CreateDate)
                .Take(7).ToList().Select(m => new HeaderMessage()
                {
                    Id = m.Id,
                    Content = m.Title,
                    Date = m.CreateDate.ToPersianBeautiful(),
                    Sender = m.Name
                }).ToList();

            return View("_Header", model);
        }

        public virtual void UpdateRank(string url, byte rank)
        {
            var context = new TazehaContext();
            var blog = context.NewsletterUsers.FirstOrDefault(s => s.BlogAddress.Contains(url));
            blog.PageRank = rank;
            context.SaveChanges();
        }

        public virtual ActionResult Search(string q)
        {
            if (Utility.HasFaWord(q))
            {

            }
            else
            {
                return RedirectToAction(MVC.Dashboard.Feed.Index(0, null, q));
            }

            return RedirectToAction(MVC.Dashboard.Home.Index());

        }

    }
}