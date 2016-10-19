using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.NewsCms.Web.Models;
using Mn.NewsCms.Common;
using Mn.Framework.Web.Mvc;
using Mn.Framework.Web.Model;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class CategoryController : BaseAdminController
    {
        [Authorize(Roles = "admin")]
        public virtual ActionResult Index()
        {
            var model = new PageGridModel();
            model.GridMenu = new ColumnActionMenu(new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Mn.NewsCms.Common.Resource.General.Edit, "openModal('/Dashboard/Category/Manage/#=Id#')"),
                new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Mn.NewsCms.Common.Resource.General.Delete, "deleteGridRow('/Dashboard/Category/Delete/#=Id#')"));

            return View(model);
        }

        public virtual JsonNetResult Category_Read([DataSourceRequest] DataSourceRequest request, long? feedId)
        {
            var query = Ioc.CatBiz.GetList();
            if (feedId.HasValue)
                query = Ioc.CatBiz.GetList().Where(c => c.Feeds.Any(f => f.Id == feedId));

            var cats = query.Select(c => new CategoryViewModel()
            {
                Id = c.Id,
                Code = c.Code,
                Title = c.Title,
                Color = c.Color,              
                Priority = c.Priority,
                ViewMode = c.ViewMode
            }).ToList();

            return JsonNet(cats.ToDataSourceResult(request));
        }

        public virtual JsonResult AddFeedToCat(int catId, int feedId)
        {
            var feed = Ioc.FeedBiz.Get(feedId);
            var cat = Ioc.CatBiz.Get(catId);
            feed.Categories.Add(cat);
            Ioc.FeedBiz.Edit(feed);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult RemoveFeedFromCat(int feedId)//int catId, long feedId
        {
            var feed = Ioc.FeedBiz.Get(feedId);
            var cat = Ioc.CatBiz.Get(int.Parse(Request.Params["Id"]));
            feed.Categories.Remove(cat);
            Ioc.FeedBiz.Edit(feed);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual ActionResult Manage(int Id)
        {
            var model = new CategoryModel();
            if (Id != 0)
                model = Ioc.CatBiz.Get(Id).ToViewModel<CategoryModel>();

            var cats = Ioc.CatBiz.GetList().Where(c => c.Active).Select(c => new DropDownListItem() { Value = c.Id.ToString(), Text = c.Title }).ToList();
            cats.Insert(0, new DropDownListItem() { Text = "بدون سرگروه", Value = "0" });
            ViewBag.Cats = cats;

            return View(model);
        }

        [HttpPost]
        public virtual JsonResult Manage(Category category)
        {
            var res = Ioc.CatBiz.CreateEdit(category);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}