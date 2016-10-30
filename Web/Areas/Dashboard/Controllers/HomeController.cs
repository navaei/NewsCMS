using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Mn.NewsCms.Common;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Mn.NewsCms.Common.Content;
using Mn.NewsCms.Common.Helper;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Web.Areas.Dashboard.Models.Share;
using Mn.NewsCms.Web.Areas.Dashboard.Models;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class HomeController : BaseAdminController
    {
        private readonly ISiteBusiness _siteBusiness;
        private readonly IFeedItemBusiness _feedItemBusiness;
        private readonly IFeedBusiness _feedBusiness;
        private readonly IPostBiz _postBiz;
        private readonly IContactBusiness _contactBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly ICommentBiz _commentBiz;

        public HomeController(ISiteBusiness siteBusiness, IFeedItemBusiness feedItemBusiness, IFeedBusiness feedBusiness, IPostBiz postBiz,
            IContactBusiness contactBusiness, IUserBusiness userBusiness, ICommentBiz commentBiz)
        {
            _siteBusiness = siteBusiness;
            _feedItemBusiness = feedItemBusiness;
            _feedBusiness = feedBusiness;
            _postBiz = postBiz;
            _contactBusiness = contactBusiness;
            _userBusiness = userBusiness;
            _commentBiz = commentBiz;
        }

        [OutputCache(Duration = 600)]
        public virtual ActionResult Index(int page = 1)
        {
            var model = new DashboardModel();
            var today = DateTime.Today;

            model.SitesCount = _siteBusiness.GetList().Count();

            model.Feeds = _feedBusiness.GetList().OrderByDescending(f => f.LastUpdateDateTime).Take(10).ToList();
            model.ActiveFeedsCount = _feedBusiness.GetList().Where(f => f.Deleted == Common.Share.DeleteStatus.Active).Count();
            model.TodayFeedsCount = _feedBusiness.GetList().Where(f => f.LastUpdateDateTime >= today).Count();

            model.Comments = _commentBiz.GetList().OrderByDescending(c => c.Id).Take(8).ToList();
            model.Posts = _postBiz.GetList().Where(p => p.PostType == PostType.News).OrderByDescending(p => p.Id).Take(8).ToList();
            model.PostsCount = _postBiz.GetList().Count();

            model.UsersCount = _userBusiness.GetList().Count();
            model.Users = _userBusiness.GetList().OrderByDescending(u => u.Id).Take(8).ToList();

            model.MessagesCount = _contactBusiness.GetList().Count();

            model.TodayItemsCount = _feedItemBusiness.GetList().Where(t => t.CreateDate >= today).Count();

            return View(model);
        }

        public virtual ActionResult Header()
        {
            Header model = new Models.Share.Header();
            model.Messsages = _contactBusiness.GetList().OrderByDescending(m => m.CreateDate)
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
            //var context = new TazehaContext();
            //var blog = context.NewsletterUsers.FirstOrDefault(s => s.BlogAddress.Contains(url));
            //blog.PageRank = rank;
            //context.SaveChanges();
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