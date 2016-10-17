using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tazeyab.Common;
using Tazeyab.Common.Models;
using Tazeyab.DomainClasses.ContentManagment;
using Tazeyab.Web.Models;
using Tazeyab.Web.WebLogic;


namespace Tazeyab.Web.Controllers
{
    [Authorize]
    public partial class UserController : BaseController
    {
        //
        // GET: /User/
        public virtual ActionResult Index()
        {

            var model = new ReaderModel();
            var membership = new WebLogic.TzMembership();
            var MUser = membership.GetCurrentUser();

            model.UserTitle = membership.GetCurrentUserTitle();
            ViewBag.Title = "صفحه شخصی " + model.UserTitle;
            model.Tags = Ioc.TagBiz.GetList()
                .Where(x => x.Users.Any(c => c.Id == MUser.Id)).Select(t => new TagViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    EnValue = t.EnValue
                }).ToList();

            model.Categories = Ioc.CatBiz.GetList().Where(x => x.Users.Any(c => c.Id == MUser.Id)).
                Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Code = c.Code,
                    ParentId = c.ParentId
                })
                .ToList();
            model.Categories.ForEach(x => x.ParentId = x.ParentId > 0 && model.Categories.Any(y => y.Id == x.ParentId) ? x.ParentId : 0);
            model.Sites = Ioc.SiteBiz.GetList().Where(x => x.Users.Any(c => c.Id == MUser.Id))
                .Select(x => new SiteOnlyTitle { Id = x.Id, SiteUrl = x.SiteUrl, SiteTitle = x.SiteTitle }).ToList();

            var recBiz = Ioc.RecomendBiz;
            model.RecomendTags = recBiz.Tags(10, model.Tags.Select(t => t.Id).ToList()).Select(t => new TagViewModel()
            {
                Id = t.Id,
                Title = t.Title,
                EnValue = t.EnValue
            }).ToList();
            model.RecomendCats = recBiz.Cats(5).Select(c => new CategoryViewModel()
            {
                Id = c.Id,
                Title = c.Title,
                Code = c.Code
            }).ToList();
            model.RecomendSites = recBiz.Sites(10, model.Sites.Select(s => s.Id).ToList());

            return View("Index." + TazeyabConfig.ThemeName, model);
        }

        [HttpGet]
        public virtual ActionResult CatsUsersDeleting(string CatCode)
        {
            try
            {
                var user = Ioc.UserBiz.GetList().SingleOrDefault(x => x.UserName == User.Identity.Name);
                var cat = user.Categories.SingleOrDefault(c => c.Title == CatCode || c.Code == CatCode);
                user.Categories.Remove(cat);
                Ioc.UserBiz.Update(user);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public virtual ActionResult CatsUsersAdding(string CatCode)
        {
            try
            {
                TazehaContext context = new TazehaContext();
                var user = context.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);
                context.Categories.SingleOrDefault(x => x.Code == CatCode).Users.Add(user);
                context.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public virtual ActionResult SitesUsersDeleting(string SiteURL)
        {
            try
            {
                TazehaContext context = new TazehaContext();
                var user = context.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);
                context.Sites.SingleOrDefault(x => x.SiteUrl == SiteURL).Users.Remove(user);
                context.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public virtual ActionResult SitesUsersAdding(string SiteURL)
        {
            try
            {
                TazehaContext context = new TazehaContext();
                var user = context.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);
                context.Sites.SingleOrDefault(x => x.SiteUrl == SiteURL).Users.Add(user);
                context.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public virtual ActionResult TagsUsersDeleting(string TagContent)
        {
            try
            {
                TazehaContext context = new TazehaContext();
                var user = context.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);
                context.Tags.SingleOrDefault(x => x.EnValue == TagContent || x.Value == TagContent).Users.Remove(user);
                context.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public virtual ActionResult TagsUsersAdding(string TagContent)
        {
            try
            {
                TazehaContext context = new TazehaContext();
                var user = context.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);
                context.Tags.SingleOrDefault(x => x.EnValue == TagContent || x.Value == TagContent).Users.Add(user);
                context.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public virtual bool IsUserFlow(string Content, string EntityCode)
        {
            return Ioc.UserBiz.IsUserFlow(EntityCode, User.Identity.Name, Content);
        }
    }
}
