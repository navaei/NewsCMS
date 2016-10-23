using System;
using System.Linq;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Share;
using Mn.NewsCms.Web.Models;
using Mn.NewsCms.Web.WebLogic;


namespace Mn.NewsCms.Web.Controllers
{
    [Authorize]
    public partial class UserController : BaseController
    {
        private readonly ITagBusiness _tagBusiness;
        private readonly ICategoryBusiness _categoryBusiness;
        private readonly ISiteBusiness _siteBusiness;
        private readonly IRecomendBiz _recomendBiz;
        private readonly IUserBusiness _userBusiness;

        public UserController(ITagBusiness tagBusiness, ICategoryBusiness categoryBusiness, ISiteBusiness siteBusiness,
            IRecomendBiz recomendBiz, IUserBusiness userBusiness)
        {
            _tagBusiness = tagBusiness;
            _categoryBusiness = categoryBusiness;
            _siteBusiness = siteBusiness;
            _recomendBiz = recomendBiz;
            _userBusiness = userBusiness;
        }

        public virtual ActionResult Index()
        {

            var model = new ReaderModel();
            var membership = new WebLogic.CmsMembership(_userBusiness);
            var MUser = membership.GetCurrentUser();

            model.UserTitle = membership.GetCurrentUserTitle();
            ViewBag.Title = "صفحه شخصی " + model.UserTitle;
            model.Tags = _tagBusiness.GetList()
                .Where(x => x.Users.Any(c => c.Id == MUser.Id)).Select(t => new TagViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    EnValue = t.EnValue
                }).ToList();

            model.Categories = _categoryBusiness.GetList().Where(x => x.Users.Any(c => c.Id == MUser.Id)).
                Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Code = c.Code,
                    ParentId = c.ParentId
                })
                .ToList();
            model.Categories.ForEach(x => x.ParentId = x.ParentId > 0 && model.Categories.Any(y => y.Id == x.ParentId) ? x.ParentId : 0);
            model.Sites = _siteBusiness.GetList().Where(x => x.Users.Any(c => c.Id == MUser.Id))
                .Select(x => new SiteOnlyTitle { Id = x.Id, SiteUrl = x.SiteUrl, SiteTitle = x.SiteTitle }).ToList();

            model.RecomendTags = _recomendBiz.Tags(10, model.Tags.Select(t => t.Id).ToList()).Select(t => new TagViewModel()
            {
                Id = t.Id,
                Title = t.Title,
                EnValue = t.EnValue
            }).ToList();
            model.RecomendCats = _recomendBiz.Cats(5).Select(c => new CategoryViewModel()
            {
                Id = c.Id,
                Title = c.Title,
                Code = c.Code
            }).ToList();
            model.RecomendSites = _recomendBiz.Sites(10, model.Sites.Select(s => s.Id).ToList());

            return View("Index." + CmsConfig.ThemeName, model);
        }

        [HttpGet]
        public virtual ActionResult CatsUsersDeleting(string CatCode)
        {
            try
            {
                var user = _userBusiness.GetList().SingleOrDefault(x => x.UserName == User.Identity.Name);
                var cat = user.Categories.SingleOrDefault(c => c.Title == CatCode || c.Code == CatCode);
                user.Categories.Remove(cat);
                _userBusiness.Update(user);
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
                var context = new TazehaContext();
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
                var context = new TazehaContext();
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
                var context = new TazehaContext();
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
                var context = new TazehaContext();
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
                var context = new TazehaContext();
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
            return _userBusiness.IsUserFlow(EntityCode, User.Identity.Name, Content);
        }
    }
}
