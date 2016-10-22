using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Linq;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Web.Models;
using Mn.NewsCms.Web.Areas.Dashboard.Models;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Web.WebLogic.BaseModel;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class FeedController : BaseAdminController
    {
        private readonly ICategoryBusiness _categoryBusiness;
        private readonly IFeedBusiness _feedBusiness;

        public FeedController(ICategoryBusiness categoryBusiness, IFeedBusiness feedBusiness)
        {
            _categoryBusiness = categoryBusiness;
            _feedBusiness = feedBusiness;
        }

        public virtual ActionResult Index(long? siteId, long? catId, string term)
        {
            var model = new AdminFeedViewModel();
            model.SiteId = siteId;
            model.CatId = catId;
            model.Cats = _categoryBusiness.GetList().ToList().Select(c => new SelectListItem() { Text = c.Title, Value = c.Id.ToString() }).ToList();
            model.Term = term;

            model.GridMenu = new ColumnActionMenu(new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Mn.NewsCms.Common.Resources.General.Edit, "openModal('/Dashboard/Feed/CreateEdit/#=Id#')"));

            return View(model);
        }
        public virtual JsonResult Feeds_Read([DataSourceRequest] DataSourceRequest request, long? siteId, long? catId, string term)
        {
            var query = _feedBusiness.GetList();
            if (siteId.HasValue && siteId > 0)
                query = query.Where(f => f.SiteId == siteId);
            if (catId.HasValue && catId > 0)
                query = query.Where(f => f.Categories.Any(c => c.Id == catId.Value || c.ParentId == catId.Value));
            else if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(f => f.Site.SiteUrl.StartsWith(term));
            }
            //if (request.Filters == null || !request.Filters.Any())
            //    query = query.Take(20);

            var feeds = query.Select(f => new FeedViewModel()
            {
                Id = f.Id,
                Title = f.Title,
                Link = f.Link,
                SiteId = f.SiteId,
                SiteSiteTitle = f.Site.SiteTitle,
                SiteSiteUrl = f.Site.SiteUrl,
                Deleted = f.Deleted,
                UpdateDurationTitle = f.UpdateDuration.Title,
                UpdateDurationId = f.UpdateDurationId,
                LastUpdateDateTime = f.LastUpdateDateTime,
                LastUpdaterVisit = f.LastUpdaterVisit
            });

            return Json(feeds.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual ActionResult CreateEdit(long? Id)
        {
            FeedViewModel model;
            if (Id.HasValue)
            {
                var dbFeed = _feedBusiness.Get(Id.Value);
                model = dbFeed.ToViewModel<FeedViewModel>();
                model.SelectedCategories = dbFeed.Categories.Select(c => c.Id).ToList();
                model.SelectedTags = dbFeed.Tags.Select(c => c.Id).ToList();
                ViewBag.Tags = dbFeed.Tags.Select(c => new TagViewModel { Id = c.Id, Title = c.Title }).ToList();
            }
            else
                model = new FeedViewModel();

            var cats = _categoryBusiness.GetList().Where(c => c.Active).Select(c => new MnTitleValue() { Title = c.Title, Value = c.Id }).ToList();
            ViewBag.Categories = cats;

            return View(model);
        }

        [HttpPost]
        public virtual JsonResult CreateEdit(FeedViewModel model)
        {
            OperationStatus res;
            var feeddb = new Feed();
            if (model.Id > 0)
            {
                feeddb =_feedBusiness.Get(model.Id);

                var lastUpdateDateTime = feeddb.LastUpdateDateTime;
                var lastUpdaterVisit = feeddb.LastUpdaterVisit;

                feeddb = model.ToModel<Feed>(feeddb);

                feeddb.LastUpdateDateTime = lastUpdateDateTime;
                feeddb.LastUpdaterVisit = lastUpdaterVisit;

            }
            else
                feeddb = model.ToModel<Feed>();

            if (model.SelectedCategories != null)
                feeddb.Categories.AddEntities(Ioc.CatBiz.GetList(model.SelectedCategories.ToList()).ToList());
            if (model.SelectedTags != null)
                feeddb.Tags.AddEntities(_tagBusiness.GetList().Where(t => model.SelectedTags.Contains(t.Id)).ToList());

            res = _feedBusiness.CreateEdit(feeddb);

            return Json(res.ToJOperationResult(), JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult Feed_CreateEdit(Feed feed)
        {
            OperationStatus status;
            if (feed.Id == 0)
                status = _feedBusiness.CreateEdit(feed);
            else
                status = _feedBusiness.Edit(feed);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
    }
}